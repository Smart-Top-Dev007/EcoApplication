using System;
using System.Linq;
using EcoCentre.Models.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EcoCentre.Models.Domain
{
	public class TaskRepository
	{
		private readonly UnitOfWork _unitOfWork;

		public TaskRepository(UnitOfWork unitOfWork )
		{
			_unitOfWork = unitOfWork;
		}

		public T Find<T>(Type taskType) where T : BackgroundTaskData, new()
		{
			var name = taskType.Name;
            return FindTaskByName<T>(name);
		}

        public T FindTaskByName<T>(string name) where T : BackgroundTaskData, new()
        {
            var taskList = _unitOfWork.Database.GetCollection<T>("BackgroundTasks").AsQueryable().Where(x => x.Name == name).ToList();
            while (taskList.Count > 1)
            {
                RemoveTask(taskList.Last());
                taskList.RemoveAt(taskList.Count - 1);
            }

            return taskList.FirstOrDefault() ?? new T { Name = name, Id = ObjectId.GenerateNewId().ToString()};
        }

        public void RemoveTask<T>(T task) where T : BackgroundTaskData, new()
        {
            _unitOfWork.Database.GetCollection<T>("BackgroundTasks").DeleteOne(x => x.Id == task.Id);
        }

		public void Save(BackgroundTaskData data)
		{
			_unitOfWork.Database.GetCollection<BackgroundTaskData>("BackgroundTasks").ReplaceOne(x=>x.Id == data.Id, data,new UpdateOptions{IsUpsert = true});
		}
	}
}