using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Role.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetRoleByIdQueryHandler(
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.Id);
            if (role == null)
            {
                return Result<RoleDto>.Failure("Role not found");
            }

            var roleDto = _mapper.Map<RoleDto>(role);
            return Result<RoleDto>.Success(roleDto);
        }
    }
} 