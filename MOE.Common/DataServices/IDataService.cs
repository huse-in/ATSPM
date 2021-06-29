using MOE.Common.Business.SiteSecurity;
using MOE.Common.Models;
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
        IGenericTableController<Signal> SignalController { get; }
    }

    //example SQL Data Service
    public class SqlDataService : IDataService
    {
        private static SPM _dbc;

        public SqlDataService(SPM dbc)
        {
            _dbc = dbc;
        }

        public IGenericTableController<Signal> SignalController {get; private set;} = new SQLTableController<Signal>(_dbc.Signals);
    }

    //example Web Data Service
    public class WebDataService : IDataService
    {
        private static Uri _webService;

        public WebDataService(Uri webService)
        {
            _webService = webService;
        }

        public IGenericTableController<Signal> SignalController { get; private set; } = new WebTableController<Signal>(_webService);
    }
}
