using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Application.Services.User;
using HotelBooking.Domain.AggregateModels.UserAggregate;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.CQRS.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserHotelRepository _userHotelRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IHotelRepository hotelRepository,
            IUserHotelRepository userHotelRepository,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            ICurrentUserService currentUserService,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _hotelRepository = hotelRepository;
            _userHotelRepository = userHotelRepository;
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get current user's role
                var currentUser = await _userRepository.GetByIdAsync(_currentUserService.UserId);
                if (currentUser == null)
                {
                    return Result<int>.Failure("Current user not found");
                }

                // Check if current user has permission to create the requested role
                if (!CanCreateRole(currentUser.RoleId, request.RoleId))
                {
                    return Result<int>.Failure("You don't have permission to create users with this role");
                }

                if (!await _userRepository.IsEmailUniqueAsync(request.Email))
                {
                    return Result<int>.Failure("Email already exists");
                }

                var password = GenerateDefaultPassword();
                var hashedPassword = _passwordHasher.HashPassword(password);

                var user = new Domain.AggregateModels.UserAggregate.User(
                    request.Email,
                    hashedPassword,
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.RoleId
                );

                await _userRepository.AddAsync(user);

                // If current user is hotel manager, only allow assigning to their hotels
                if (currentUser.RoleId == (int)Domain.Utils.Enum.Role.HotelManager)
                {
                    var userHotels = await _userHotelRepository.FindAsync(u => u.UserId == currentUser.Id);
                    var allowedHotelIds = userHotels.Select(uh => uh.HotelId).ToList();
                    
                    // Filter hotel IDs to only include hotels the manager has access to
                    var validHotelIds = request.HotelIds.Intersect(allowedHotelIds).ToList();
                    
                    foreach (var hotelId in validHotelIds)
                    {
                        var hotel = await _hotelRepository.GetByIdAsync(hotelId);
                        if (hotel != null)
                        {
                            var userHotel = new UserHotel(user, hotel);
                            await _userHotelRepository.AddAsync(userHotel);
                        }
                    }
                }
                else
                {
                    // Super admin can assign to any hotels
                    foreach (var hotelId in request.HotelIds)
                    {
                        var hotel = await _hotelRepository.GetByIdAsync(hotelId);
                        if (hotel != null)
                        {
                            var userHotel = new UserHotel(user, hotel);
                            await _userHotelRepository.AddAsync(userHotel);
                        }
                    }
                }

                // Send welcome email
                await SendWelcomeEmailAsync(user, password);

                return Result<int>.Success(user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return Result<int>.Failure("An error occurred while creating the user");
            }
        }

        private bool CanCreateRole(int currentUserRoleId, int requestedRoleId)
        {
            // Super admin can create any role
            if (currentUserRoleId == (int)Domain.Utils.Enum.Role.SuperAdmin)
            {
                return true;
            }

            // Hotel manager can only create frontdesk users
            if (currentUserRoleId == (int)Domain.Utils.Enum.Role.HotelManager)
            {
                return requestedRoleId == (int)Domain.Utils.Enum.Role.FrontDesk;
            }

            return false;
        }

        private string GenerateDefaultPassword()
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string numberChars = "0123456789";
            const string specialChars = "#?!@$%^&*-";
            const int length = 8;

            var random = new Random();
            var password = new char[length];

            // Ensure at least one of each required character type
            password[0] = upperChars[random.Next(upperChars.Length)];
            password[1] = lowerChars[random.Next(lowerChars.Length)];
            password[2] = numberChars[random.Next(numberChars.Length)];
            password[3] = specialChars[random.Next(specialChars.Length)];

            // Fill the rest with random characters
            var allChars = upperChars + lowerChars + numberChars + specialChars;
            for (int i = 4; i < length; i++)
            {
                password[i] = allChars[random.Next(allChars.Length)];
            }

            // Shuffle the password
            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }

            return new string(password);
        }

        private async Task SendWelcomeEmailAsync(Domain.AggregateModels.UserAggregate.User user, string password)
        {
            try
            {
                var subject = "Welcome to Hotel Booking System";
                var html = $@"
                    <html>
                    <body>
                        <h2>Welcome to Hotel Booking System!</h2>
                        <p>Dear {user.FirstName} {user.LastName},</p>
                        <p>Your account has been created successfully. You can now log in to the system using the following credentials:</p>
                        <p><strong>Email:</strong> {user.Email}</p>
                        <p><strong>Password:</strong> {password}</p>
                        <p>For security reasons, please change your password after your first login.</p>
                        <p>Best regards,<br>Hotel Booking System Team</p>
                    </body>
                    </html>";

                await _emailService.SendEmailAsync(user.Email, subject, html);
                _logger.LogInformation($"Welcome email sent to {user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending welcome email to {user.Email}");
                // Don't throw the exception as we don't want to fail user creation if email fails
            }
        }
    }
} 