namespace HighPerformanceTests.thread
{
    public class Lock
    {
        protected readonly object locked = new object();

        public void @lock() // throws InterruptedException 
        {
            //while (System.Threading.Monitor.IsEntered(locked))
            //    System.Threading.Monitor.Wait(locked);
            System.Threading.Monitor.Enter(locked);
        }

        public void unlock()
        {
            System.Threading.Monitor.Exit(locked);
            System.Threading.Monitor.PulseAll(locked);
        }
    }
}