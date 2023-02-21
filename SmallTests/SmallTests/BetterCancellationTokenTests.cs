using NUnit.Framework;
using SmallTests.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
        // Wait() it will cause cancel, not caused by CancellToken
        // So TestCTAsync6_2 is real test
        public void TestCT6([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            Test(csBoth.Token, cs1, cs2.Token, waitTask, TaskStatus.Canceled, true); //cs1 cancel, and check if cancelled on cs2, how can that work?
        }

        //=====================
        [Test]
        public async Task TestCTAsync1([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(csBoth.Token, cs1, cs2.Token, waitTask, TaskStatus.Canceled, true);
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

        [Test]
        // Wait() it will cause cancel, not caused by CancellToken
        // So TestCTAsync6_2 is real test
        public async Task TestCTAsync6([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsync(csBoth.Token, cs1, cs2.Token, waitTask, TaskStatus.Canceled, true);
        }

        [Test]
        [Description("ttt")]
        //nunit3-console.exe .\SmallTests.dll --where "method == TestCTAsync6_2"
        public async Task TestCTAsync6_2()
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource cs3 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsyncNoWait(csBoth.Token, cs3, cs2.Token, TaskStatus.Running);
            await TestAsyncNoWait(csBoth.Token, cs3, csBoth.Token, TaskStatus.Running);
        }

        [Test]
        public async Task TestCTAsync7()
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            await TestAsyncNoWait(cs1.Token, csBoth, csBoth.Token, TaskStatus.Faulted);
            //await TestAsyncNoWait(cs1.Token, csBoth, cs1.Token, TaskStatus.Running);
        }

        [Test]
        public async Task TestCTAsync8()
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);
            
            await TestAsyncNoWait(cs1.Token, csBoth, cs1.Token, TaskStatus.Running);
        }

        [Test]
        public async Task TestCTAsync9()
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();
            using CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);

            await TestAsyncNoWait(cs1.Token, csBoth, cs2.Token, TaskStatus.Running);
        }

        // assignedToTask, theOneCallsCancel, checkAndThrowIfCancellationRequestedOn, waitTask, expectedTaskStatus , taskWillStop

        public void Test(
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

                    Console.WriteLine("Out of loop, task wil finish");
                }, checkAndThrowIfCancellationRequestedOn, assignedToTask);
                // token task is on can be csBoth or cs1, no difference

                theOneCallsCancel.Cancel(false);

                if (waitTask)
                {
                    //This line iself can throw AggregateException [TaskCanceledException], not cancelled by CancellationToken
                    Assert.IsTrue(task.Wait(700), "Task Timed out"); //However, if the calling thread is not waiting on the task, this specific exception will not be propagated. 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log :  Exception occurred when run task : {ex}"); //since waited , here it got TaskCanceledException which is subclass of OperationCanceledException
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

            Assert.AreEqual(waitTask, e != null && taskWillStop); // if waited,there is exception. If not waited, there is no exception. (even task in on different token, this is same as task on right token)
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
            int count = 100;
            bool expectIsFaulted = expectedTaskStatus == TaskStatus.Faulted;

            Exception e = null;
            Task task = null;

            try
            {
                task = Task.Factory.StartNew((state) =>
                {
                    int i = 0;
                    while (i++ < count)
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
                else
                {
                    Assert.IsTrue(WaitHelper.SpinWait(() => task.Status >= TaskStatus.Running, 5, 10), "Task cannot start");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log : Exception occurred when run task in TestAsync: {ex}"); //since waited , here it got TaskCanceledException which is subclass of OperationCanceledException
                e = ex;
            }

            if (waitTask)
            {
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
            }

            Assert.AreEqual(waitTask && taskWillStop, e != null, e?.ToString()); // if waited,there is exception. If not waited, there is no exception. (even task in on different token, this is same as task on right token)

            if (waitTask)
            {
                Assert.IsTrue(e is AggregateException); //strange
                Assert.IsTrue(e.GetBaseException() is OperationCanceledException);
            }

            Thread.Sleep(50);
            Assert.AreEqual(expectedTaskStatus, task.Status, "Status is not expected.");
            Assert.AreEqual(expectIsFaulted, task.IsFaulted, $"Task status {task.Status} is not expected. Expected faulted: {expectIsFaulted}. WaitTask : {waitTask}");

            if (taskWillStop || waitTask)
            {
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
            else
            {
                Assert.AreEqual(taskWillStop, WaitHelper.SpinWait(() => task.IsCompleted, 5, count / 3), "Task will stop? " + taskWillStop);
            }
        }

        public async Task TestAsyncNoWait(
            CancellationToken assignedToTask,
            CancellationTokenSource theOneCallsCancel,
            CancellationToken checkAndThrowIfCancellationRequestedOn,
            TaskStatus expectedTaskStatus
            
        )
        {
            TestHelpers.Logger.Value.LogInformation("Logger: start testing TestAsyncNoWait ... "); //not working in VS, but works in nunit3-console
            Console.WriteLine("Console start testing .."); //works in both vs and nunit3-console
            int count = 300;
            bool expectIsFaulted = expectedTaskStatus == TaskStatus.Faulted;

            Task task = Task.Factory.StartNew((state) =>
            {
                int i = 0;
                var ct = checkAndThrowIfCancellationRequestedOn;// ((CancellationToken)state);
                while (i++ < count)
                {
                    Thread.Sleep(5);

                    if (ct.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation is indeed Requested !!!!");
                        throw new OperationCanceledException("OperationCanceled", ct);
                    }
                    else
                    {
                        Console.WriteLine("Cancellation is NOT requested ..");
                    }

                    //same as ((CancellationToken)state).ThrowIfCancellationRequested();
                }

            }, checkAndThrowIfCancellationRequestedOn, assignedToTask);
            // token task is on can be csBoth or cs1, no difference

            Console.WriteLine($"Task status is {task.Status} before call Cancel ");
            theOneCallsCancel.Cancel();
            Console.WriteLine($"Task status is {task.Status} after call Cancel ");

            Assert.IsTrue(WaitHelper.SpinWait(() => task.Status >= TaskStatus.Running, 5, 10), "Task cannot start");
            Console.WriteLine($"Task status is {task.Status} after wait status go to running or above, right before check... ");

            Thread.Sleep(30);
            Assert.AreEqual(expectedTaskStatus, task.Status, "Status is not expected.");
            Assert.AreEqual(expectIsFaulted, task.IsFaulted, $"Task status {task.Status} is not expected. Expected faulted: {expectIsFaulted}.");

            var taskWillStop = expectedTaskStatus != TaskStatus.Running;
            if (taskWillStop)
            {
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
            else
            {
                Assert.AreEqual(taskWillStop, WaitHelper.SpinWait(() => task.IsCompleted, 5, count / 3), "Task will stop? " + taskWillStop);
                Assert.AreEqual(expectedTaskStatus, task.Status, "Status is not expected.");
            }
        }
    }
}
