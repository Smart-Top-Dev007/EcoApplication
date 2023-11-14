using System;

namespace EcoCentre.Models.Domain
{
    public class BackgroundTaskData :Entity
    {
        public string Name { get; set; }
        public DateTime LastRun { get; set; }

    }
}