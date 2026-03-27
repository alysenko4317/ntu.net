
using System.Linq;
using System.Collections.Generic;
using PCALib;
using System.Text;
using System;

namespace Lab2_App
{
    public static class IListExtensions
    {
        public static string ToString(this IList<double[]> dataSet, int decimalPlaces = 2)
        {
            StringBuilder sb = new StringBuilder();

            foreach (double[] arr in dataSet)
            {
                foreach (double e in arr)
                    sb.Append(e.ToString("F" + decimalPlaces) + " ");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] x = Enumerable.Range(1, 10).ToArray();

            double[] y = {
                1.07316461, 3.06854049, 6.48392454, 4.17343951, 6.55016433,
                10.87542494, 11.97433776, 16.62849467, 16.18395185, 17.1753926
            };

            // construct the 2D dataset
            IList<double[]> X = Enumerable.Zip(x, y, (a, b) => new double[] { a, b }).ToList();

            // centering the dataset
            IDataTransformation<double[]> mct = new MeanCenteringTransformer(X);
            IList<double[]> X_centered = mct.Transform(X);

            // applying PCA, dimensionality reduction to 1D
            IDataTransformation<double[]> pca = new DimensionalityReductionPCA(X_centered, 0.0001, 1000, 1);
            IList<double[]> X_reduced = pca.Transform(X_centered);

            // reconstruct the dataset to original dimensionality
            IList<double[]> X_restored = mct.Reconstruct(pca.Reconstruct(X_reduced));

            Console.WriteLine(X.ToString(2));
            Console.WriteLine(X_reduced.ToString(2));
            Console.WriteLine(X_restored.ToString(2));
        }
    }
}
