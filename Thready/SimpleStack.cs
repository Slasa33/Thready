
    using System;
    using System.Collections.Generic;
    
    namespace Thready
{
        public class SimpleStack<T>
        {
            private static bool _done;
            static readonly object _locker = new object();

            private List<T> data = new List<T>();

            public T Top
            {
                get
                {
                    lock (_locker)
                    {
                        int idx = this.data.Count - 1;
                        if (idx == -1)
                        {
                            throw new StackEmptyException();
                        }

                        return data[idx];
                    }
                }
            }


            public bool IsEmpty
            {
                get
                {
                    lock (_locker)
                    {
                        return this.data.Count == 0;
                    }
                }
            }



            public void Push(T val)
            {
                lock (_locker)
                {
                    this.data.Add(val);
                }
            }


            public T Pop()
            {
                T val;
                lock (_locker)
                {
                    int idx = this.data.Count - 1;
                    if (idx == -1)
                    {
                        throw new StackEmptyException();
                    }

                    val = this.data[idx];
                    this.data.RemoveAt(idx);
                }
                return val;
            }


            public class StackEmptyException : Exception
            {

            }
        }
    }
