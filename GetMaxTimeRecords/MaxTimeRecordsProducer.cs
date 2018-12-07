﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetMaxTimeRecords
{
    class MaxTimeRecordsProducer
    {

        
        
        private static DateTime GetMostRecentRecordTime(string signalId)
        {

            var CELRepo = MOE.Common.Models.Repositories.ControllerEventLogRepositoryFactory.Create();
            DateTime mostRecentEventTime = CELRepo.GetMostRecentRecordTimestamp(signalId);

            if (mostRecentEventTime != null)
            {

                return (mostRecentEventTime);
            }
            return (DateTime.Now.AddDays(-2));
        }

        //private object[] GetRandomRow(DataConsumer consumer)
        //{
           // put stuff to create records here
            
           
        //}



    }
}
