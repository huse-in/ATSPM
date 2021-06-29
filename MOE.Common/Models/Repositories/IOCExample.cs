using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MOE.Common.DataServices;

namespace MOE.Common.Models.Repositories
{
    public class IOCExample
    {
        public IOCExample()
        {
            //if the CommonServiceLocator.ServiceLocator provider has not been set, set it to an IServiceLocator implementation
            //in this case IServiceLocator is from MvvmLight's IoC implementation
            //if the ServiceLocator is already set, you don't want to set it again
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => (IServiceLocator)SimpleIoc.Default);

                //register all your services and stuff here:

                //to register an entry you can map an interface to an object like this:
                SimpleIoc.Default.Register<IControllerEventLogRepository, ControllerEventLogRepository>();

                //you can state that you want that object created right away, default is object is created when first called
                //if it's already been created it doesn't create it again
                SimpleIoc.Default.Register<IControllerEventLogRepository, ControllerEventLogRepository>(true);

                //if your object has a constructor or if you need to do some instructions on object creation you can pass over a factory function
                SimpleIoc.Default.Register<IControllerEventLogRepository>(() => new ControllerEventLogRepository());

                //You can register multiple instances of IControllerEventLogRepository by giving it an instance name
                SimpleIoc.Default.Register<IControllerEventLogRepository>(() => new ControllerEventLogRepository(), "Production");
                SimpleIoc.Default.Register<IControllerEventLogRepository>(() => new ControllerEventLogRepositoryTest(), "Testing");


                //this is how you can register different data services
                SimpleIoc.Default.Register<IDataService>(() => new SqlDataService(new SPM()), "SQLDatabase");
                SimpleIoc.Default.Register<IDataService>(() => new WebDataService(new Uri("https://coolwebservice.com")), "WebDatabase");
            }
        }

        public void TestServiceLocator()
        {
            //once the CommonServiceLocator.ServiceLocator provider has been set you can get an instance of whatever you want, where you want it
            //instead of using ControllerEventLogRepositoryFactory.Create(); you would use this:
            IControllerEventLogRepository CELR = ServiceLocator.Current.GetInstance<IControllerEventLogRepository>();

            //you can get a specific instance by key
            IControllerEventLogRepository CELRProduction = ServiceLocator.Current.GetInstance<IControllerEventLogRepository>("Production");
            IControllerEventLogRepository CELRTesting = ServiceLocator.Current.GetInstance<IControllerEventLogRepository>("Testing");

            //you can also get ALL instances of a type
            IEnumerable<IControllerEventLogRepository> CELRAll = ServiceLocator.Current.GetAllInstances<IControllerEventLogRepository>();
        }
    }

    public class ControllerEventLogRepositoryTest : IControllerEventLogRepository
    {
        public bool CheckForRecords(string signalId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetAllAggregationCodes(string signalId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public int GetApproachEventsCountBetweenDates(int approachId, DateTime startTime, DateTime endTime, int phaseNumber)
        {
            throw new NotImplementedException();
        }

        public int GetDetectorActivationCount(string signalId, DateTime startTime, DateTime endTime, int detectorChannel)
        {
            throw new NotImplementedException();
        }

        public int GetEventCountByEventCodesParamDateTimeRange(string signalId, DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute, List<int> eventCodes, int param)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetEventsBetweenDates(DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetEventsByEventCodesParam(string signalId, DateTime startTime, DateTime endTime, List<int> eventCodes, int param)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetEventsByEventCodesParamDateTimeRange(string signalId, DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute, List<int> eventCodes, int param)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetEventsByEventCodesParamWithLatencyCorrection(string signalId, DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double latencyCorrection)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(string signalId, DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset, double latencyCorrection)
        {
            throw new NotImplementedException();
        }

        public Controller_Event_Log GetFirstEventBeforeDate(string signalId, int eventCode, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Controller_Event_Log GetFirstEventBeforeDateByEventCodeAndParameter(string signalId, int eventCode, int eventParam, DateTime date)
        {
            throw new NotImplementedException();
        }

        public DateTime GetMostRecentRecordTimestamp(string signalID)
        {
            throw new NotImplementedException();
        }

        public int GetRecordCount(string signalId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public int GetRecordCountByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime, List<int> eventParameters, List<int> events)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetRecordsByParameterAndEvent(string signalId, DateTime startTime, DateTime endTime, List<int> eventParameters, List<int> eventCodes)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetSignalEventsBetweenDates(string signalId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetSignalEventsByEventCode(string signalId, DateTime startTime, DateTime endTime, int eventCode)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetSignalEventsByEventCodes(string signalId, DateTime startTime, DateTime endTime, List<int> eventCodes)
        {
            throw new NotImplementedException();
        }

        public int GetSignalEventsCountBetweenDates(string signalId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetSplitEvents(string signalId, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public double GetTmcVolume(DateTime startDate, DateTime endDate, string signalId, int phase)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetTopEventsAfterDateByEventCodesParam(string signalId, DateTime timestamp, List<int> eventCodes, int param, int top)
        {
            throw new NotImplementedException();
        }

        public List<Controller_Event_Log> GetTopNumberOfSignalEventsBetweenDates(string signalId, int numberOfRecords, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
