using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmoWeb3._1.Models
{
	[Table("inmuebles")]
	public class Inmueble : EntidadBase
	{
		public string Direccion { get; set; }
		public int Superficie { get; set; }
		public decimal Latitud { get; set; }
		public decimal Longitud { get; set; }

		public int PropietarioId { get; set; }
		public Propietario Propietario { get; set; }

		public override string ToString()
		{
			return Direccion ?? "N/D";
		}
	}
}
