using System;
using System.Threading;

namespace HSCF.Threading
{
    /// <summary>
    /// This class is a timer that performs some tasks periodically.
    /// </summary>
    public class Timer : IRunnable
    {
        #region Public events

        /// <summary>
        /// Handler for timer elapsed
        /// </summary>
        public event EventHandler Elapsed;

        #endregion

        #region Public fields

        /// <summary>
        /// Task period of timer (as milliseconds).
        /// </summary>
        public int Period { get; set; }

        #endregion

        #region Private fields

        /// <summary>
        /// This timer is used to perfom the task at spesified intervals.
        /// </summary>
        private readonly System.Threading.Timer _taskTimer;

        /// <summary>
        /// Indicates that whether timer is running or stopped.
        /// </summary>
        private volatile bool _running;

        /// <summary>
        /// Indicates that whether performing the task or _taskTimer is in sleep mode.
        /// This field is used to wait executing tasks when stopping Timer.
        /// </summary>
        private volatile bool _performingTasks;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Timer.
        /// </summary>
        /// <param name="period">Task period of timer (as milliseconds)</param>
        public Timer(int period)
        {
            Period = period;
            _taskTimer = new System.Threading.Timer(TimerCallBack, null, Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            _taskTimer.Change(Period, Period);
            _running = true;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            lock (_taskTimer)
            {
                _running = false;
                _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Waits the service to stop.
        /// </summary>
        public void WaitToStop()
        {
            lock (_taskTimer)
            {
                while (_performingTasks)
                {
                    Monitor.Wait(_taskTimer);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method is called by _taskTimer.
        /// </summary>
        /// <param name="state">Not used argument</param>
        private void TimerCallBack(object state)
        {
            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);

            lock (_taskTimer)
            {
                if (!_running)
                {
                    return;
                }

                _performingTasks = true;
            }

            try
            {
                if(Elapsed != null)
                {
                    Elapsed(this, new EventArgs());
                }
            }
            catch
            {

            }
            finally
            {
                lock (_taskTimer)
                {
                    _performingTasks = false;
                    if (_running)
                    {
                        _taskTimer.Change(Period, Period);
                    }

                    Monitor.Pulse(_taskTimer);
                }
            }
        }

        #endregion
    }
}
