using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoWeb3._1
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>//la api web valida con token
			{
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = Configuration["TokenAuthentication:Issuer"],
					ValidAudience = Configuration["TokenAuthentication:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
						Configuration["TokenAuthentication:SecretKey"])),
				};
			});
			services.AddAuthorization(options =>
			{
				//options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
				options.AddPolicy("Admin", policy => policy.RequireRole("SuperAdministrador"));
			});
			services.AddControllersWithViews();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			//services.AddNewtonsoftJson(x =>
			//				x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			/* PARA MySql - usando Pomelo */
			services.AddDbContext<DataContext>(
				options => options.UseMySql(Configuration["ConnectionStrings:DefaultConnection"])
				//options => options.UseMySql(Configuration["ConnectionStrings:Local"])
			);
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials()
						//.AllowAnyOrigin()
						.SetIsOriginAllowed((host) => true) //allow all connections (including Signalr)
					);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors("CorsPolicy");
			app.UseDeveloperExceptionPage();
			/*
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}*/
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
