using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Domain.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepository branchRepository, IMapper mapper)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<BranchDTO>>> GetAllAsync()
        {
            var serviceResponse = new ServiceResponse<List<BranchDTO>>();

            var branches = await _branchRepository.GetAllAsync();
            if (branches != null && branches.Count > 0)
            {
                serviceResponse.Data = _mapper.Map<List<BranchDTO>>(branches);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get all branches successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No branches found.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<PagingReturnModel<BranchDTO>>> GetPagedAsync(int pageIndex, int pageSize, string search = null)
        {
            var serviceResponse = new ServiceResponse<PagingReturnModel<BranchDTO>>();

            var pagedResult = await _branchRepository.GetPagedAsync(pageIndex, pageSize, search);
            if (pagedResult != null && pagedResult.Items.Any())
            {
                var mappedItems = _mapper.Map<List<BranchDTO>>(pagedResult.Items);
                var mappedResult = new PagingReturnModel<BranchDTO>(
                    mappedItems,
                    pagedResult.TotalCount,
                    pagedResult.CurrentPage,
                    pagedResult.PageSize
                );

                serviceResponse.Data = mappedResult;
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Get paged branches successfully!";
            }
            else
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "No branches found.";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<BranchDetailsDTO>> FindByIdAsync(int id)
        {
            var serviceResponse = new ServiceResponse<BranchDetailsDTO>();
            var branch = await _branchRepository.FindByIdAsync(id);
            if (branch == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Không tìm thấy cơ sở";
                return serviceResponse;
            }

            serviceResponse.Data = _mapper.Map<BranchDetailsDTO>(branch);
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Get branch successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<BranchCreateDTO>> SaveAsync(BranchCreateDTO branchDTO)
        {
            var serviceResponse = new ServiceResponse<BranchCreateDTO>();
            try
            {
                var branch = _mapper.Map<BranchModel>(branchDTO);

                var savedBranch = await _branchRepository.AddAsync(branch);

                serviceResponse.Data = _mapper.Map<BranchCreateDTO>(savedBranch);
                serviceResponse.ResponseType = EResponseType.Success;
                serviceResponse.Message = "Save branch successfully!";
                return serviceResponse;
            }
            catch (Exception e)
            {
                serviceResponse.ResponseType = EResponseType.InternalError;
                serviceResponse.Message = e.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<BranchDetailsDTO>> UpdateAsync(int id, BranchDetailsDTO branchDTO)
        {
            var serviceResponse = new ServiceResponse<BranchDetailsDTO>();
            var existingBranch = await _branchRepository.FindByIdAsync(id);
            if (existingBranch == null)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Không tìm thấy cơ sở";
                return serviceResponse;
            }

            _mapper.Map(branchDTO, existingBranch);
            var updatedBranch = await _branchRepository.UpdateAsync(existingBranch);
            
            serviceResponse.Data = _mapper.Map<BranchDetailsDTO>(updatedBranch);
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Update branch successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            var serviceResponse = new ServiceResponse<bool>();
            var result = await _branchRepository.DeleteAsync(id);
            if (!result)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Không tìm thấy cơ sở";
                return serviceResponse;
            }

            serviceResponse.Data = true;
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Delete branch successfully!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteMultipleAsync(int[] ids)
        {
            var serviceResponse = new ServiceResponse<bool>();
            var result = await _branchRepository.DeleteMultipleAsync(ids);
            if (!result)
            {
                serviceResponse.ResponseType = EResponseType.NotFound;
                serviceResponse.Message = "Không tìm thấy cơ sở";
                return serviceResponse;
            }

            serviceResponse.Data = true;
            serviceResponse.ResponseType = EResponseType.Success;
            serviceResponse.Message = "Delete branches successfully!";
            return serviceResponse;
        }
    }
} 