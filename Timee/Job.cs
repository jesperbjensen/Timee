using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Timee
{
    public class Job
    {
        private object SaveLock = new object();
        
        public Job()
        {
            Runs = new List<Run>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Active { get; set; }
        public TimeSpan? Interval { get; set; }
        public List<Run> Runs { get; set; }

        public string Status { 
            get
            {
                if (!Active)
                    return "Deactivated";

                if (JobManager.GetRunningJobs().Any(x=>x == this))
                    return "Running";

                return "Waiting";
            }
        }

        public Run AddRun()
        {
            var run = new Run(this);
            Runs.Add(run);
            run.Started();
            Save();
            return run;
        }


        public DateTimeOffset? LastRun 
        { 
            get
            {
                var run = LastFinishedRun;
                if (run == null)
                    return null;

                return run.StartedAt;
            }
        }

        public JobStatus? LastStatus
        {
            get
            {
                var run = LastFinishedRun;
                if (run == null)
                    return null;

                return run.Status;
            }
        }

        public TimeSpan? LastRunTime
        {
            get
            {
                var run = LastFinishedRun;
                if (run == null)
                    return null;

                return run.RunTime;
            }
        }

        public Run LastFinishedRun
        {
            get
            {
                return Runs.LastOrDefault(x => x.FinishedAt.HasValue);
            }
        }

        public Run[] GetRuns()
        {
            return Runs.ToArray();
        }

        public void Save()
        {
            lock (SaveLock)
            {
                try
                {
                    File.WriteAllText(JobManager.DataDirectory + "/" + Id.ToString() + ".json", JsonConvert.SerializeObject(this, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    JobManager.Log(this, "Unable to save", ex);
                    throw;
                }
            }
        }
    }
}
