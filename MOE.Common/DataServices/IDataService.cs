using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
using MOE.Common.Models.Extensions;
using MOE.Common.TableController;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.DataServices
{
    public interface IDataService
    {
        IGenericTableController<Controller_Event_Log> EventLogController { get; }
    }

    //example SQL Data Service
    public class SqlDataService : IDataService
    {
        private static SPM _dbc;

        public SqlDataService(SPM dbc)
        {
            _dbc = dbc;
        }

        public IGenericTableController<Controller_Event_Log> EventLogController { get; private set;} = new SQLTableController<Controller_Event_Log>(_dbc.Controller_Event_Log);


        public void TestMethods()
        {
            int test = 5;
            test.ToPercent(10);

            EventLogController.GetTopEventsAfterDateByEventCodesParam()
        }
    }

    //example Web Data Service
    public class WebDataService : IDataService
    {
        private static Uri _webService;

        public WebDataService(Uri webService)
        {
            _webService = webService;
        }

        public IGenericTableController<Controller_Event_Log> EventLogController { get; private set; } = new WebTableController<Controller_Event_Log>(_webService);
    }
}
