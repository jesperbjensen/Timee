﻿using System;
using Nancy.Hosting.Self;

namespace Timee
{
    public class Timee
    {
        private NancyHost _nancyHost;

        public void Start()
        {
            _nancyHost = new NancyHost(new Uri("http://localhost:8888/"), new Uri("http://127.0.0.1:8888/"));
            _nancyHost.Start();
        }

        public void Stop()
        {
            _nancyHost.Stop();
        }
    }
}