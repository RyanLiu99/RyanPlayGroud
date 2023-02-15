using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallTests
{
    internal class BetterCancellationTokenTests
    {
        // assignedToTask, theOneCallsCancel, checkAndThrowIfCancellationRequestedOn, waitTask, expectedTaskStatus , taskWillStop

        [Test]
        public void TestCT1([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(cs1.Token, cs1, csBoth.Token, waitTask, TaskStatus.Canceled, true);
         
        }

        [Test]
        public void TestCT2([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(cs1.Token, cs2, csBoth.Token, waitTask, TaskStatus.Faulted, true);
        }

        [Test]
        public void TestCT3([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(csBoth.Token, cs1, csBoth.Token, waitTask, TaskStatus.Canceled, true);
        }
        [Test]
        public void TestCT4([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(csBoth.Token, cs2, csBoth.Token, waitTask, TaskStatus.Canceled, true);
        }
        
        [Test]
        public void TestCT5([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(csBoth.Token, cs1, cs1.Token, waitTask, TaskStatus.Canceled, true);
        }

        [Test]
        public void TestCT6([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(csBoth.Token, cs1, cs2.Token, waitTask, TaskStatus.Canceled, true);  //cs1 cancel, and check if cancelled on cs2, how can that work?
        }

        //=====================
        [Test]
        public async Task TestCTAsync1([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(csBoth.Token, cs1, cs2.Token, waitTask, TaskStatus.Canceled,true);
        }

        [Test]
        public async Task TestCTAsync2([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(cs1.Token, cs2, csBoth.Token, waitTask, TaskStatus.Faulted, true);
        }

        [Test]
        public async Task TestCTAsync3([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(csBoth.Token, cs1, csBoth.Token, waitTask, TaskStatus.Canceled, true);
        }

        [Test]
        public async Task TestCTAsync4([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(csBoth.Token, cs2, csBoth.Token, waitTask, TaskStatus.Canceled, true);
        }

        [Test]
        public async Task TestCTAsync5([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(csBoth.Token, cs1, cs1.Token, waitTask, TaskStatus.Canceled, true);
        }

        public void Test(
            CancellationToken assignedToTask,
            CancellationTokenSource theOneCallsCancel,
            CancellationToken checkAndThrowIfCancellationRequestedOn,
            bool waitTask,
            TaskStatus expectedTaskStatus ,
            bool taskWillStop
        )
        {
            bool expectIsFaulted = expectedTaskStatus == TaskStatus.Faulted;
            Exception e = null;
            Task task = null;

            try
            {
                task = Task.Factory.StartNew((state) =>
                {
                    int i = 0;
                    while (i++ < 100)
                    {
                        Thread.Sleep(5);
                        ((CancellationToken)state).ThrowIfCancellationRequested();
                    }

                }, checkAndThrowIfCancellationRequestedOn, assignedToTask);
                // token task is on can be csBoth or cs1, no difference

                theOneCallsCancel.Cancel(false);

                if (waitTask)
                {
                    Assert.IsTrue(task.Wait(600), "Timed out"); //However, if the calling thread is not waiting on the task, this specific exception will not be propagated. 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log :Test 2 Exception occurred : {e}"); //since waited , here it got TaskCanceledException which is subclass of OperationCanceledException
                e = ex;
            }

            Assert.AreEqual(taskWillStop, task.ContinueWith((t) =>
            {
                Assert.AreEqual(expectIsFaulted, t.IsFaulted);
                Assert.AreEqual(expectedTaskStatus, t.Status);
                if (expectIsFaulted)
                {
                    Assert.IsNotNull(t.Exception);
                    Assert.IsTrue(t.Exception is AggregateException);
                    Assert.IsTrue(t.Exception.GetBaseException() is OperationCanceledException);
                    Assert.IsFalse(t.Exception.GetBaseException() is TaskCanceledException);
                }
                else
                {
                    Assert.IsNull(t.Exception);
                }
            }).Wait(600), "timeout on continue");


            //Thread.Sleep(20); //More then sleep in while

            Assert.AreEqual(waitTask, e != null); // if waited,there is exception. If not waited, there is no exception. (even task in on different token, this is same as task on right token)
            if (waitTask)
            {
                Assert.IsTrue(e is AggregateException); //strange, 
                Assert.IsTrue(e.GetBaseException() is OperationCanceledException);
            }

            //No matter waited or not waited, task is in same state:
            Assert.AreEqual(expectIsFaulted, task.IsFaulted);
            Assert.AreEqual(expectedTaskStatus, task.Status);
            if (expectIsFaulted)
            {
                Assert.IsNotNull(task.Exception);
                Assert.IsTrue(task.Exception is AggregateException);
                Assert.IsTrue(task.Exception.GetBaseException() is OperationCanceledException);
                Assert.IsFalse(task.Exception.GetBaseException() is TaskCanceledException);
            }
            else
            {
                Assert.IsNull(task.Exception);
            }
        }
        
        public async Task TestAsync(
            CancellationToken assignedToTask,
            CancellationTokenSource theOneCallsCancel,
            CancellationToken checkAndThrowIfCancellationRequestedOn,
            bool waitTask,
            TaskStatus expectedTaskStatus,
            bool taskWillStop
            )
        {
            bool expectIsFaulted = expectedTaskStatus == TaskStatus.Faulted;

            Exception e = null;
            Task task = null;

            try
            {
                task = Task.Factory.StartNew((state) =>
                {
                    int i = 0;
                    while (i++ < 100)
                    {
                        Thread.Sleep(5);
                        ((CancellationToken)state).ThrowIfCancellationRequested();
                    }

                }, checkAndThrowIfCancellationRequestedOn, assignedToTask);
                // token task is on can be csBoth or cs1, no difference

                theOneCallsCancel.Cancel(false);

                if (waitTask)
                {
                    Assert.IsTrue(task.Wait(600), "Timed out"); //However, if the calling thread is not waiting on the task, this specific exception will not be propagated. 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log :Test 2 Exception occurred : {e}"); //since waited , here it got TaskCanceledException which is subclass of OperationCanceledException
                e = ex;
            }

            await task.ContinueWith((t) =>
            {
                Assert.AreEqual(expectIsFaulted, t.IsFaulted);
                Assert.AreEqual(expectedTaskStatus, t.Status);
                if (expectIsFaulted)
                {
                    Assert.IsNotNull(t.Exception);
                    Assert.IsTrue(t.Exception is AggregateException);
                    Assert.IsTrue(t.Exception.GetBaseException() is OperationCanceledException);
                    Assert.IsFalse(t.Exception.GetBaseException() is TaskCanceledException);
                }
                else
                {
                    Assert.IsNull(t.Exception);
                }
            });


            //Thread.Sleep(20); //More then sleep in while

            Assert.AreEqual(waitTask, e != null); // if waited,there is exception. If not waited, there is no exception. (even task in on different token, this is same as task on right token)
            if (waitTask)
            {
                Assert.IsTrue(e is AggregateException); //strange
                Assert.IsTrue(e.GetBaseException() is OperationCanceledException);
            }

            //No matter waited or not waited, task is in same state:
            Assert.AreEqual(expectIsFaulted, task.IsFaulted);
            Assert.AreEqual(expectedTaskStatus, task.Status);
            if (expectIsFaulted)
            {
                Assert.IsNotNull(task.Exception);
                Assert.IsTrue(task.Exception is AggregateException);
                Assert.IsTrue(task.Exception.GetBaseException() is OperationCanceledException);
                Assert.IsFalse(task.Exception.GetBaseException() is TaskCanceledException);
            }
            else
            {
                Assert.IsNull(task.Exception);
            }
        }
    }
}
