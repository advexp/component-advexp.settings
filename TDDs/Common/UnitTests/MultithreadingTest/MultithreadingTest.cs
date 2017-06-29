using System;
using System.Threading;
using NUnit.Framework;
using Advexp;

namespace TDD
{
    public class MyThread
    {
        public MyThread(Object lockObject, ManualResetEvent doneEvent)
        {
            m_doneEvent = doneEvent;
            //m_lockObject = lockObject;
        }

        // Wrapper method for use with thread pool.
        public void ThreadPoolCallback(Object threadContext)
        {
            //int index = (int)threadContext;

            // randomly delay thread start
            //MyRandom rand = new MyRandom();
            //Thread.Sleep(rand.Next(10000));

            DifferentTypesLocalSettings settings = new DifferentTypesLocalSettings();
            DifferentTypesLocalSettings refSettings = new DifferentTypesLocalSettings();

            // copy settings to refSettings
            settings.SaveObjectSettings();
            refSettings.LoadObjectSettings();

            DifferentTypesTest.CompareSettings(settings, refSettings);

            m_doneEvent.Set();
        }

        //Object m_lockObject;
        ManualResetEvent m_doneEvent;

    }

    //[TestFixture]
    public class MultithreadingTest
    {
        //DifferentTypesLocalSettings m_singlesettings;

        //------------------------------------------------------------------------------
        //[TestFixtureSetUp]
        public void Setup()
        {
        }

        //------------------------------------------------------------------------------
        public void MultiSettingsThread(Object threadContext)
        {

        }

        //------------------------------------------------------------------------------
        public void SingleSettingsThread(Object threadContext)
        {
            
        }

        //------------------------------------------------------------------------------
        //[Test]
        public void MultiSettingsTest()
        {
            const int ThreadsCount = 64;
            Object lockObject = new Object();

            //ThreadPool.SetMaxThreads(1, 1);

            // One event is used for each Fibonacci object
            ManualResetEvent[] doneEvents = new ManualResetEvent[ThreadsCount];
            MyThread[] threadsArray = new MyThread[ThreadsCount];

            for (int i = 0; i < ThreadsCount; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                MyThread myThread = new MyThread(lockObject, doneEvents[i]);
                threadsArray[i] = myThread;
                ThreadPool.QueueUserWorkItem(myThread.ThreadPoolCallback, i);
            }

            // Wait for all threads in pool to calculation...
            WaitHandle.WaitAll(doneEvents);
        }
    }

}

