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
	public class PropietariosController : Controller
	{
		private readonly ILogger<PropietariosController> _logger;
		private readonly DataContext _context;
        private readonly IConfiguration config;

        public PropietariosController(ILogger<PropietariosController> logger, DataContext context, IConfiguration config)
		{
			_logger = logger;
			_context = context;
            this.config = config;

        }

        // GET: api/<controller>
        [HttpGet("{id=0:int:max(0)}")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var g = User.Identity.Grupo();
                var res = await _context.Propietarios.Where(e => e.Email == usuario && e.GrupoId == g).FirstOrDefaultAsync();
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

        // GET api/<controller>/5
        [Authorize(Policy = "Admin")]
        [HttpGet(@"{s:regex(^([[^.@]]+)(\.[[^.@]]+)*@([[^.@]]+\.)+([[^.@]]+)$)}")]
        public async Task<IActionResult> Get(string s)
        {
            try
            {
                var res = await _context.Propietarios.Where(e => e.Email == s).FirstOrDefaultAsync();
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

        // GET api/<controller>/5
        [Authorize(Policy = "Admin")]
        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var res = await _context.Propietarios.Where(e => e.Id == id).FirstOrDefaultAsync();
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
        public async Task<IActionResult> Post([FromBody]Propietario entidad)
        {
            try
            {
                //entidad.GrupoId = User.Identity.Grupo();
                entidad.Clave = Hash(entidad.Clave);
                _context.Propietarios.Add(entidad);
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
        public async Task<IActionResult> Put(int id, [FromBody]Propietario entidad)
        {
            try
            {
                var g = User.Identity.Grupo();
                if (_context.Propietarios.AsNoTracking().FirstOrDefault(e => e.Id == id && e.GrupoId == g) != null)
                {
                    entidad.Id = id;
                    var ent = _context.Entry<Propietario>(entidad);
                    ent.State = EntityState.Modified;
                    if (String.IsNullOrWhiteSpace(entidad.Clave))
                    {
                        ent.Property(nameof(Propietario.Clave)).IsModified = false;
                    }
                    else
                    {
                        entidad.Clave = Hash(entidad.Clave);
                    }
                    //_context.Propietarios.Update(entidad);
                    await _context.SaveChangesAsync();
                    return Ok(entidad);
                }
                return BadRequest();
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
                var g = User.Identity.Grupo();
                if (_context.Propietarios.AsNoTracking().FirstOrDefault(e => e.Id == id && e.GrupoId == g) != null)
                {
                    _context.Propietarios.Remove(new Propietario { Id = id});
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest();
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

        // POST api/<controller>/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginView loginView)
        {
            try
            {
                string hashed = Hash(loginView.Clave);
                var p = await _context.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Email o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("Nombre", p.Nombre),
                        new Claim("Id", p.Id.ToString()),
                        new Claim("Grupo", p.GrupoId.ToString()),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(7),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new Exc(ex));
            }
        }

        private string Hash(string pass)
		{
            var h = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pass,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
            return h;
        }
    }
}
