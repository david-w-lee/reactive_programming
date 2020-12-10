using System;
using System.Collections;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace reactive_programming
{

    class Program
    {
        public class MyObserver : IObserver<int>
        {
            public void OnCompleted()
            {
                Console.WriteLine("End");
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(int value)
            {
                Console.WriteLine(value);
            }
        }


        /// <summary>
        /// In an Observable, we invoke observers as callbacks.
        /// </summary>
        private void Test1()
        {
            IObservable<int> observable = Observable.Create<int>(obsvr =>
            {
                obsvr.OnNext(1);
                Thread.Sleep(1000);
                obsvr.OnNext(2);
                Thread.Sleep(1000);
                obsvr.OnNext(3);
                return Disposable.Empty;
            });
            MyObserver observer = new MyObserver();

            using (observable.Subscribe(observer))
            {
                Console.ReadLine();
            }

            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// Subject allow us to invoke OnNext(), OnComplete(), OnError().
        /// This allows us multicast messages to observers.
        /// </summary>
        private void Test2()
        {
            Subject<string> messenger = new Subject<string>();
            messenger.Subscribe(msg => { Console.WriteLine($"msg: {msg}"); });
            messenger.Subscribe(msg => { File.AppendAllText(@"c:\projects\file.txt", $"msg: {msg}" + Environment.NewLine);});

            messenger.OnNext("Message 1");
            messenger.OnNext("Message 2");

            // Returns the last message before subscription.
            BehaviorSubject<string> bmessenger = new BehaviorSubject<string>("start point");

            // Returns all the messages before subscritiption.
            ReplaySubject<string> rmessenger = new ReplaySubject<string>();

            Console.ReadLine();
        }

        // BehaviorSubject returns the last message before subscription.
        private void Test3()
        {
            BehaviorSubject<string> messenger = new BehaviorSubject<string>("start point");
            messenger.Subscribe(msg => { Console.WriteLine($"David received: {msg}"); });
            
            messenger.OnNext("Message 1");
            messenger.OnNext("Message 2");

            messenger.Subscribe(msg => { Console.WriteLine($"Nick received: {msg}"); });

            Console.ReadLine();
        }

        // ReplaySubject returns all the messages before subscritiption.
        private void Test4()
        {
            ReplaySubject<string> messenger = new ReplaySubject<string>();
            messenger.Subscribe(msg => { Console.WriteLine($"David received: {msg}"); });

            messenger.OnNext("Message 1");
            messenger.OnNext("Message 2");

            messenger.Subscribe(msg => { Console.WriteLine($"Nick received: {msg}"); });

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Test2();
        }

    }
}
