﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningFoundation
{
    /// <summary>
    /// Holds all processing relevant data.
    /// Instance of the context i spassed to every pipeline
    /// durring processing.
    /// </summary>
    public class Context : IContext
    {
        /// <summary>
        /// Descriptor instance, which was passed in the pipeline
        /// before any processing.
        /// </summary>
        public IDataDescriptor DataDescriptor { get; set; }

        /// <summary>
        /// Gets the score of training operation.
        /// </summary>
        public IScore Score { get; set; }

        /// <summary>
        /// Indicates if the operation is running in the batch.
        /// </summary>
        public bool IsMoreDataAvailable { get; set; }


        /// <summary>
        /// Indication of the current mini-batch iteration. It is incremented over each batch iteration,
        /// since we need how much data should be skipped in the current batch iteration. 
        /// </summary> 
        public int BatchIteration { get; set; }
    }
}
