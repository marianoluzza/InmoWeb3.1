using System;

namespace InmoWeb3._1.Models
{
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
