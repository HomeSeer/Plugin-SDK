using System;
using System.Collections;
using System.Threading;

namespace HSCF.Threading
{
    /// <summary>
    /// A threaded queue that process only one item in a time and keeps others in a queue.
    /// </summary>
    /// <typeparam name="T">Type of the processing item</typeparam>
    public class QueueProcessorThread<T> 
    {
        /// <summary>
        /// This event is used to process get and process an item from queue. When an item inserted this
        /// queue, ProcessItem event is raised.
        /// </summary>
        public event ProcessQueueItemHandler<T> ProcessItem;

        /// <summary>
        /// Maximum item count can queue store.
        /// If queue if full and an item is added, an exception is thrown.
        /// Setting this field to 0 (or a negative number) allows infinite item.
        /// </summary>
        public int MaxItemCount { get; set; }

        /// <summary>
        /// Gets the item count currently waiting on queue.
        /// </summary>
        public int ItemCount
        {
            get
            {
                lock (_queue.SyncRoot)
                {
                    return _queue.Count;
                }
            }
        }

        /// <summary>
        /// Queue object to store items.
        /// </summary>
        private readonly Queue _queue;

        /// <summary>
        /// Running thread.
        /// </summary>
        private Thread _thread;
        
        /// <summary>
        /// Thread control flag.
        /// </summary>
        private volatile bool _running;

        /// <summary>
        /// Construnctor.
        /// </summary>
        public QueueProcessorThread()
        {
            _queue = Queue.Synchronized(new Queue());
        }

        /// <summary>
        /// Construnctor.
        /// </summary>
        /// <param name="maxItemCount">Maximum item count can queue store</param>
        public QueueProcessorThread(int maxItemCount) : this()
        {
            MaxItemCount = maxItemCount;
        }

        /// <summary>
        /// Starts the processing of items. Thread runs, listens and process items on queue.
        /// </summary>
        public void Start()
        {
            lock (_queue.SyncRoot)
            {
                if (_running)
                {
                    return;
                }

                _running = true;
                _thread = new Thread(DoProcess);
                _thread.Start();
            }
        }

        /// <summary>
        /// Stops the processing of items and stops the thread.
        /// </summary>
        public void Stop()
        {
            lock (_queue.SyncRoot)
            {
                if (!_running)
                {
                    return;
                }

                _running = false;
                Monitor.PulseAll(_queue.SyncRoot);
            }
        }

        /// <summary>
        /// Waits stopping of thread, thus waits end of execution of currently processing item.
        /// </summary>
        public void WaitToStop()
        {
            try
            {
                _thread.Join();
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// Adds given item to queue to process.
        /// </summary>
        /// <param name="queueItem"></param>
        public void Add(T queueItem)
        {
            lock (_queue.SyncRoot)
            {
                if (MaxItemCount > 0 && _queue.Count >= MaxItemCount)
                {
                    throw new Exception("Can not add to Queue since it is full. There are " + _queue.Count + " waiting items on queue.");
                }

                _queue.Enqueue(queueItem);
                Monitor.PulseAll(_queue.SyncRoot);
            }
        }

        /// <summary>
        /// Thread's running method. Listens queue and processes items.
        /// </summary>
        private void DoProcess()
        {
            while (_running)
            {
                var queueItem = default(T);
                var remainingItemCount = 0;
                lock (_queue.SyncRoot)
                {
                    if (_queue.Count > 0)
                    {
                        queueItem = (T)_queue.Dequeue();
                        remainingItemCount = _queue.Count;
                    }
                    else
                    {
                        Monitor.Wait(_queue.SyncRoot,1000);     // rjh added timeout so we can exit if _running goes to false (for exiting app gracefully)
                    }
                }

                if (!Equals(queueItem, default(T)))
                {
                    OnProcessItem(queueItem, remainingItemCount);
                }
            }
        }

        /// <summary>
        /// This method is used to raise ProcessItem event.
        /// </summary>
        /// <param name="queueItem">The item that must be processed</param>
        /// <param name="remainingItemCount">Waiting item count on queue except this one</param>
        protected virtual void OnProcessItem(T queueItem, int remainingItemCount)
        {
            if (ProcessItem == null)
            {
                return;
            }

            try
            {
                ProcessItem(this, new ProcessQueueItemEventArgs<T>(queueItem, remainingItemCount));
            }
            catch
            {

            }
        }
    }
}
