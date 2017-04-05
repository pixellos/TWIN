using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLedMatrix.Models;

namespace WebLedMatrix.Controllers
{
    public class StatisticsController : ApiController
    {
        private IList<StatisticsViewModel> Session { get; }
        public StatisticsController(IList<StatisticsViewModel> statistics)
        {
            this.Session = statistics;
        }

        [Route("Stats/Summary/{page}/{entiresPerPage}")]
        public IEnumerable<StatisticsViewModel> Summary(int page = 0, int entriesPerPage = 50)
        {
            IEnumerable<StatisticsViewModel> toShow;
            if (page < 0)
            {
                throw new ArgumentException(nameof(page));
            }
            if (entriesPerPage < 1)
            {
                throw new ArgumentException(nameof(entriesPerPage));
            }
            if (page == 0)
            {
                toShow = this.Session.Take(entriesPerPage);
            }
            else
            {
                toShow = this.Session.Skip((page -1) * entriesPerPage).Take(entriesPerPage);
            }
            return toShow;
        }

        public object Summary1(int page = 0)
        {
            var result = this.Summary(entriesPerPage: 5);
            return null;
        }
    }
}   