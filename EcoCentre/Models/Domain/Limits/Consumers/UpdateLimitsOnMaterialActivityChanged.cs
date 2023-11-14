using System;
using System.Linq;
using EcoCentre.Models.Domain.Materials;
using EcoCentre.Models.Domain.Materials.Events;
using MassTransit;
using MongoDB.Driver;

namespace EcoCentre.Models.Domain.Limits.Consumers
{
	public class UpdateLimitsOnMaterialActivityChanged : Consumes<MaterialActivityChanged>.All
	{
		private readonly Repository<LimitStatus> _limitsRepository;

		public UpdateLimitsOnMaterialActivityChanged(Repository<LimitStatus> limitsRepository)
		{
			_limitsRepository = limitsRepository;
		}

		public void Consume(MaterialActivityChanged message)
		{
			var today = DateTime.Now;
			var query = Builders<LimitStatus>.Filter.ElemMatch(x => x.Limits, Builders<LimitStatusYear>.Filter.And(
				Builders<LimitStatusYear>.Filter.Eq(y => y.Year, today.Year),
				Builders<LimitStatusYear>.Filter.ElemMatch(y => y.Materials, Builders<LimitStatusMaterial>.Filter.Eq(m => m.MaterialId, message.MaterialId))));

			var items = _limitsRepository.Collection.Find(query).ToList();

			foreach (var item in items)
			{
				var currentLimit = item.Limits.First(x => x.Year == today.Year);
				var material = currentLimit.Materials.First(x => x.MaterialId == message.MaterialId);
				material.IsActive = message.IsActive;
				_limitsRepository.Save(item);
			}
		}
	}

}