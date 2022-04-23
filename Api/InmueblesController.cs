using Inmobiliaria_.Net_Core.Models;
using InmoWeb3._1.Extra;
using InmoWeb3._1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
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
	public class InmueblesController : Controller
	{
		private readonly ILogger<InmueblesController> _logger;
		private readonly DataContext _context;
        private readonly IConfiguration config;
        private readonly int miGrupo, miId;
        private readonly string identidad;

        public InmueblesController(ILogger<InmueblesController> logger, DataContext context, IConfiguration config, IHttpContextAccessor contextAccessor)
		{
			_logger = logger;
			_context = context;
            this.config = config;
            miGrupo = contextAccessor.HttpContext.User.Identity.Grupo();
            miId = contextAccessor.HttpContext.User.Identity.MiId();
            identidad = contextAccessor.HttpContext.User.Identity.Name;
        }

        // GET: api/<controller>
        [Authorize(Policy = "Admin")]
        [HttpGet("g/{grupo}")]
        public async Task<IActionResult> GetByGrupo(int grupo)
        {
            try
            {
                var res = await _context.Inmuebles.Where(e => e.GrupoId == grupo).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // GET: api/<controller>
        [HttpGet("{id:int:max(0)}")]
        public async Task<IActionResult> GetAll(int id)
        {
            try
            {
                var res = await _context.Inmuebles.Where(e => e.PropietarioId == miId).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                
                var res = await _context.Inmuebles.Include(e => e.Propietario)
                    .Where(e => e.Id == id && e.GrupoId == miGrupo && e.Propietario.Email == identidad)
                    .FirstOrDefaultAsync();
                if (res == null)
                    return NotFound();
                else
                    return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Inmueble entidad)
        {
            try
            {
                entidad.PropietarioId = miId;
                entidad.GrupoId = miGrupo;
                _context.Inmuebles.Add(entidad);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Inmueble entidad)
        {
            try
            {
                entidad.Id = id;
                if (_context.Inmuebles.AsNoTracking()
                    .FirstOrDefault(e => e.Id == entidad.Id && e.GrupoId == miGrupo && e.PropietarioId == miId) != null)
                {
                    _context.Inmuebles.Update(entidad);
                    await _context.SaveChangesAsync();
                    return Ok(entidad);
                }
                else return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (_context.Inmuebles.AsNoTracking()
                    .FirstOrDefault(e => e.Id == id && e.GrupoId == miGrupo && e.PropietarioId == miId) != null)
                {
                    _context.Inmuebles.Remove(new Inmueble { Id = id });
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else return BadRequest();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return BadRequest(new Exc(ex));
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }
    }
}
