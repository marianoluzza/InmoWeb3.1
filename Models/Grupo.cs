using System;

namespace InmoWeb3._1.Models
{
	public class Grupo
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Email { get; set; }
		public DateTime Fecha { get; set; }

		public override string ToString()
		{
			return $"Grupo Nº {Id} {Nombre}";
		}
	}
}
