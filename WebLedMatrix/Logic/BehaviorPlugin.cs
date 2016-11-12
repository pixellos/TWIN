using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLedMatrix.Logic
{
    public interface IPugin
    {
        IEnumerable<Delegate> delegates { get; set; }
    }

    public class Plugin : IPugin
    {
        public IEnumerable<Delegate> delegates { get; set; } = new HashSet<Delegate>();
        public Plugin()
        {

        }
    }
    public class BehaviorPlugin
    {

    }
}