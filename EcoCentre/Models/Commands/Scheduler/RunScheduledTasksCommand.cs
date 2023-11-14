using System;
using System.Collections.Generic;
using NLog;

namespace EcoCentre.Models.Commands.Scheduler
{
    public class RunScheduledTasksCommand
    {
        private readonly IEnumerable<ITask> _backgroundTasks;
	    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public RunScheduledTasksCommand(IEnumerable<ITask> backgroundTasks)
        {
            _backgroundTasks = backgroundTasks;
        }

        public void Execute()
        {
            var executionTime = DateTime.UtcNow;
            foreach (var task in _backgroundTasks)
            {
                try
                {
                    task.Execute(executionTime);
                }
                catch (Exception e)
                {

					Logger.Error(e, "Failed to run task: " + task.GetType().Name);
				}
            }

        }
    }
}