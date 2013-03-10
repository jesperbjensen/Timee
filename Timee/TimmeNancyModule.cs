using System;
using Nancy;
using Nancy.Responses;

namespace Timee
{
    public class TimmeNancyModule : NancyModule
    {
        public TimmeNancyModule()
        {
            Get["/"] = _ => View["index"];
            Get["/activate/{id}"] = _ => { JobManager.Activate((Guid)_.id); return new RedirectResponse("/"); };
            Get["/deactivate/{id}"] = _ => { JobManager.Deactivate((Guid)_.id); return new RedirectResponse("/"); };
            Get["/details/{id}"] = _ => View["details", JobManager.GetJob((_.id))];
            Get["/add"] = _ => View["add", new Job() { Interval = TimeSpan.FromHours(1)}];
            Post["/add"] = CreateJob;
            Get["/edit/{id}"] = _ => View["edit", JobManager.GetJob((_.id))];
            Post["/edit/{id}"] = UpdateJob;
        }

        private dynamic CreateJob(dynamic arg)
        {
            Job job = new Job();
            job.Name = Request.Form["Name"];
            job.Url = Request.Form["Url"];
            job.Interval = TimeSpan.Parse(Request.Form["Interval"]);
            job.Active = false;
         
            JobManager.AddJob(job);

            return new RedirectResponse("/");
        }
        private dynamic UpdateJob(dynamic arg)
        {
            Job job = JobManager.GetJob(arg.id);
            job.Name = Request.Form["Name"];
            job.Url = Request.Form["Url"];
            job.Interval = TimeSpan.Parse(Request.Form["Interval"]);
            job.Save();

            return new RedirectResponse("/");
        }
    }
}