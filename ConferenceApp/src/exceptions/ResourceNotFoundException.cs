using System;

namespace ConferenceApp.exceptions
{
	public class ResourceNotFoundException : Exception
	{
		public ResourceNotFoundException() : base("Resource not found exception!") { }
		public ResourceNotFoundException(int id) : base($"Resource with ID {id} was not found.") { }
	}
}
