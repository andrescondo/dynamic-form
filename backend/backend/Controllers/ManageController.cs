using System;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
//using backend.Models;
namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageController : ControllerBase
    {
        private readonly IManageService _manageService;
        private readonly ILogger<ManageController> _logger;
        //private readonly IValidateApiPermissionService _permissionService;

        public ManageController(IManageService manageService)
		{
            _manageService = manageService;
		}

        [HttpGet(Name = "GetAllForm")]
        public async Task<IActionResult> Get()
        {
            var data = await _manageService.GetAllFormAsync();
            return Ok(data);
        }


	}
}

