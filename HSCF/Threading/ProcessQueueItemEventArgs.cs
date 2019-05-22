using System;

namespace HSCF.Threading
{
    /// <summary>
    /// Stores processing item and some informations about queue.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProcessQueueItemEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The item to process.
        /// </summary>
        public T ProcessItem { get; set; }

        /// <summary>
        /// The item count waiting for processing on queue (after this one).
        /// </summary>
        public int QueuedItemCount { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="processItem">The item to process</param>
        /// <param name="queuedItemCount">The item count waiting for processing on queue (after this one)</param>
        public ProcessQueueItemEventArgs(T processItem, int queuedItemCount)
        {
            ProcessItem = processItem;
            QueuedItemCount = queuedItemCount;
        }
    }
}