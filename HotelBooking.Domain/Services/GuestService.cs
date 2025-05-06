using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Guest;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Domain.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;

        public GuestService(IGuestRepository guestRepository, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GuestDTO>> GetGuestByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<GuestDTO>();
            try
            {
                var guest = await _guestRepository.GetByIdAsync(id);
                if (guest == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Guest not found";
                    return serviceResponse;
                }

                serviceResponse.Data = _mapper.Map<GuestDTO>(guest);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get guest successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GuestDTO>> GetGuestByIdentityNumberAsync(string identityNumber)
        {
            var serviceResponse = new ServiceResponse<GuestDTO>();
            try
            {
                var guest = await _guestRepository.GetByIdentityNumberAsync(identityNumber);
                if (guest == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Guest not found";
                    return serviceResponse;
                }

                serviceResponse.Data = _mapper.Map<GuestDTO>(guest);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get guest successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GuestDTO>>> GetAllGuestsAsync()
        {
            var serviceResponse = new ServiceResponse<List<GuestDTO>>();
            try
            {
                var guests = await _guestRepository.GetAllAsync();
                serviceResponse.Data = _mapper.Map<List<GuestDTO>>(guests);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get all guests successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GuestDTO>> CreateGuestAsync(CreateGuestDTO guestDTO)
        {
            var serviceResponse = new ServiceResponse<GuestDTO>();
            try
            {
                // Check if guest with same identity number already exists
                var existingGuest = await _guestRepository.GetByIdentityNumberAsync(guestDTO.IdentityNumber);
                if (existingGuest != null)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Guest with this identity number already exists";
                    return serviceResponse;
                }

                var guest = _mapper.Map<GuestModel>(guestDTO);
                await _guestRepository.AddAsync(guest);
                serviceResponse.Data = _mapper.Map<GuestDTO>(guest);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Create guest successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GuestDTO>> UpdateGuestAsync(int id, CreateGuestDTO guestDTO)
        {
            var serviceResponse = new ServiceResponse<GuestDTO>();
            try
            {
                var guest = await _guestRepository.GetByIdAsync(id);
                if (guest == null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Guest not found";
                    return serviceResponse;
                }

                // Check if another guest with same identity number exists
                var existingGuest = await _guestRepository.GetByIdentityNumberAsync(guestDTO.IdentityNumber);
                if (existingGuest != null && existingGuest.Id != id)
                {
                    serviceResponse.ResponseType = EResponseType.BadRequest;
                    serviceResponse.Message = "Another guest with this identity number already exists";
                    return serviceResponse;
                }

                _mapper.Map(guestDTO, guest);
                await _guestRepository.UpdateAsync(guest);
                serviceResponse.Data = _mapper.Map<GuestDTO>(guest);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Update guest successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteGuestAsync(int id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var guest = await _guestRepository.GetByIdAsync(id);
                if (guest != null)
                {
                    serviceResponse.ResponseType = EResponseType.NotFound;
                    serviceResponse.Message = "Guest not found";
                    return serviceResponse;
                }

                await _guestRepository.SoftDeleteAsync(guest);
                serviceResponse.Data = true;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Delete guest successfully!";
            }
            catch (Exception ex)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
} 