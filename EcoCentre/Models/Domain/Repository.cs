using System.Collections.Generic;
using System.Linq;
using EcoCentre.Models.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EcoCentre.Models.Domain
{
	public class Repository<T> where T : Entity
	{
		private readonly UnitOfWork _unitOfWork;

		public Repository(UnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		private string _collectionName;
		public string CollectionName
		{
			get
			{
				if (string.IsNullOrEmpty(_collectionName))
				{
					_collectionName = typeof(T).Name;
				}
				return _collectionName;
			}
		}

		public IMongoCollection<T> Collection => _unitOfWork.Database.GetCollection<T>(CollectionName);

		public T FindOne(string id)
		{
			if (string.IsNullOrEmpty(id) || id.Length != 24)
				return null;
			return Collection.AsQueryable().FirstOrDefault(x => x.Id == id);
		}
		public List<T> GetMany(IList<string> ids)
		{
			var result = FindMany(ids);

			var missingIds = result.Select(x => x.Id).Except(ids).ToList();
			if (missingIds.Any())
			{
				throw new NotFoundException($"Entity of type {typeof(T).Name} with ids '{ids.JoinBy(", ")}' was not found.");
			}

			return result;
		}

		private List<T> FindMany(IList<string> ids)
		{
			return Collection.AsQueryable().Where(x => ids.Contains(x.Id)).ToList();
		}

		public void Save(T entity)
		{
			if (string.IsNullOrWhiteSpace(entity.Id))
			{
				Collection.InsertOne(entity);
			}
			else
			{
				Collection.ReplaceOne(new BsonDocument("_id", new ObjectId(entity.Id)), entity,
					new UpdateOptions { IsUpsert = true });
			}
		}

		public IQueryable<T> Query => Collection.AsQueryable();

		public void Remove(T entity)
		{
			Collection.DeleteOne(x => x.Id == entity.Id);
		}
		public void Insert(T entity)
		{
			Collection.InsertOne(entity);
		}

		public void DropCollection()
		{
			_unitOfWork.Database.DropCollection(CollectionName);
		}

		public void ReIndex()
		{

		}

		public void RemoveAll()
		{
			DropCollection();

		}

		public void InsertBatch(List<T> newRows)
		{
			Collection.InsertMany(newRows);
		}
	}
}