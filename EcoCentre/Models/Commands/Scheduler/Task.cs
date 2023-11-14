using System;
using EcoCentre.Models.Domain;

namespace EcoCentre.Models.Commands.Scheduler
{
    public class BgTaskInterval
    {
        public TimeSpan Interval { get; set; }
    }
    public abstract class Task<TData> : ITask where TData : BackgroundTaskData, new()
    {
        private readonly TaskRepository _taskRepository;
        public TData Data { get; private set; }
        public TimeSpan Interval { get; set; }
        protected Task(TaskRepository taskRepository, BgTaskInterval interval)
        {
            Interval = interval.Interval;//new TimeSpan(0,0,5,0);

            _taskRepository = taskRepository;
            Data = _taskRepository.Find<TData>(GetType());
        }

        protected abstract void DoWork(DateTime execTime);
        

        public void Execute(DateTime execTime, bool force = false)
        {
            if ((Data.LastRun + Interval > execTime) && !force)
                return;
            DoWork(execTime);
            Data.LastRun = execTime;
            _taskRepository.Save(Data);
        }
    }
}