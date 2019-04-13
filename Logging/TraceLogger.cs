/*
 *  TraceLogger
 *  Application Logger based on TraceSource
 * 
 */

using System;
using System.Diagnostics;

namespace App.Diagnostics
{
    /// <summary>
    /// Custom logger based on TraceSource
    /// </summary>
    public sealed class TraceLogger
    { 
        TraceSource traceSource;
        public int EventId { get; private set; }
        public string Name { get { return traceSource.Name; } }
        private static readonly Random getRandom = new Random();
        private static readonly object syncLock = new object();

        #region PRIVATE
        /// <summary>
        /// Initialize TraceLogger
        /// </summary>
        /// <param name="sourcename">Source name</param>
        private TraceLogger (string sourcename)
        {
            traceSource = new TraceSource(sourcename,SourceLevels.All); 
            EventId = GenerateEventId(); 
            
        }

        private TraceLogger (string sourcename, ushort eventId)
        {
            traceSource = new TraceSource(sourcename, SourceLevels.All);
            EventId = eventId;
        }

        /// <summary>
        /// Setups the trace logger
        /// </summary>
        /// <param name="sourcename">the source name </param>
        /// <param name="eventId">0 will create a new event id randomly, other is just parameter</param>
        /// <returns></returns>
        public static TraceLogger Setup (string sourcename = "*", ushort eventId = 0)
        {
            
            if(eventId != 0)
                return new TraceLogger(sourcename, eventId);

            return new TraceLogger(sourcename);
        }

        //Generate random _event_id from 1 - 65536
        private static int GenerateEventId ()
        {
            lock (syncLock)
            { // synchronize
                return getRandom.Next(1, 65536);
            }
        }

        // log event
        private void LogEvent (TraceEventType type, string message)
        {
            traceSource.TraceEvent(type, EventId, message);
        }
        #endregion

        public void If(bool condition, TraceEventType traceEventType, object arg)
        {
            if (condition)
            {
                TraceData(traceEventType, arg);
            }
        }
        public void If(bool condition, TraceEventType traceEventType, string message)
        {
           if(condition)
            {
                TraceEvent(traceEventType, message);
            }
        }

        public void TraceEvent(TraceEventType traceEventType, string message)
        {
            traceSource.TraceEvent(traceEventType, EventId, message);
        }

        public void TraceData(TraceEventType traceEventType, object arg)
        {
            traceSource.TraceData(traceEventType, EventId, arg); 
        }

        /// <summary>
        /// LogException to Trace listeners
        /// </summary>
        /// <param name="exception"> exception object</param>
        public void LogException (Exception exception)
        {
            traceSource.TraceData(TraceEventType.Critical, EventId, exception);
        }


       
        /// <summary>
        /// Fatal Error or Application crash
        /// </summary>
        public void Critical (string format, params object[] args)
        {
            Critical(string.Format(format, args));
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
            Error(string.Format(format, args));
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
            Warn(string.Format(format, args));
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
            Info(string.Format(format, args));
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
            Verbose(string.Format(format, args));
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
