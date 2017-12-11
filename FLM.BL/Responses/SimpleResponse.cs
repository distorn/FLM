using System;

namespace FLM.BL.Responses
{
	public class SimpleResponse : IResponse
	{
		public String Message { get; set; }
		public Boolean IsError { get; set; }
		public String ErrorMessage { get; set; }
	}
}