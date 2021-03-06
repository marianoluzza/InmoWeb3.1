using Inmobiliaria_.Net_Core.Models;
using InmoWeb3._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InmoWeb3._1.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly DataContext context;

		public HomeController(ILogger<HomeController> logger, DataContext context)
		{
			_logger = logger;
			this.context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public async Task<IActionResult> Grupos()
		{
			var lista = await context.Grupos.ToListAsync();
			return View(lista);
		}

		public IActionResult Generar()
		{
			var q = from t in Assembly.GetExecutingAssembly().GetTypes()
					where t.IsClass && t.Namespace == "InmoWeb3._1.Models" && t.IsSubclassOf(typeof(EntidadBase))
					select t;
			return View(q.ToList());
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
