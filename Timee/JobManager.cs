using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Timee
{
    public static class JobManager
    {
        private static readonly List<Job> Jobs = new List<Job>();
        private static readonly List<Job> JobsRunning = new List<Job>();
        private static IObservable<DateTime> _clock;
        public static string DataDirectory { get; set; }
 
        static JobManager()
        {
            string fullPath = System.Reflection.Assembly.GetAssembly(typeof (JobManager)).Location;
            string dataDirectory = Path.GetDirectoryName(fullPath) + "/data";
            if (!Directory.Exists(dataDirectory)) Directory.CreateDirectory(dataDirectory);

            DataDirectory = dataDirectory;
        }

        public static void AddJob(Job job)
        {
            Jobs.Add(job);
            job.Save();
        }

        public static void Start()
        {
            _clock = from _ in Observable.Interval(TimeSpan.FromSeconds(1))
                        select DateTime.Now;

            _clock.Subscribe(now =>
                {
                    Action[] actions;
                    lock(JobsRunning)
                    {
                        IEnumerable<Job> jobsToRun;
                        jobsToRun = from item in Jobs
                                        where ShouldRun(item)
                                        select item;

                        actions = jobsToRun.Select(x => new Action(() => Run(x))).ToArray();
                        JobsRunning.AddRange(jobsToRun);
                    }
                    Parallel.Invoke(actions);
                });
        }

        private static bool ShouldRun(Job job)
        {
            DateTimeOffset lastRun = (job.LastRun ?? DateTimeOffset.MinValue);
            return !JobsRunning.Contains(job) && job.Active && (DateTimeOffset.Now - lastRun) > job.Interval;
        }

        public static void Stop()
        {
            // Top observable?
        }

        private static void Run(Job job)
        {
            var run = job.AddRun();

            var client = new WebClient();
            try
            {
                System.Threading.Thread.Sleep(1000);
                run.Message = client.DownloadString(job.Url);
            }
            catch (Exception ex)
            {
                run.Failed(ex.ToString());
            }

            var endTime = DateTimeOffset.Now;

            run.Finish();

            JobsRunning.Remove(job);
        }

        public static Job[] ListJobs()
        {
            return Jobs.ToArray();
        }

        public static Job[] GetRunningJobs()
        {
            return JobsRunning.ToArray();
        }

        public static void Deactivate(Guid id)
        {
            var job = Jobs.Single(x => x.Id == id);
            job.Active = false;
            Log(job, "Deactivated");
        }

        public static void Activate(Guid id)
        {
            var job = Jobs.Single(x => x.Id == id);
            job.Active = true;
            Log(job, "Activated");
        }

        public static void Log(string message)
        {
            Console.WriteLine("{0}", message);
        }

        public static void Log(Job job, string message)
        {
            Console.WriteLine("Job {0} - '{1}': {2}", job.Id, job.Name, message);
        }

        public static void Log(Job job, string message, Exception ex)
        {
            Console.WriteLine("Job {0} - '{1}': {2}\n{3}", job.Id, job.Name, message, ex);
        }

        public static Job GetJob(Guid id)
        {
            return Jobs.Single(x => x.Id == id);
        }

        public static void Load()
        {
            foreach(var file in Directory.GetFiles(DataDirectory, "*.json"))
            {
                var job = JsonConvert.DeserializeObject<Job>(File.ReadAllText(file));
                AddJob(job);
            }
        }

    }
}
