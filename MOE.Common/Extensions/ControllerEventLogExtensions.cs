using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Management;
using Microsoft.EntityFrameworkCore.Internal;
using MOE.Common.Models.Repositories;
using MOE.Common.TableController;

namespace MOE.Common.Models.Extensions
{
    public static class ControllerEventLogExtensions
    {
        public static double ToPercent(this int a, int b)
        {
            return (a / b * 100);
        }

        public static double ToPercent(this double a, double b)
        {
            return (a / b * 100);
        }

        public static int GetRecordCountByParameterAndEvent(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime, DateTime endTime,
            List<int> eventParameters,
            List<int> eventCodes)
        {
            var query = controller.Query.Where(c =>
                c.SignalID == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime);
            if (eventParameters != null && eventParameters.Count > 0)
                query = query.Where(c => eventParameters.Contains(c.EventParam));
            if (eventCodes != null && eventCodes.Count > 0)
                query = query.Where(c => eventCodes.Contains(c.EventCode));
            return query.Count();
        }

        public static List<Controller_Event_Log> GetRecordsByParameterAndEvent(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime,
            DateTime endTime, List<int> eventParameters, List<int> eventCodes)
        {
            var query = controller.Query.Where(c =>
                c.SignalID == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime);
            if (eventParameters != null && eventParameters.Count > 0)
                query = query.Where(c => eventParameters.Contains(c.EventParam));
            if (eventCodes != null && eventCodes.Count > 0)
                query = query.Where(c => eventCodes.Contains(c.EventCode));
            return query.ToList();
        }

        public static List<Controller_Event_Log> GetAllAggregationCodes(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime, DateTime endTime)
        {
            var codes = new List<int> { 150, 114, 113, 112, 105, 102, 1 };
            var records = controller.Query
                .Where(c => c.SignalID == signalId && c.Timestamp >= startTime && c.Timestamp <= endTime &&
                            codes.Contains(c.EventCode))
                .ToList();
            return records;
        }

        public static int GetDetectorActivationCount(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, int detectorChannel)
        {
            var count = (from cel in controller.Query
                         where cel.Timestamp >= startTime
                               && cel.Timestamp < endTime
                               && cel.SignalID == signalId
                               && cel.EventParam == detectorChannel
                               && cel.EventCode == 82
                         select cel).Count();
            return count;
        }

        public static double GetTmcVolume(this IGenericTableController<Controller_Event_Log> controller, DateTime startDate, DateTime endDate, string signalId, int phase)
        {
            var repository =
                SignalsRepositoryFactory.Create();
            var signal = repository.GetVersionOfSignalByDate(signalId, startDate);
            var graphDetectors = signal.GetDetectorsForSignalByPhaseNumber(phase);

            var tmcChannels = new List<int>();
            foreach (var gd in graphDetectors)
                foreach (var dt in gd.DetectionTypes)
                    if (dt.DetectionTypeID == 4)
                        tmcChannels.Add(gd.DetChannel);


            double count = (from cel in controller.Query
                            where cel.Timestamp >= startDate
                                  && cel.Timestamp < endDate
                                  && cel.SignalID == signalId
                                  && tmcChannels.Contains(cel.EventParam)
                                  && cel.EventCode == 82
                            select cel).Count();

            return count;
        }

        public static List<Controller_Event_Log> GetSplitEvents(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime, DateTime endTime)
        {
            var results = (from r in controller.Query
                           where r.SignalID == signalId && r.Timestamp > startTime && r.Timestamp < endTime
                                 && r.EventCode > 130 && r.EventCode < 150
                           select r).ToList();

            return results;
        }

