using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            // Apply filters
            if (!request.IncludeInactive)
            {
                users = users.Where(u => !u.IsDeleted).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                users = users.Where(u =>
                    u.Email.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.PhoneNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(request.RoleName))
            {
                users = users.Where(u => u.UserRoles.Any(ur => ur.Role.Name == request.RoleName)).ToList();
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                users = request.SortBy.ToLower() switch
                {
                    "email" => request.SortDescending 
                        ? users.OrderByDescending(u => u.Email).ToList()
                        : users.OrderBy(u => u.Email).ToList(),
                    "firstname" => request.SortDescending
                        ? users.OrderByDescending(u => u.FirstName).ToList()
                        : users.OrderBy(u => u.FirstName).ToList(),
                    "lastname" => request.SortDescending
                        ? users.OrderByDescending(u => u.LastName).ToList()
                        : users.OrderBy(u => u.LastName).ToList(),
                    "phone" => request.SortDescending
                        ? users.OrderByDescending(u => u.PhoneNumber).ToList()
                        : users.OrderBy(u => u.PhoneNumber).ToList(),
                    _ => users
                };
            }

            // Apply pagination
            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                users = users
                    .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .ToList();
            }

            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Result<List<UserDto>>.Success(userDtos);
        }
    }
} 