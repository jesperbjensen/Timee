using Nancy;

namespace Timee
{
    public class TimmeNancyModule : NancyModule
    {
        public TimmeNancyModule()
        {
            Get["/"] = _ => View["index"];
        }
    }
}