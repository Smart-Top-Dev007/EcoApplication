using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace EcoCentre.Models.Infrastructure
{
	public static class ValidationExtensions
	{
		public static IRuleBuilderOptions<T, IEnumerable<TProperty>> NotEmptyCollection<T, TProperty>(
			this IRuleBuilder<T, IEnumerable<TProperty>> ruleBuilder)
		{
			return ruleBuilder.Must(x => x != null && x.Any())
				.WithMessage("'{PropertyName}' ne doit pas être vide.");
		}
	}
}