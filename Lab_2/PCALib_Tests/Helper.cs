using System;

namespace PCALib_Tests
{
    public class Helper
    {
        public static bool
        AreEqualVectors(double[] a, double[] b, double precision)
        {
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (Math.Abs(a[i] - b[i]) > precision)
                    return false;
            }

            return true;
        }

        public static bool
        AreEqualMatrices(double[][] a, double[][] b, double precision)
        {
            if (a.Length != b.Length || a[0].Length != b[0].Length)
                return false;

            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < a[0].Length; j++)
                    if (Math.Abs(a[i][j] - b[i][j]) > precision)
                        return false;

            return true;
        }
    }
}
