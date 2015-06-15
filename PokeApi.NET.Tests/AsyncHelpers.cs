using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PokeAPI.Tests
{
    // from http://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously , because Task.RunSynchronously complains

    class ExclusiveSynchronizationContext : SynchronizationContext
    {
        readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
        readonly Queue<Tuple<SendOrPostCallback, object>> items = new Queue<Tuple<SendOrPostCallback, object>>();

        bool done;

        public Exception InnerException { get; set; }

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException("We cannot send to our same thread");
        }
        public override void Post(SendOrPostCallback d, object state)
        {
            lock (items)
            {
                items.Enqueue(Tuple.Create(d, state));
            }
            workItemsWaiting.Set();
        }

        public void EndMessageLoop()
        {
            Post(_ => done = true, null);
        }
        public void BeginMessageLoop()
        {
            while (!done)
            {
                Tuple<SendOrPostCallback, object> task = null;
                lock (items)
                {
                    if (items.Count > 0)
                        task = items.Dequeue();
                }
                if (task != null)
                {
                    task.Item1(task.Item2);
                    if (InnerException != null)
                        throw new Exception("AsyncHelpers.Run method threw an exception.", InnerException);
                }
                else
                {
                    workItemsWaiting.WaitOne();
                }
            }
        }

        public override SynchronizationContext CreateCopy() => this;
    }

    public static class AsyncHelpers
    {
        public static void RunSync(Func<Task> task)
        {
            var oc = SynchronizationContext.Current;
            var n = new ExclusiveSynchronizationContext();

            SynchronizationContext.SetSynchronizationContext(n);

            n.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e) when (!Debugger.IsAttached)
                {
                    n.InnerException = e;
                    throw;
                }
                finally
                {
                    n.EndMessageLoop();
                }
            }, null);

            n.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oc);
        }
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oc = SynchronizationContext.Current;
            var n = new ExclusiveSynchronizationContext();

            SynchronizationContext.SetSynchronizationContext(n);

            T ret = default(T);

            n.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e) when (!Debugger.IsAttached)
                {
                    n.InnerException = e;
                    throw;
                }
                finally
                {
                    n.EndMessageLoop();
                }
            }, null);

            n.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oc);

            return ret;
        }
    }
}
