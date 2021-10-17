using System;

namespace InmoWeb3._1.Models
{
	public class Propietario : EntidadBase
	{
		public string Nombre { get; set; }
		public string Email { get; set; }
		public string Clave { get; set; }
		public string Telefono { get; set; }

		public override string ToString()
		{
			return Nombre ?? "N/D";
		}
	}
}
