
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        int[] arr = new int[5];
        IList<int> lst = arr;
    
        // This line will throw a NotSupportedException
        // because the array has a fixed size.
        lst.Add(100);
    }
}
