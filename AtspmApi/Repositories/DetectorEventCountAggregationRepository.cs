﻿using System;
using System.Collections.Generic;
using System.Linq;
using AtspmApi.Models;

namespace AtspmApi.Repositories
{
    public class DetectorEventCountAggregationRepository : IDetectorEventCountAggregationRepository
    {
        private readonly Models.AtspmApi _db;

        public DetectorEventCountAggregationRepository()
        {
            _db = new Models.AtspmApi();
        }

        public DetectorEventCountAggregationRepository(Models.AtspmApi context)
        {
            _db = context;
        }

        public int GetDetectorEventCountSumAggregationByDetectorIdAndDateRange(int detectorId, DateTime start,
            DateTime end)
        {
            //var count = 0;
            //    if (_db.DetectorEventCountAggregations.Any(r => r.Id == detectorId 
            //                                                 && r.BinStartTime >= start && r.BinStartTime <= end))
            //    {
            //        count = _db.DetectorEventCountAggregations.Where(r => r.Id == detectorId 
            //                                                            && r.BinStartTime >= start && r.BinStartTime <= end)
            //            .Sum(r => r.EventCount);
            //    }
            var count = 0;
            //if (_db.DetectorEventCountAggregations.Any(r => r.DetectorId.Contains(detectorId.ToString())
            if (_db.DetectorEventCountAggregations.Any(r => r.DetectorPrimaryId == detectorId
                                                            && r.BinStartTime >= start && r.BinStartTime < end))
            {
                count = _db.DetectorEventCountAggregations.Where(r => r.DetectorPrimaryId  == detectorId
                                                                      && r.BinStartTime >= start && r.BinStartTime < end)
                    .Sum(r => r.EventCount);
            }

            return count;
        }
        

        public List<DetectorEventCountAggregation> GetDetectorEventCountAggregationByDetectorIdAndDateRange(int detectorId, DateTime start,
            DateTime end)
        {
            // Note: this one looked like the one above.  Remove the ID field from the tables.  
            // This field was an identity Specification starting at 1 and growing by one with each record.
            // Does it make sense to compare the id field to the detectorID?  I don't see why this was compared, other than they were both ints!

            if (_db.DetectorEventCountAggregations.Any(r => r.DetectorPrimaryId == detectorId
                                                             && r.BinStartTime >= start && r.BinStartTime < end))
                {
                    return _db.DetectorEventCountAggregations.Where(r => r.DetectorPrimaryId == detectorId
                                                                      && r.BinStartTime >= start &&
                                                                      r.BinStartTime < end).ToList();
                }
            return new List<DetectorEventCountAggregation>();
        }
    }

    
}