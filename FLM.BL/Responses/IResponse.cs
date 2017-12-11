using System;

namespace FLM.BL.Responses
{
	public interface IResponse
	{
		String Message { get; set; }
		Boolean IsError { get; set; }
		String ErrorMessage { get; set; }
	}
}