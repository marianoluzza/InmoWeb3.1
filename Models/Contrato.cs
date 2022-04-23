using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmoWeb3._1.Models
{
	[Table("contratos")]
	public class Contrato : EntidadBase
	{
		public DateTime Desde { get; set; }
		public DateTime Hasta { get; set; }

		public int InmuebleId { get; set; }
		public Inmueble Inmueble { get; set; }
		public int InquilinoId { get; set; }
		public Inquilino Inquilino { get; set; }

		public override string ToString()
		{
			var a = Inmueble?.Propietario?.Nombre;
			var b = Inquilino?.Nombre;
			return (a ?? "N/D") + " / " + (b ?? "N/D");
		}
	}
}
