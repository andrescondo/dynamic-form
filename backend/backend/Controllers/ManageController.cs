using System;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models.Request;
using backend.Models.Response;
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

        [HttpGet("all-form",Name = "GetAllForm")]
        public async Task<IActionResult> Get()
        {
            var data = await _manageService.GetAllFormAsync();
            if (data.Error)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        [HttpGet("form/{id}", Name = "GetForm")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _manageService.GetFormAsync(id);
            if (data.Error)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

        [HttpPost("form/create", Name = "PostCreateForm")]
        public async Task<IActionResult> Post(CreateTableRequest form)
        {
            ResponseGeneral data = await _manageService.PostCreateForm(form);
            if (data.Error) {
                return BadRequest(data);
            }
            return Ok(data);
        }

        [HttpPut("form/edit", Name = "PostEditForm")]
        public async Task<IActionResult> Put(EditTableRequest form)
        {
            ResponseGeneral data = await _manageService.PostEditForm(form);
            if (data.Error)
            {
                return BadRequest(data);
            }
            return Ok(data);
        }

    }
}

