using System;

namespace EcoCentre.Models.Commands.Scheduler
{
	public class ScheduledTaskException:Exception
	{
		public ScheduledTaskException(ITask task, Exception innerException) : base("Failed to run task: " + task.GetType().Name,innerException)
		{
		}
	}
}