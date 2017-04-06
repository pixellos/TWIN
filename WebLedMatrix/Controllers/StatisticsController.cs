using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebLedMatrix.Models;

namespace WebLedMatrix.Controllers
{
    public class StatisticsController : System.Web.Http.ApiController
    {
        private IList<StatisticsViewModel> Session { get; }
        public StatisticsController(IList<StatisticsViewModel> session)
        {
            this.Session = session;
        }

        [HttpGet]
        [Route("Stats/All/{page:int?}&{entiresPerPage:int?}")]
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
                toShow = this.Session.Skip((page - 1) * entriesPerPage).Take(entriesPerPage);
            }
            return toShow;
        }

        [HttpGet]
        [Route("Stats/User/{user}/{page:int?}/{entiresPerPage:int?}")]
        public IEnumerable<StatisticsViewModel> UserSummary(string user, int page = 0, int entriesPerPage = 50)
        {
            IEnumerable<StatisticsViewModel> toShow;
            var values = this.Session.Where(x => x.Name == user);
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
                toShow = values.Take(entriesPerPage);
            }
            else
            {
                toShow = values.Skip((page - 1) * entriesPerPage).Take(entriesPerPage);
            }
            return toShow;
        }
    }
}   