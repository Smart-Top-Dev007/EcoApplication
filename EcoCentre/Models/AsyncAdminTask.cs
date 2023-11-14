using System;
using EcoCentre.Models.Commands.Scheduler;

namespace EcoCentre.Models
{
    public abstract class AsyncAdminTask : IAdminTask
    {

        private DateTime Started { get; set; }

        public int Id
        {
            get { return GetHashCode(); }
        }

        public void Execute()
        {
            if (IsRunning) return;
            new System.Threading.Tasks.Task<int>(ExecuteAsync).Start();
        }

        private int ExecuteAsync()
        {
            try
            {
                IsRunning = true;
                Started = DateTime.UtcNow;
                DoWork();
            }catch(Exception e)
            {

                ErrorLog.Log(e);
            }
            finally
            {
                IsRunning = false;

            }
            return 0;
        }
        protected abstract void DoWork();
        public decimal Progress { get; set; }

        public TimeSpan? ExecutionTime
        {
            get
            {
                if (IsRunning) return DateTime.UtcNow - Started;
                return null;
            }
        }

        public TimeSpan? EstimatedTime
        {
            get
            {

                if (IsRunning && ExecutionTime != null)
                {
                    if (Progress == 0)
                        return null;
                    return new TimeSpan((long)(ExecutionTime.Value.Ticks / Progress));
                }
                return null;
            }
        }

        public bool IsRunning { get; set; }
    }
}