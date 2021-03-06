using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmoWeb3._1.Models
{
	[Table("grupos")]
	public class Grupo
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Email { get; set; }
		public DateTime Fecha { get; set; }

		public override string ToString()
		{
			return $"Grupo N? {Id} {Nombre}";
		}
	}
}
