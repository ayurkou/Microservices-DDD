using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Accounting.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar()
        {
            throw new NotImplementedException();
        }

        [HttpGet("avatar")]
        public async Task<IActionResult> GetAvatar()
        {
            throw new NotImplementedException();
        }

        [HttpGet("workhistory")]
        public async Task<IActionResult> GetWorkHistory()
        {
            throw new NotImplementedException();
        }

        [HttpPost("workhistory")]
        public async Task<IActionResult> AddWorkHistory()
        {
            throw new NotImplementedException();
        }

        [HttpGet("workhistory/{id}")]
        public async Task<IActionResult> GetWorkHistory(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("workhistory/{id}")]
        public async Task<IActionResult> UpdateWorkHistory(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("workhistory/{id}")]
        public async Task<IActionResult> DeleteWorkHistory(int id)
        {
            throw new NotImplementedException();
        }
    }
}