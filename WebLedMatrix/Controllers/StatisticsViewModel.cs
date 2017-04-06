using System;

namespace WebLedMatrix.Controllers
{
    public class StatisticsViewModel
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime Commands { get; set; }
    }
}