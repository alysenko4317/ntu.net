using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCALib
{
    public interface IDataTransformation<T>
    {
        T Transform(T data);
        T Reconstruct(T transformedData);
    }

    public static class DataTransformationExtensions
    {
        public static IList<double[]>
        Transform(this IDataTransformation<double[]> transformer, IList<double[]> dataSet)
        {
            IList<double[]> Xt = new List<double[]>(dataSet.Count);
            foreach (double[] x in dataSet)
                Xt.Add(transformer.Transform(x));
            return Xt;
        }

        public static IList<double[]>
        Reconstruct(this IDataTransformation<double[]> transformer, IList<double[]> dataSet)
        {
            IList<double[]> Xt = new List<double[]>(dataSet.Count);
            foreach (double[] x in dataSet)
                Xt.Add(transformer.Reconstruct(x));
            return Xt;
        }
    }
}
