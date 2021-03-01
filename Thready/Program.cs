using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Microsoft.VisualBasic;

namespace Thready2
{
    class Program
    {
        static void Main(string[] args)
        {
            object _lock = new object();
            SimpleStack<int> simpleStack = new SimpleStack<int>();

            Random rand = new Random();


            Thread t = new Thread((() =>
            {
                while (true)
                {
                    simpleStack.Push(rand.Next());
                    lock (_lock)
                    {
                        Monitor.Pulse(_lock);
                    }

                    Thread.Sleep(100);
                }
            }));
            t.Start();


            List<Thread> threads = new List<Thread>();

            int id;

            for (int i = 0; i < 5; i++)
            {
                Thread t2 = new Thread((() =>
                {
                    while (true)
                    {
                        lock (_lock)
                        {
                            if (simpleStack.IsEmpty)
                            {
                                do
                                {
                                    Monitor.Wait(_lock);
                                } while (simpleStack.IsEmpty);
                            }

                            id = simpleStack.Pop(); 
                        }

                        Console.WriteLine($"{id}, {Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(rand.Next(40,1001));
                    }
                }));
                t2.Start();
                threads.Add(t2);
            }

            t.Join();
            foreach (var t2 in threads)
            {
                t2.Join();
            }

        }
    }
}
