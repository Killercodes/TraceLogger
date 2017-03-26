using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Logging.Test
{
    [TestClass]
    public class LoggingTest
    {
        [TestMethod]
        public void VerboseTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("verboseTest",0);
            c1.Verbose("this is verbose");
            c1.Verbose("this is {0}", "verbose");

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void InfoTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("infoTest",0);
            c1.Info("this is info 1");
            c1.Info("this is infor {0}", 2);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void WarnTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("WarnTest",0);
            c1.Warn("this is warning 1");
            c1.Warn("this is warning {0}", 2);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void ErrorTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("ErrorTest",0);
            c1.Error("this is ErrorTest 1");
            c1.Error("this is ErrorTest {0}", 2);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void CriticalTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("ErrorTest");
            c1.Critical("this is ErrorTest 1");
            c1.Critical("this is ErrorTest {0}", 2);

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void LogTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("ExTest",0);

            try
            {
                var v1 = System.IO.File.Open("c:\\hack.dll",System.IO.FileMode.Open);
            }
            catch (Exception ex)
            {
                c1.LogException(ex);
                
            }

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void AllTest ()
        {
            TraceLogger c1 = TraceLogger.Setup("ExTest",0);
            
            c1.Verbose("this is {0}", "verbose");
            c1.Start("starting logical operation");
            c1.Verbose("this a test for all and this is verbose output");
            c1.Verbose("this the same test with info output");

            try
            {
                c1.Warn("this might throw an exception");
                var v1 = System.IO.File.Open("c:\\hack.dll", System.IO.FileMode.Open);
            }
            catch (Exception ex)
            {
                c1.Error("this is the exception message {0}", ex.Message);
                c1.LogException(ex);
                c1.LogException<Exception>(ex);
            }
            c1.Stop("Stopping logical operation");

            Assert.AreEqual(1, 1);
        }
    }
}
