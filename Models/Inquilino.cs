using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmoWeb3._1.Models
{
	[Table("inquilinos")]
	public class Inquilino : EntidadBase
	{
		public string Nombre { get; set; }
		public string Telefono { get; set; }

		public override string ToString()
		{
			return Nombre ?? "N/D";
		}
	}
}
