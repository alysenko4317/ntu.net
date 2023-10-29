using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

// A race condition occurs when two or more threads can access shared data and they try to change it
// at the same time. The final value of the shared variable depends on the order in which the threads
// operations are performed.

namespace raceConditionSample
{
    public class StateObject
    {
        private int state = 5;
        Mutex mutexObj = new Mutex();

        public void ChangeState(int loop)
        {
            mutexObj.WaitOne();
            //Console.Write('.');

            if (state == 5) {
                state++;
                Trace.Assert(state == 6, "Состязание за ресурсы возникло после " + loop + " циклов (" + state + ")");
            }

            state = 5;

            mutexObj.ReleaseMutex();
        }
    }

    public class SampleThread
    {
        public void RaceCondition(object obj)
        {
            Trace.Assert(obj is StateObject, "obj должен иметь тип StateObject");
            StateObject state = obj as StateObject;
            int i = 0;
            while (true)
                state.ChangeState(i++);
        }
    }

    class Program
    {
        public static void Main_()
        {
            var state = new StateObject();

            for (int i = 0; i < 20; i++)
                new Task(new SampleThread().RaceCondition, state).Start();

            Thread.Sleep(1000);
        }
    }
}
