using Inmobiliaria_.Net_Core.Models;
using InmoWeb3._1.Extra;
using InmoWeb3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmoWeb3._1.Controllers
{

	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class PagosController : Controller
	{
		private readonly ILogger<PagosController> _logger;
		private readonly DataContext _context;
        private readonly IConfiguration config;
        private readonly int miGrupo;
        private readonly string identidad;

        public PagosController(ILogger<PagosController> logger, DataContext context, IConfiguration config)
		{
			_logger = logger;
			_context = context;
            this.config = config;
            miGrupo = User.Identity.Grupo();
            identidad = User.Identity.Name;
        }


        // GET: api/<controller>
        [Authorize(Policy = "Admin")]
        [HttpGet("{grupo}")]
        public async Task<IActionResult> GetByGrupo(int grupo)
        {
            try
            {
                var res = await _context.Pagos.Include(e => e.Contrato).ThenInclude(e => e.Inmueble)
                    .Where(e => e.GrupoId == grupo).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // GET: api/<controller>
        [HttpGet("{grupo}")]
        public async Task<IActionResult> GetByContrato(int contratoId)
        {
            try
            {
                var res = await _context.Pagos.Include(e => e.Contrato).ThenInclude(e => e.Inmueble)
                    .Where(e => e.ContratoId == contratoId && e.GrupoId == miGrupo)
                    .ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var res = await _context.Pagos.Include(e => e.Contrato).ThenInclude(e => e.Inmueble)
                    .Where(e => e.Id == id && e.GrupoId == miGrupo)
                    .FirstOrDefaultAsync();
                if (res == null)
                    return NotFound();
                else
                    return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<controller>
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Pago entidad)
        {
            try
            {
                _context.Pagos.Add(entidad);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<controller>/5
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Pago entidad)
        {
            try
            {
                _context.Pagos.Update(entidad);
                await _context.SaveChangesAsync();
                return Ok(entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _context.Pagos.Remove(new Pago { Id = id });
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
