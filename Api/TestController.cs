using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoWeb3._1.Api
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		[AllowAnonymous]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(new { Mensaje = "Anduvo", Valor = new Random().Next(0,10000) });
		}

		[HttpGet("token")]
		public IActionResult Token()
		{
			dynamic exo = new System.Dynamic.ExpandoObject();
			foreach (var c in User.Claims)
			{
				((IDictionary<String, Object>)exo).Add(c.Type, c.Value);
			}
			return Ok(exo);
		}
	}
}
