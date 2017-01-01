﻿namespace HighPerformanceTests.thread
{
    public class Barrier // extends SimpleBarrier implements RunDelayed
    {

        //        /**
        //         * The Stack containing waiting Runnables. (Stack so its quick and easy to
        //         * have them wait and to remove them.)
        //         */

        //        protected Stack runnablesWaiting = null;// new Stack();//

        //        /**
        //         * The default queue into which to put (runDelayed) runnables that are
        //         * waiting on any of these Barriers. The limit on the number of threads that
        //         * can be running the (runDelayed) objects is the amount of memory available.
        //         * The (runDelayed) objects are placed in this queue by default when a Future
        //         * is given a value.
        //         */

        //        protected static final RunQueue classRunQueue = new RunQueue();

        //        /**
        //         * The queue into which to put (runDelayed) runnables that are waiting on
        //         * this Barrier. The limit on the number of threads that can be running the
        //         * (runDelayed) objects is the amount of memory available. They are placed in
        //         * the queue when the Future is given a value.
        //         */

        //        protected RunQueue runQueue = classRunQueue;

        //        /**
        //         * Creates a Barrier at which n Threads or Runnables may repeatedly gather.
        //         *
        //         * @param n total number of threads that must gather.
        //         */

        //        public Barrier(int n)
        //        {
        //            super(n);
        //        }

        //        /**
        //         * Is called by a thread to wait for the rest of the n Threads or Runnables
        //         * to gather before the set of threads or runnables may continue executing.
        //         *
        //         * @throws InterruptedException If interrupted while waiting.
        //         */
        //        public synchronized void gather() throws InterruptedException
        //        {
        //        if (--count > 0)
        //            wait();
        //        else {
        //            releaseRunnables();
        //        count = initCount;
        //            notifyAll();
        //        }
        //    }

        //    /**
        //     * Is a non-delaying version of gather().
        //     */
        //    public synchronized void signal()
        //    {
        //        if (--count == 0)
        //        {
        //            releaseRunnables();
        //            count = initCount;
        //            notifyAll();
        //        }
        //    }

        //    protected void releaseRunnables()
        //    {
        //        RunQueue q = getRunQueue();
        //        if (runnablesWaiting != null)
        //        {
        //            while (!runnablesWaiting.empty())
        //            {
        //                q.run((Runnable)runnablesWaiting.pop());
        //            }
        //            // runnablesWaiting=null;
        //        }
        //    }

        //    /**
        //     * Get the RunQueue for a Barrier object. The run queue should be changed
        //     * with setRunQueue for more precise control.
        //     *
        //     * @return The RunQueue that objects runDelayed on a Barrier object will be
        //     *         placed in.
        //     */

        //    public RunQueue getRunQueue()
        //    {
        //        return runQueue;
        //    }

        //    /**
        //     * Set the RunQueue for a Barrier object.
        //     */

        //    public void setRunQueue(RunQueue rq)
        //    {
        //        runQueue = rq;
        //    }

        //    /**
        //     * Get the RunQueue for the Barrier class.
        //     *
        //     * @return The RunQueue that objects runDelayed on a pure Future will be
        //     *         placed in.
        //     */

        //    public static RunQueue getClassRunQueue()
        //    {
        //        return classRunQueue;
        //    }

        //    /**
        //     * Schedule a runnable object to execute when the Barrier has gathered the
        //     * correct number of threads or runnables.
        //     */

        //    public synchronized void runDelayed(Runnable r)
        //    {
        //        if (--count > 0)
        //        {
        //            if (runnablesWaiting == null)
        //            {
        //                runnablesWaiting = new Stack();
        //            }
        //            runnablesWaiting.push(r);
        //        }
        //        else
        //        {
        //            releaseRunnables();
        //            count = initCount;
        //            notifyAll();
        //            getRunQueue().run(r);
        //        }
        //    }

    }
}