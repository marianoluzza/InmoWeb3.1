using System;

namespace InmoWeb3._1.Models
{
	public class Pago : EntidadBase
	{
		public int Numero { get; set; }
		public DateTime Fecha { get; set; }

		public int ContratoId { get; set; }
		public Contrato Contrato { get; set; }

		public override string ToString()
		{
			return "Pago Nº " + Numero;
		}
	}
}
