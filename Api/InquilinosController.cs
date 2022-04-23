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
	public class InquilinosController : Controller
	{
		private readonly ILogger<InquilinosController> _logger;
		private readonly DataContext _context;
        private readonly IConfiguration config;

        public InquilinosController(ILogger<InquilinosController> logger, DataContext context, IConfiguration config)
		{
			_logger = logger;
			_context = context;
            this.config = config;

        }


        // GET: api/<controller>
        [Authorize(Policy = "Admin")]
        [HttpGet("{grupo}")]
        public async Task<IActionResult> GetByGrupo(int grupo)
        {
            try
            {
                var res = await _context.Inquilinos.Where(e => e.GrupoId == grupo).ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // GET api/<controller>/5
        [Authorize(Policy = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var res = await _context.Inquilinos.Where(e => e.Id == id).FirstOrDefaultAsync();
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
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Inquilino entidad)
        {
            try
            {
                _context.Inquilinos.Add(entidad);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        // PUT api/<controller>/5
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Inquilino entidad)
        {
            try
            {
                _context.Inquilinos.Update(entidad);
                await _context.SaveChangesAsync();
                return Ok(entidad);
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
                _context.Inquilinos.Remove(new Inquilino { Id = id });
                await _context.SaveChangesAsync();
                return Ok();
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
