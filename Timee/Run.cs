using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timee
{
    public class Run
    {
        private readonly Job _job;

        public Run(Job job)
        {
            _job = job;
        }

        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public JobStatus? Status { get; set; }
        public string Message { get; set; }
        public string ExceptionText { get; set; }

        public TimeSpan? RunTime { 
            get
            {
                if (FinishedAt == null || StartedAt == null)
                    return null;
                return (FinishedAt - StartedAt);
            } 
        }

        public void Failed(string exception)
        {
            ExceptionText = exception;
            Status = JobStatus.Fail;
            JobManager.Log(_job, "Failed");
            _job.Save();
        }

        
        public void Finish()
        {
            FinishedAt = DateTime.Now;

            if (Status == null)
                Status = JobStatus.Success;

            JobManager.Log(_job, "Finished");

            _job.Save();
        }

        public void Started()
        {
            JobManager.Log(_job, "Started");
            StartedAt = DateTimeOffset.Now;
        }
    }
}
