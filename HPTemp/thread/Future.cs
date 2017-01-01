
namespace HighPerformanceTests.thread
{
    public class Future<T> : SimpleFuture<T> // , IThreadPoolWorkItem, IAsyncResult, IDisposable
    {
        private readonly object _lock = new object();
        // protected Stack runnablesWaiting = null;
        // protected static RunQueue classRunQueue = new RunQueue();
        // protected RunQueue runQueue = classRunQueue;

        public Future()
        { }

        public Future(T value)
            : base(value)
        { }

        public override T Value
        {
            set
            {
                lock (_lock)
                {
                    base.Value = value;
                    //if (runnablesWaiting != null)
                    //{
                    //    while (!runnablesWaiting.empty())
                    //        getRunQueue().run((Runnable) runnablesWaiting.pop());
                    //    runnablesWaiting = null;
                    //}
                }
            }
        }

        //public RunQueue getRunQueue() {return runQueue;}

        //public void setRunQueue(RunQueue rq) { runQueue = rq; }

        //public static RunQueue getClassRunQueue() { return classRunQueue; }

        //public synchronized void runDelayed(Runnable r) 
        //{
        //   if (value != this)
        //      runQueue.run(r);
        //   else {
        //      if (runnablesWaiting == null)
        //         runnablesWaiting = new Stack();
        //      runnablesWaiting.push(r);
        //   }
        //}
    }
}