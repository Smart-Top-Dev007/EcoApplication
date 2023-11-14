using System;

namespace EcoCentre.Models.Infrastructure
{
	public class NotFoundException : DomainException
	{
		public NotFoundException()
		{
		}
		
		public NotFoundException(string message)
			: base(message)
		{
		}

		public NotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public NotFoundException(Type type, string id)
			: base($"Entity of type {type.Name} with id '{id}' was not found.")
		{
		}
	}
}