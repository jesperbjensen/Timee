using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Timee
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<Timee>(s =>
                {
                    s.ConstructUsing(name => new Timee());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("The little job service that could.");
                x.SetDisplayName("Timee");
                x.SetServiceName("Timee");
            });    
        }
    }
}
