using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CalcComponent
{
    public class Calculator
    {
        public int _varAddTwo;
        public int _varFact1;
        public int _varFact2;
        public int _varLoopValue;
        public double _varTotalCalculations = 0;

        public delegate void FactorialCompleteHandler(double Factorial, double TotalCalculations);
        public delegate void AddTwoCompleteHandler(int Result, double TotalCalculations);
        public delegate void LoopCompleteHandler(double TotalCalculations, int Counter);

        public event FactorialCompleteHandler FactorialComplete;
        public event FactorialCompleteHandler FactorialMinusOneComplete;
        public event AddTwoCompleteHandler AddTwoComplete;
        public event LoopCompleteHandler LoopComplete;

        // declares the variables you will use to hold your thread objects
        public Thread _factorialThread;
        public Thread _factorialMinusOneThread;
        public Thread _addTwoThread;
        public Thread _loopThread;

        public void factorialMinusOne()
        {
            // the method will calculate the value of a number minus 1 factorial (_varFact2-1!)

            double varResult = 1;
            double varTotalAsOfNow = 0;
            
            // performs a factorial calculation on varFact2 - 1.
            for (int varX = 1; varX <= _varFact2 - 1; varX++)
            {
                varResult *= varX;
                // increments varTotalCalculations and keeps track of the current
                // total as of this instant
                _varTotalCalculations += 1;
                varTotalAsOfNow = _varTotalCalculations;
            }

            Thread.Sleep(3000);

            // signals that the method has completed, and communicates the result
            // and a value of total calculations performed up to this point
            FactorialMinusOneComplete(varResult, varTotalAsOfNow);
        }

        public void factorialMinusOneAsync()
        {
            _factorialMinusOneThread =
                new System.Threading.Thread(
                    new System.Threading.ThreadStart(this.factorialMinusOne));
            _factorialMinusOneThread.Start();
        }

        public void factorial()
        {
            // thes method will calculate the value of a number factorial (_varFact1!)

            double varResult = 1;
            double varTotalAsOfNow = 0;
            for (int varX = 1; varX <= _varFact1; varX++)
            {
                varResult *= varX;
                _varTotalCalculations += 1;
                varTotalAsOfNow = _varTotalCalculations;
            }

            Thread.Sleep(3000);
            FactorialComplete(varResult, varTotalAsOfNow);
        }

        public void factorialAsync()
        {
            _factorialThread =
                new System.Threading.Thread(
                    new System.Threading.ThreadStart(this.factorial));
            _factorialThread.Start();   // starts the thread.
        }

        public void addTwo()
        {
            // The method will add two to a number (_varAddTwo+2)

            double varTotalAsOfNow = 0;
            int varResult = _varAddTwo + 2;
            _varTotalCalculations += 1;
            varTotalAsOfNow = _varTotalCalculations;

            Thread.Sleep(3000);
            AddTwoComplete(varResult, varTotalAsOfNow);
        }

        public void addTwoAsync()
        {
            _addTwoThread =
                new System.Threading.Thread(
                    new System.Threading.ThreadStart(this.addTwo));
            _addTwoThread.Start();
        }

        public void runLoop()
        {
            // the method will run a loop with a nested loop varLoopValue times

            int varX;
            double varTotalAsOfNow = 0;
            for (varX = 1; varX <= _varLoopValue; varX++)
            {
                // this nested loop is added solely for the purpose of slowing down
                // the program and creating a processor-intensive application.
                for (int varY = 1; varY <= 500; varY++)
                {
                    _varTotalCalculations += 1;
                    varTotalAsOfNow = _varTotalCalculations;
                }
            }

            Thread.Sleep(3000);
            LoopComplete(varTotalAsOfNow, _varLoopValue);
        }

        public void runLoopAsync()
        {
            _loopThread =
                 new System.Threading.Thread(
                     new System.Threading.ThreadStart(this.runLoop));
            _loopThread.Start();
        }
    }
}
