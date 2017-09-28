/*
 *  TraceLogger
 *  Application Logger based on TraceSource
 *  
 */


namespace iTrace
{
    /// <summary>
    /// Custom logger based on TraceSource
    /// </summary>
    public sealed class TraceLogger
    { 
        TraceSource _trace_source;
        int _event_id = 1;
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();

        /// <summary>
        /// Initialize TraceLogger
        /// </summary>
        /// <param name="sourcename">Source name</param>
        private TraceLogger (string sourcename)
        {
            _trace_source = new TraceSource(sourcename,SourceLevels.All);
            _event_id = generateEventId(); 
        }

        private TraceLogger (string sourcename, ushort eventId)
        {
            _trace_source = new TraceSource(sourcename, SourceLevels.All);
            _event_id = eventId;
        }

        /// <summary>
        /// Setups the trace logger
        /// </summary>
        /// <param name="sourcename">the source name </param>
        /// <param name="eventId">0 will create a new event id randomly, other is just parameter</param>
        /// <returns></returns>
        public static TraceLogger Setup (string sourcename, ushort eventId = 0)
        {
            if(eventId != 0)
                return new TraceLogger(sourcename, eventId);

            return new TraceLogger(sourcename);
        }

        //Generate random _event_id from 1 - 65536
        private static int generateEventId ()
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(1, 65536);
            }
        }

        // log event
        private void LogEvent (TraceEventType type, string message)
        {
            _trace_source.TraceEvent(type, _event_id, message);
        }

        /// <summary>
        /// LogException to Trace listeners
        /// </summary>
        /// <param name="ex"> exception object</param>
        public void LogException (Exception ex)
        {
            _trace_source.TraceData(TraceEventType.Critical, _event_id, ex);
        }
        /// <summary>
        /// LogException to Trace listeners
        /// </summary>
        public void LogException<T> (T ex)
        {
            _trace_source.TraceData(TraceEventType.Critical, _event_id, ex);
        }

        /// <summary>
        /// Fatal Error or Application crash
        /// </summary>
        public void Critical (string format, params object[] args)
        {
            LogEvent(TraceEventType.Critical, string.Format(format, args));
        }

        /// <summary>
        /// Fatal Error or Application crash
        /// </summary>
        public void Critical (string message)
        {
            LogEvent(TraceEventType.Critical, message);
        }

        /// <summary>
        /// Recoverable error
        /// </summary>
        public void Error (string format, params object[] args)
        {
            LogEvent(TraceEventType.Error, string.Format(format, args));
        }

        /// <summary>
        /// Recoverable error
        /// </summary>
        public void Error (string message)
        {
            LogEvent(TraceEventType.Error, message);
        }

        /// <summary>
        /// Noncritical problem
        /// </summary>
        public void Warn (string format, params object[] args)
        {
            LogEvent(TraceEventType.Warning, string.Format(format, args));
        }
        /// <summary>
        /// Noncritical problem
        /// </summary>
        public void Warn (string message)
        {
            LogEvent(TraceEventType.Warning,message);
        }

        /// <summary>
        /// Informational Message
        /// </summary>
        public void Info (string format, params object[] args)
        {
            LogEvent(TraceEventType.Information, string.Format(format, args));
        }
        /// <summary>
        /// Informational Message
        /// </summary>
        public void Info (string message)
        {
            LogEvent(TraceEventType.Information,message);
        }

        /// <summary>
        /// Debugging Trace
        /// </summary>
        public void Verbose (string format, params object[] args)
        {
            LogEvent(TraceEventType.Verbose, string.Format(format, args));
        }

        /// <summary>
        /// Debugging Trace
        /// </summary>
        public void Verbose (string message)
        {
            LogEvent(TraceEventType.Verbose, message);
        }

        /// <summary>
        /// Starting a logical operation
        /// </summary>
        public void Start (string message)
        {
            LogEvent(TraceEventType.Start, message);
        }

        /// <summary>
        /// Stopping a logical operation
        /// </summary>
        public void Stop (string message)
        {
            LogEvent(TraceEventType.Stop, message);
        }

        /// <summary>
        /// Resumption a logical operation
        /// </summary>
        public void Resume (string message)
        {
            LogEvent(TraceEventType.Resume, message);
        }

        /// <summary>
        /// Suspension a logical operation
        /// </summary>
        public void Suspend (string message)
        {
            LogEvent(TraceEventType.Suspend, message);
        }

        /// <summary>
        /// Changing of corelation identity
        /// </summary>
        public void Transfer (string message)
        {
            LogEvent(TraceEventType.Transfer, message);
        }
    }
}
