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

    [Authorize(Policy = "Admin")]
    [ApiController]
	[Route("api/[controller]")]
	public class GruposController : Controller
	{
		private readonly ILogger<GruposController> _logger;
		private readonly DataContext _context;
        private readonly IConfiguration config;

        public GruposController(ILogger<GruposController> logger, DataContext context, IConfiguration config)
		{
			_logger = logger;
			_context = context;
            this.config = config;
        }

        // GET api/<controller>/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var res = await _context.Grupos.Where(e => e.Id == id).FirstOrDefaultAsync();
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
        
        // GET api/<controller>/5
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var res = await _context.Grupos.ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Grupo entidad)
        {
            try
            {
                _context.Grupos.Add(entidad);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = entidad.Id }, entidad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Grupo entidad)
        {
            try
            {
                _context.Grupos.Update(entidad);
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
                _context.Grupos.Remove(new Grupo { Id = id });
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
