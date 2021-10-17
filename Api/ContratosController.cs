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
	public class ContratosController : Controller
	{
		private readonly ILogger<ContratosController> _logger;
		private readonly DataContext _context;
        private readonly IConfiguration config;
        private readonly int miGrupo;
        private readonly string identidad;

        public ContratosController(ILogger<ContratosController> logger, DataContext context, IConfiguration config)
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
                var res = await _context.Contratos.Include(e => e.Inquilino).Include(e => e.Inmueble).ThenInclude(e => e.Propietario)
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
        public async Task<IActionResult> GetByInmueble(int inmuebleId)
        {
            try
            {
                var res = await _context.Contratos.Include(e => e.Inquilino).Include(e => e.Inmueble).ThenInclude(e => e.Propietario)
                    .Where(e => e.InmuebleId == inmuebleId && e.GrupoId == miGrupo && e.Inmueble.Propietario.Email == identidad)
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
                var res = await _context.Contratos.Include(e => e.Inquilino).Include(e => e.Inmueble).ThenInclude(e => e.Propietario)
                    .Where(e => e.Id == id && e.GrupoId == miGrupo && e.Inmueble.Propietario.Email == identidad)
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
        public async Task<IActionResult> Post([FromBody]Contrato entidad)
        {
            try
            {
                _context.Contratos.Add(entidad);
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
        public async Task<IActionResult> Put(int id, [FromBody]Contrato entidad)
        {
            try
            {
                _context.Contratos.Update(entidad);
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
                _context.Contratos.Remove(new Contrato { Id = id });
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
