﻿using System;
using System.Collections;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace reactive_programming
{

    class Program
    {

        private void Test1()
        {
            IObservable<string> obj = Observable.Generate(0, _ => true, i => i + 1, i => new string('#', i), i => TimeSelector(i));

            using (obj.Subscribe(Console.WriteLine))
            {
                Console.WriteLine("Press any key to exit!!!");
                Console.ReadLine();
            }

            Console.WriteLine("Hello World!");
        }


        public class MyObservable : IObservable<int>
        {
            public IDisposable Subscribe(IObserver<int> observer)
            {
                observer.OnNext(1);
                observer.OnNext(2);
                observer.OnNext(3);
                return Disposable.Empty;
            }
        }

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

        private void Test2()
        {
            //MyObservable observable = new MyObservable();
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

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Test2();
        }


        private static TimeSpan TimeSelector(int i)
        {
            return TimeSpan.FromSeconds(i);
        }
    }
}