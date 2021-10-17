using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InmoWeb3._1.Extra
{
	public class MySqlGen
	{
		public string Create(Type tipo)
		{
			string res = $"CREATE TABLE `{tipo.Name + "s"}` ({Environment.NewLine}";
			foreach (var c in tipo.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
			{
				res += $"\t`{c.Name}` {GetTipo(c)} ,{Environment.NewLine}";
			}
			res += $"\tPRIMARY KEY (`id`),{Environment.NewLine}" +
				$"\tKEY `gestioo_tipo_id` (`gestioo_tipo_id`),{Environment.NewLine}" +
				$"\tCONSTRAINT `{tipo.Name + "s"}_otro_id` FOREIGN KEY(`clave_id`) REFERENCES `otra_tabla` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION{Environment.NewLine}" +
				$") ENGINE=InnoDB;";
			return res;
		}

		public string GetTipo(PropertyInfo p)
		{
			bool esId = p.Name == "Id";
			switch (p.PropertyType.Name.ToLower())
			{
				case "int32":
				case "int":
					return $"INT NOT NULL {(esId? "AUTOINCREMENT": "DEFAULT 0")}";
				case "datetime":
					return "DATETIME";
				default:
					return "VARCHAR(200) NOT NULL DEFAULT ''";
			}
		}
	}
}
