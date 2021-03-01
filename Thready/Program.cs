using System;
using System.Collections.Generic;
using System.Threading;

namespace Thready
{
    class Program
    {
        static readonly object _lock = new object();
        private static SimpleStack<int> simpleStack = new SimpleStack<int>();
        static void Main(string[] args)
        {
            

            Random rand = new Random();

            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(CallThread);
                t.Start(rand);
                threads.Add(t);
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            static void CallThread(object randGen)
            {
                Random rnd = (Random) randGen;

                while (true)
                {
                    if (rnd.NextDouble() > 0.6)
                    {
                        simpleStack.Push(rnd.Next());
                    }
                    else
                    {
                        lock (_lock)
                        {
                            if (!simpleStack.IsEmpty)
                            {
                                simpleStack.Pop();
                            }
                        }
                    }
                }
            }

        }
    }
}
