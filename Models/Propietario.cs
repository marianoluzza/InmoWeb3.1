using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmoWeb3._1.Models
{
	[Table("propietarios")]
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
