using System;

namespace EcoCentre.Models
{
    public interface IAdminTask
    {
        int Id { get; }
        void Execute();
        decimal Progress { get; }
        TimeSpan? ExecutionTime { get; }
        bool IsRunning { get; }
        TimeSpan? EstimatedTime { get; }
    }
}