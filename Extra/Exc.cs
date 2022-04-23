using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoWeb3._1.Extra
{
	public class Exc
	{
		public Exc(Exception e)
		{
			Message = e.Message;
			InnerMessage = e.InnerException?.Message;
			StackTrace = e.StackTrace;
		}
		public string Message { get; set; }
		public string InnerMessage { get; set; }
		public string StackTrace { get; set; }
	}
}
