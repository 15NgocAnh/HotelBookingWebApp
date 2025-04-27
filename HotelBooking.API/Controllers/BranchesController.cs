using Asp.Versioning;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchesController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBranches([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = null)
        {
            var serviceResponse = await _branchService.GetPagedAsync(pageIndex, pageSize, search);
            return Ok(serviceResponse.getData());
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var serviceResponse = await _branchService.GetAllAsync();
            return Ok(serviceResponse.getData());
        }


        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetBranchById(int id)
        {
            var serviceResponse = await _branchService.FindByIdAsync(id);
            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(BranchCreateDTO branch)
        {
            var serviceResponse = await _branchService.SaveAsync(branch);
            return Ok(serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBranch(int id, [FromBody] BranchDetailsDTO branch)
        {
            var serviceResponse = await _branchService.UpdateAsync(id, branch);
            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBranch(int id)
        {
            var serviceResponse = await _branchService.DeleteAsync(id);
            return Ok(serviceResponse.getMessage());
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteMultipleBranches([FromBody] int[] ids)
        {
            var serviceResponse = await _branchService.DeleteMultipleAsync(ids);
            return Ok(serviceResponse.getMessage());
        }
    }
} 