
using System;
using System.Collections;
using System.Linq;

namespace Lab2_App
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] numbers = { "40", "2012", "176", "5" };

            // convert array of strings into array of ints using LINQ
            // with sorting
            int[] nums = numbers.Select(s => Int32.Parse(s))
                                .OrderBy(s => s)
                                .ToArray();

        }
    }
}