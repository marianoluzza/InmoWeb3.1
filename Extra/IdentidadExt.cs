using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace InmoWeb3._1.Extra
{
	public static class IdentidadExt
	{
		public static int Grupo([NotNull] this IIdentity source)
		{
			int res = 0;
			if (source is ClaimsIdentity)
			{
				var r = (source as ClaimsIdentity).Claims.FirstOrDefault<Claim>(x => x.Type.ToLower() == "grupo")?.Value;
				int.TryParse(r, out res);
			}
			return res;
		}
		public static int MiId([NotNull] this IIdentity source)
		{
			int res = 0;
			if (source is ClaimsIdentity)
			{
				var r = (source as ClaimsIdentity).Claims.FirstOrDefault<Claim>(x => x.Type.ToLower() == "id")?.Value;
				int.TryParse(r, out res);
			}
			return res;
		}

	}
}
