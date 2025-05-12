using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<int>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoleCommandHandler(
            IRoleRepository roleRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<int>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (await _roleRepository.IsNameUniqueAsync(request.Name))
            {
                return Result<int>.Failure("Role name already exists");
            }

            var role = new Domain.AggregateModels.UserAggregate.Role(request.Name, request.Description);

            await _roleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<int>.Success(role.Id);
        }
    }
} 