        public static List<Controller_Event_Log> GetSignalEventsBetweenDates(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                return (from r in controller.Query
                        where r.SignalID == signalId
                              && r.Timestamp >= startTime
                              && r.Timestamp < endTime
                        select r).ToList();
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetSignalEventsBetweenDates";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public static List<Controller_Event_Log> GetTopNumberOfSignalEventsBetweenDates(this IGenericTableController<Controller_Event_Log> controller, string signalId, int numberOfRecords,
            DateTime startTime, DateTime endTime)
        {
            try
            {
                var events =
                (from r in controller.Query
                 where r.SignalID == signalId
                       && r.Timestamp >= startTime
                       && r.Timestamp < endTime
                 select r).Take(numberOfRecords).ToList();

                if (events != null)
                    return events;
                var emptyEvents = new List<Controller_Event_Log>();
                return emptyEvents;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetTopNumberOfSignalEventsBetweenDates";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public static int GetRecordCount(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime, DateTime endTime)
        {
            try
            {
                return controller.Query.Count(r => r.SignalID == signalId
                                                           && r.Timestamp >= startTime
                                                           && r.Timestamp < endTime);
            }
             catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetRecordCount";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = signalId + " - " +ex.Message;
                logRepository.Add(e);
                throw ex;
            }
        }

        public static bool CheckForRecords(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime, DateTime endTime)
        {
            try
            {
                return controller.Query.Any(r => r.SignalID == signalId
                                                         && r.Timestamp >= startTime
                                                         && r.Timestamp < endTime);
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "CheckForRecords";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public static List<Controller_Event_Log> GetSignalEventsByEventCode(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, int eventCode)
        {
            try
            {
                return (from r in controller.Query
                        where r.SignalID == signalId
                              && r.Timestamp >= startTime
                              && r.Timestamp < endTime
                              && r.EventCode == eventCode
                        select r).ToList();
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCode";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public static List<Controller_Event_Log> GetSignalEventsByEventCodes(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes)
        {
            try
            {
                var events = (from s in controller.Query
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            //catch (EntityCommandExecutionException ex)
            //{
                
            //}
            
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCodes";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = signalId + " - " + ex.Message;
                logRepository.Add(e);
                throw ex;
            }
        }

        public static List<Controller_Event_Log> GetEventsByEventCodesParam(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime,
            DateTime endTime, List<int> eventCodes, int param)
        {
            try
            {
                var events = controller.Query.Where(s => s.SignalID == signalId &&
                                   s.Timestamp >= startTime &&
                                   s.Timestamp <= endTime &&
                                   s.EventParam == param &&
                                   eventCodes.Contains(s.EventCode)).ToList();
                events = events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventParam).ToList();
                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParam";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public static List<Controller_Event_Log> GetTopEventsAfterDateByEventCodesParam(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime timestamp, List<int> eventCodes, int param, int top)
        {
            try
            {
                var endDate = timestamp.AddHours(1);
                var events = controller.Query.Where(c =>
                    c.SignalID == signalId &&
                    c.Timestamp > timestamp &&
                    c.Timestamp < endDate &&
                    c.EventParam == param &&
                    eventCodes.Contains(c.EventCode)).ToList();
                return events
                    .OrderBy(s => s.Timestamp)
                    .Take(top).ToList(); 
            }
            catch (Exception e)
            {
                var errorLog = ApplicationEventRepositoryFactory.Create();
                //errorLog.QuickAdd(Assembly.GetExecutingAssembly().FullName, GetType().DisplayName(), e.TargetSite.ToString(), ApplicationEvent.SeverityLevels.Low, e.Message);
                return null;
            }
        }


        public static int GetEventCountByEventCodesParamDateTimeRange(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param)
        {
            try
            {
                return
                (from s in controller.Query
                 where s.SignalID == signalId &&
                       s.Timestamp >= startTime &&
                       s.Timestamp <= endTime &&
                       (s.Timestamp.Hour > startHour && s.Timestamp.Hour < endHour ||
                        s.Timestamp.Hour == startHour && s.Timestamp.Hour == endHour &&
                        s.Timestamp.Minute >= startMinute && s.Timestamp.Minute <= endMinute ||
                        s.Timestamp.Hour == startHour && s.Timestamp.Hour < endHour &&
                        s.Timestamp.Minute >= startMinute ||
                        s.Timestamp.Hour < startHour && s.Timestamp.Hour == endHour &&
                        s.Timestamp.Minute <= endMinute)
                       &&
                       s.EventParam == param &&
                       eventCodes.Contains(s.EventCode)
                 select s).Count();
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetEventCountByEventCodesParamDateTimeRange";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public static List<Controller_Event_Log> GetEventsByEventCodesParamDateTimeRange(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, int startHour, int startMinute, int endHour, int endMinute,
            List<int> eventCodes, int param)
        {
            try
            {
                var events = (from s in controller.Query
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    (s.Timestamp.Hour > startHour && s.Timestamp.Hour < endHour ||
                                     s.Timestamp.Hour == startHour && s.Timestamp.Hour == endHour &&
                                     s.Timestamp.Minute >= startMinute && s.Timestamp.Minute <= endMinute ||
                                     s.Timestamp.Hour == startHour && s.Timestamp.Hour < endHour &&
                                     s.Timestamp.Minute >= startMinute ||
                                     s.Timestamp.Hour < startHour && s.Timestamp.Hour == endHour &&
                                     s.Timestamp.Minute <= endMinute)
                                    &&
                                    s.EventParam == param &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetSignalEventsByEventCodesParamDateTimeRange";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }


        public static List<Controller_Event_Log> GetEventsByEventCodesParamWithOffsetAndLatencyCorrection(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double offset,
            double latencyCorrection)
        {

            try
            {
                var events = (from s in controller.Query
                              where s.SignalID == signalId &&
                                    s.Timestamp >= startTime &&
                                    s.Timestamp <= endTime &&
                                    s.EventParam == param &&
                                    eventCodes.Contains(s.EventCode)
                              select s).ToList();
                events.Sort((x, y) => DateTime.Compare(x.Timestamp, y.Timestamp));
                foreach (var cel in events)
                {
                    cel.Timestamp = cel.Timestamp.AddMilliseconds(offset);
                    cel.Timestamp = cel.Timestamp.AddSeconds(0 - latencyCorrection);
                }
                return events;
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public static List<Controller_Event_Log> GetEventsByEventCodesParamWithLatencyCorrection(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            DateTime startTime, DateTime endTime, List<int> eventCodes, int param, double latencyCorrection)
        {
            try
            {
                var events = controller.Query.Where(s => s.SignalID == signalId &&
                          s.Timestamp >= startTime &&
                          s.Timestamp <= endTime &&
                          s.EventParam == param &&
                          eventCodes.Contains(s.EventCode)).ToList();
                foreach (var cel in events)
                {
                    cel.Timestamp = cel.Timestamp.AddSeconds(0 - latencyCorrection);
                }
                return events.OrderBy(e => e.Timestamp).ThenBy(e => e.EventCode).ToList();
            }
            catch (Exception ex)
            {
                var logRepository =
                    ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithLatencyCorrection";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Timestamp = DateTime.Now;
                e.Description = ex.Message;
                logRepository.Add(e);
                throw;
            }
        }

        public static Controller_Event_Log GetFirstEventBeforeDate(this IGenericTableController<Controller_Event_Log> controller, string signalId,
            int eventCode, DateTime date)
        {
            try
            {
                var tempDate = date.AddHours(-1);
                var lastEvent = controller.Query.Where(c => c.SignalID == signalId &&
                                                                    c.Timestamp >= tempDate &&
                                                                    c.Timestamp < date &&
                                                                    c.EventCode == eventCode)
                    .OrderByDescending(c => c.Timestamp).FirstOrDefault();
                return lastEvent;
            }
            catch (Exception ex)
            {
                var logRepository = ApplicationEventRepositoryFactory.Create();
                var e = new ApplicationEvent();
                e.ApplicationName = "MOE.Common";
                //e.Class = GetType().ToString();
                e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                e.Description = ex.Message;
                e.Timestamp = DateTime.Now;
                logRepository.Add(e);
                return null;
            }
        }

        public static Controller_Event_Log GetFirstEventBeforeDateByEventCodeAndParameter(this IGenericTableController<Controller_Event_Log> controller, string signalId, int eventCode,
            int eventParam, DateTime date)
        {
            if (!String.IsNullOrEmpty(signalId))
            {
                try
                {
                    //_db.Database.CommandTimeout = 10;
                    var tempDate = date.AddDays(-1);
                    var lastEvent = controller.Query.Where(c => c.SignalID == signalId &&
                                                                        c.Timestamp >= tempDate &&
                                                                        c.Timestamp < date &&
                                                                        c.EventCode == eventCode &&
                                                                        c.EventParam == eventParam)
                        .OrderByDescending(c => c.Timestamp).FirstOrDefault();
                    return lastEvent;
                }

                catch (Exception ex)
                {
                    var logRepository = ApplicationEventRepositoryFactory.Create();
                    var e = new ApplicationEvent();
                    e.ApplicationName = "MOE.Common";
                    //e.Class = GetType().ToString();
                    e.Function = "GetEventsByEventCodesParamWithOffsetAndLatencyCorrection";
                    e.SeverityLevel = ApplicationEvent.SeverityLevels.High;
                    e.Description = ex.Message;
                    e.Timestamp = DateTime.Now;
                    logRepository.Add(e);
                    return null;
                }
            }

            return null;
        }

        public static int GetSignalEventsCountBetweenDates(this IGenericTableController<Controller_Event_Log> controller, string signalId, DateTime startTime, DateTime endTime)
        {
            return controller.Query.Count(r => r.SignalID == signalId &&
                                                r.Timestamp >= startTime
                                                && r.Timestamp < endTime);
        }

        public static List<Controller_Event_Log> GetEventsBetweenDates(this IGenericTableController<Controller_Event_Log> controller, DateTime startTime, DateTime endTime)
        {
            return controller.Query.Where(r => r.Timestamp >= startTime
                                                       && r.Timestamp < endTime).ToList();
        }

        public static int GetApproachEventsCountBetweenDates(this IGenericTableController<Controller_Event_Log> controller, int approachId, DateTime startTime, DateTime endTime,
            int phaseNumber)
        {
            var approachCodes = new List<int> { 1, 8, 10 };
            var ar = ApproachRepositoryFactory.Create();
            Approach approach = ar.GetApproachByApproachID(approachId);

            var results = controller.Query.Where(r =>
                r.SignalID == approach.SignalID && r.Timestamp > startTime && r.Timestamp < endTime
                && approachCodes.Contains(r.EventCode) && r.EventParam == phaseNumber);

            return results.Count();
        }

        public static DateTime GetMostRecentRecordTimestamp(this IGenericTableController<Controller_Event_Log> controller, string signalID)
        {
            MOE.Common.Models.Controller_Event_Log row = (from r in controller.Query
                where r.SignalID == signalID
                orderby r.Timestamp descending
                select r).Take(1).FirstOrDefault();
            if (row != null)
            {
                return row.Timestamp;
            }
            else
            {
                return new DateTime();
            }
        }
    }
}