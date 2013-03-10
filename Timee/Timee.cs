using System;
using System.Configuration;
using System.Linq;
using Nancy.Hosting.Self;

namespace Timee
{
    public class Timee
    {
        private NancyHost _nancyHost;

        public void Start()
        {
            JobManager.Load();

            _nancyHost = new NancyHost(ConfigurationManager.AppSettings["hosts"].Split(',').Select(x=>new Uri(x)).ToArray());
            _nancyHost.Start();
            JobManager.Start();
        }

        public void Stop()
        {
            _nancyHost.Stop();
            JobManager.Stop();
        }
    }
}