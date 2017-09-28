using System;
using System.Diagnostics;
using System.Globalization;



namespace iTrace
{
    public class EventLogListener : TraceListener
    {
        private EventLog eventLog;
        private readonly EventLogEntryType logType;
        //private Int32 eventId;
        private string sourceName;
        private short categoryId;


        public EventLogListener(string parameter)
        {
            sourceName = parameter;
            eventLog = new EventLog("name", ".", sourceName);
            //eventId = 5201; 
            categoryId = 5201;
            logType = EventLogEntryType.Information;
        }
		
		public EventLogListener(string parameter)
        {
            sourceName = parameter;
            eventLog = new EventLog("name", ".", sourceName);
            //eventId = 5201; 
            categoryId = 5201;
            logType = EventLogEntryType.Information;
        }
		

        private string Message(Object message)
        {
            //long epoch = ((long)(System.DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            return string.Format("{0}", message);
        }

        #region OVERRIDE

        public override void Write(string message)
        {
            try { eventLog.WriteEntry(Message(message), logType, WebConfig.EventId, categoryId); }
            catch{/*Supress Error */ }
        }

        public override void WriteLine(string message)
        {
            try { eventLog.WriteEntry(Message(message), EventLogEntryType.Information, WebConfig.EventId, categoryId); }
            catch {/*Supress Error */ }
        }

        public override void Write(string message, string category)
        {
            try
            {
                var thisLogType = (EventLogEntryType) Enum.Parse(typeof (EventLogEntryType), category);
                eventLog.WriteEntry(Message(message), thisLogType, WebConfig.EventId, categoryId);
            }
            catch{/*Supress Error */ }
        }

