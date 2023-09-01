﻿using Microsoft.AspNetCore.Mvc;

namespace SampleWebApiAspNetCore.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CSGOController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("2.0");
        }
    }
}
