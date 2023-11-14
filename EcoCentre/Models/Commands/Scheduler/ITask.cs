using System;

namespace EcoCentre.Models.Commands.Scheduler
{
    public interface ITask
    {
        void Execute(DateTime execTime, bool force = false);
    }

}