        public override void WriteLine(string message, string category)
        {
            try
            {
                var thisLogType = (EventLogEntryType)Enum.Parse(typeof(EventLogEntryType), category);
                eventLog.WriteEntry(Message(message), thisLogType, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void Write(object o)
        {
            try { eventLog.WriteEntry(Message(o), logType, WebConfig.EventId, categoryId); }
            catch{/*Supress Error */ }
        }

        public override void WriteLine(object o)
        {
            try { eventLog.WriteEntry(Message(o), logType, WebConfig.EventId, categoryId); }
            catch {/*Supress Error */ }
        }

        public override void Write(object o, string category)
        {
            try
            {
                var thisLogType = (EventLogEntryType) Enum.Parse(typeof (EventLogEntryType), category);
                eventLog.WriteEntry(Message(o), thisLogType, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void WriteLine(object o, string category)
        {
            try
            {
                var thisLogType = (EventLogEntryType) Enum.Parse(typeof (EventLogEntryType), category);
                eventLog.WriteEntry(Message(o), thisLogType, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void Fail(string message)
        {
            try { eventLog.WriteEntry(Message(message), EventLogEntryType.Error, WebConfig.EventId, categoryId); }
            catch {/*Supress Error */ }
        }

        public override void Fail(string message, string detailMessage)
        {
            try { eventLog.WriteEntry(Message(message) + Environment.NewLine + detailMessage, EventLogEntryType.Error, WebConfig.EventId, categoryId); }
            catch {/*Supress Error */ }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("Source: {0}", source));
                sb.AppendLine(string.Format("DateTime: {0}", eventCache.DateTime.ToString(CultureInfo.InvariantCulture)));
                //sb.AppendLine(Message(eventCache.ProcessId));
                sb.AppendLine(string.Format("TimeStamp: {0}", eventCache.Timestamp));
                sb.AppendLine(string.Format("Message: {0}", Message(data)));
                sb.AppendLine(string.Format(" Callstack: {0}", eventCache.Callstack));
                if (eventType == TraceEventType.Error)
                    eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Information)
                    eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Warning)
                    eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Warning, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Verbose)
                    eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Critical)
                    eventLog.WriteEntry(sb.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            try
            {
                var temp = new StringBuilder();
                temp.AppendLine(string.Format(" Source: {0}", source));
                temp.AppendLine(string.Format(" EventType: {0}", eventType.ToString()));
                temp.AppendLine(string.Format(" Id: {0}", id));
                temp.AppendLine(string.Format(" TimeStamp: {0}", eventCache.Timestamp));
                temp.AppendLine(string.Format(" DateTime: {0}", eventCache.DateTime));
                temp.AppendLine(string.Format(" Callstack: {0}", eventCache.Callstack));

                if (eventType == TraceEventType.Error)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Information)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Warning)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Warning, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Verbose)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Critical)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        // ReSharper disable MethodOverloadWithOptionalParameter
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        // ReSharper restore MethodOverloadWithOptionalParameter
        {
            try
            {
                var exceptionString = new StringBuilder();

                if (args[0] is Exception)
                {
                    Exception ex = (Exception)args[0];
                    exceptionString.AppendLine(string.Format("Message: {0}", ex.Message));
                    exceptionString.AppendLine(string.Format("Source: {0}", ex.Source));
                    exceptionString.AppendLine(string.Format("StackTrace: {0}", ex.StackTrace));
                    if (ex.InnerException != null)
                    {
                        exceptionString.AppendLine(string.Format("InnerExceptionMessage: {0}", ex.InnerException.Message));
                        exceptionString.AppendLine(string.Format("InnerExceptionSource: {0}", ex.InnerException.Source));
                        exceptionString.AppendLine(string.Format("InnerExceptionStackTrace: {0}", ex.InnerException.StackTrace));
                    }
                }
                else
                {
                    string msg = string.Format(format, args);
                    exceptionString.AppendLine(string.Format(" Message: {0}", msg));
                    exceptionString.AppendLine(string.Format(" Source: {0}", source));
                    exceptionString.AppendLine(string.Format(" EventType: {0}", eventType.ToString()));
                    exceptionString.AppendLine(string.Format(" Id: {0}", id));
                    exceptionString.AppendLine(string.Format(" TimeStamp: {0}", eventCache.Timestamp));
                    exceptionString.AppendLine(string.Format(" DateTime: {0}", eventCache.DateTime));
                    exceptionString.AppendLine(string.Format(" Callstack: {0}", eventCache.Callstack));
                }

                if (eventType == TraceEventType.Error)
                    eventLog.WriteEntry(exceptionString.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Information)
                    eventLog.WriteEntry(exceptionString.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Warning)
                    eventLog.WriteEntry(exceptionString.ToString(), EventLogEntryType.Warning, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Verbose)
                    eventLog.WriteEntry(exceptionString.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Critical)
                    eventLog.WriteEntry(exceptionString.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            try
            {
                var temp = new StringBuilder();
                temp.AppendLine(string.Format(" Message: {0}", message));
                temp.AppendLine(string.Format(" Source: {0}", source));
                temp.AppendLine(string.Format(" EventType: {0}", eventType.ToString()));
                temp.AppendLine(string.Format(" Id: {0}", id));
                temp.AppendLine(string.Format(" TimeStamp: {0}", eventCache.Timestamp));
                temp.AppendLine(string.Format(" DateTime: {0}", eventCache.DateTime));
                temp.AppendLine(string.Format(" Callstack: {0}", eventCache.Callstack));

                if (eventType == TraceEventType.Error)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Information)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Warning)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Warning, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Verbose)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);

                if (eventType == TraceEventType.Critical)
                    eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Error, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            try
            {
                var temp = new StringBuilder();
                temp.AppendLine(string.Format(" Message: {0}", message));
                temp.AppendLine(string.Format(" Source: {0}", source));
                temp.AppendLine(string.Format(" Id: {0}", id));
                temp.AppendLine(string.Format(" TimeStamp: {0}", eventCache.Timestamp));
                temp.AppendLine(string.Format(" DateTime: {0}", eventCache.DateTime));
                temp.AppendLine(string.Format(" Callstack: {0}", eventCache.Callstack));

                eventLog.WriteEntry(temp.ToString(), EventLogEntryType.Information, WebConfig.EventId, categoryId);
            }
            catch {/*Supress Error */ }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            try
            {
                var eventInstance = new EventInstance(id, 400, EventLogEntryType.Error);
                object[] errorDataArray = data;
                EventLog.WriteEvent(sourceName, eventInstance, errorDataArray);
            }
            catch {/*Supress Error */ }
        }


        #endregion
    }
}
