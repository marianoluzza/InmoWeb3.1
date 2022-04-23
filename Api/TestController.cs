using Inmobiliaria_.Net_Core.Models;
using InmoWeb3._1.Extra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		private readonly DataContext context;

		public TestController(DataContext context)
		{
			this.context = context;
		}

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

		[Authorize(Policy = "Admin")]
		[HttpPost("sql/{tabla}")]
		public async Task<IActionResult> Sql([FromBody]string sql, string tabla)
		{
			try
			{

			tabla = tabla ?? "";
			switch (tabla.ToLower())
			{
				case "propietario":
				case "propietarios":
					return Ok(await context.Propietarios.FromSqlRaw(sql).ToListAsync());
				case "inmueble":
				case "inmuebles":
					return Ok(await context.Inmuebles.FromSqlRaw(sql).ToListAsync());
				case "inquilino":
				case "inquilinos":
					return Ok(await context.Inquilinos.FromSqlRaw(sql).ToListAsync());
				case "contrato":
				case "contratos":
					return Ok(await context.Contratos.FromSqlRaw(sql).ToListAsync());
				case "pago":
				case "pagos":
					return Ok(await context.Pagos.FromSqlRaw(sql).ToListAsync());
				case "grupo":
				case "grupos":
					return Ok(await context.Grupos.FromSqlRaw(sql).ToListAsync());
				default:
					return BadRequest();
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new Exc(ex));
			}
		}
	}
}
