using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmallTests
{
    internal class CancellationTokeTests
    {

        //Exception caught or not solely depends on if task is awaited or not.  Nothing special for TaskCanceledException which is sub class or OperationCanceledException
        [Test]
        public async Task SameCancellationTokenLeadToCancelNotFaultedCauseExceptionCaughtDependsOrNotOnAWait([Values(true, false)] bool waitTask)
        {
            Exception e = null;
            Task task = null;
            CancellationTokenSource cs1 = new CancellationTokenSource();
            try
            {

                task = Task.Factory.StartNew((state) =>
                {
                    int i = 0;
                    while (i++ < 100)
                    {
                        Thread.Sleep(10);
                        ((CancellationToken)state).ThrowIfCancellationRequested();
                    }

                }, cs1.Token, cs1.Token); //state to throw,  task assigned token

                cs1.Cancel(false);
                if (waitTask)
                {
                    await task.ConfigureAwait(false); //However, if the calling thread is not waiting on the task, this specific exception will not be propagated. 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test 1 Exception occurred : {e}"); //since waited , here it got TaskCanceledException which is subclass of OperationCanceledException
                e = ex;
            }

            Assert.IsTrue(cs1.IsCancellationRequested);
            Assert.IsTrue(cs1.Token.IsCancellationRequested);

            Thread.Sleep(20);
            Assert.AreEqual(waitTask, e != null); // if waited,there is exception. If not waited, there is no exception.

            //No matter waited or not waited, task is in same state:
            Assert.AreEqual(false, task.IsFaulted);
            Assert.IsNull(task.Exception);
            Assert.AreEqual(TaskStatus.Canceled, task.Status);
        }

        [Test]
        public async Task SameCancellationTokenContinueWithLeadToCanceledNotFaulted()
        {
            Exception e = null;
            Task task = null;
            using CancellationTokenSource cs1 = new CancellationTokenSource();
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

                }, cs1.Token, cs1.Token); //state, cs token

                cs1.Cancel(false);
            }
            catch (Exception ex)
            {
                //Never come to here since not awaited
                Console.WriteLine($"Test 1 Exception occurred : {e}");
                e = ex;
            }

            await task.ContinueWith((t) =>
            {
                Assert.AreEqual(false, t.IsFaulted);
                Assert.IsNull(t.Exception);
                Assert.AreEqual(TaskStatus.Canceled, t.Status);
            }).ConfigureAwait(false);

            Assert.IsTrue(cs1.IsCancellationRequested);
            Assert.IsTrue(cs1.Token.IsCancellationRequested);

            Thread.Sleep(20);
            Assert.IsNull(e); // if waited,there is exception. If not waited, there is no exception.

            //No matter waited or not waited, task is in same state:
            Assert.AreEqual(false, task.IsFaulted);
            Assert.IsNull(task.Exception);
            Assert.AreEqual(TaskStatus.Canceled, task.Status);
        }


        [Test]
        public async Task TaskOnDifferentChildThanChildCancelsWhenJointThrowsOnContinuanceAsFaulted([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();

            CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);

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

                }, csBoth.Token, cs2.Token); //state to throw on, cs token task is on
                // token task is on can be csBoth or cs1, no difference

                cs1.Cancel(false);

                if (waitTask)
                {
                    await task.ConfigureAwait(false); //However, if the calling thread is not waiting on the task, this specific exception will not be propagated. 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log :Test 2 Exception occurred : {e}"); //since waited , here it got TaskCanceledException which is subclass of OperationCanceledException
                e = ex;
            }

            await task.ContinueWith((t) =>
            {
                Assert.AreEqual(true, t.IsFaulted);
                Assert.IsNotNull(t.Exception);
                Assert.AreEqual(TaskStatus.Faulted, t.Status);
                Assert.IsTrue(task.Exception is AggregateException);
                Assert.IsTrue(task.Exception.GetBaseException() is OperationCanceledException);
                Assert.IsFalse(task.Exception.GetBaseException() is TaskCanceledException);
            }).ConfigureAwait(false);

            Assert.IsTrue(cs1.IsCancellationRequested);
            Assert.IsTrue(cs1.Token.IsCancellationRequested);

            Thread.Sleep(20); //More then sleep in while
            Assert.AreEqual(waitTask, e != null); // if waited,there is exception. If not waited, there is no exception. (even task in on different token, this is same as task on right token)
            if (waitTask)
            {
                Assert.IsTrue(e is OperationCanceledException); //strange here is OperationCanceledException
            }

            //No matter waited or not waited, task is in same state:
            Assert.AreEqual(true, task.IsFaulted, "Is Faulted?");
            Assert.IsNotNull(task.Exception);
            Assert.IsTrue(task.Exception is AggregateException);
            Assert.IsTrue(task.Exception.GetBaseException() is OperationCanceledException);
            Assert.IsFalse(task.Exception.GetBaseException() is TaskCanceledException);
            Assert.AreEqual(TaskStatus.Faulted, task.Status, "Task should be faulted.");
        }



        [Test]
        public async Task TaskOnDifferenChildThanTheChildCancelsCauseFaultedWhenThrowOnJointed2([Values(true, false)] bool waitTask)
        {
            using CancellationTokenSource cs1 = new CancellationTokenSource();
            using CancellationTokenSource cs2 = new CancellationTokenSource();

            CancellationTokenSource csBoth = CancellationTokenSource.CreateLinkedTokenSource(cs1.Token, cs2.Token);

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

                }, csBoth.Token, cs1.Token); //throws, task on
                // token task is on can be csBoth.Token or cs1.Token, no difference

                cs2.Cancel(false);


                if (waitTask)
                {
                    await task.ConfigureAwait(false); //However, if the calling thread is not waiting on the task, this specific exception will not be propagated. 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log :Test 1 Exception occurred : {e}"); //since awaited , here it got TaskCanceledException which is subclass of OperationCanceledException
                e = ex;
            }

            Thread.Sleep(20); //More then sleep in while
            Assert.AreEqual(waitTask, e != null); // if waited,there is exception. If not waited, there is no exception. (even task in on different token, this is same as task on right token)

            if (waitTask)
            {
                Assert.IsTrue(e is OperationCanceledException);
            }

            //No matter waited or not waited, task is in same state:
            Assert.AreEqual(true, task.IsFaulted, "Is Faulted?");
            Assert.IsNotNull(task.Exception);
            Assert.AreEqual(TaskStatus.Faulted, task.Status, "Task should be faulted.");
        }


    }
}
