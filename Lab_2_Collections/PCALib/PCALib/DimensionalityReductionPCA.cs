
using System;
using System.Collections.Generic;

namespace PCALib
{
    public class DimensionalityReductionPCA : IDataTransformation<double[]>
    {
        private IList<double[]> _eigenVectors = null;

        public DimensionalityReductionPCA(IList<double[]> dataSet, double accuracyQR, int maxIterationQR, int componentsNumber)
        {
            double[][] cov = StatisticFunctions.CovarianceMatrix(dataSet);

            IList<double[][]> eigen = LinearAlgebra.QRIterativeEigenVectorValuesExtraction(cov, accuracyQR, maxIterationQR);
            IList<double[]> eigenVectors = LinearAlgebra.DecomposeMatrixToColumnVectors(eigen[0]);

            if (componentsNumber > eigenVectors.Count)
                throw new ArgumentException("componentsNumber > eigenVectors.Count");

            _eigenVectors = new List<double[]>();
            for (int i = 0; i < componentsNumber; i++)
                _eigenVectors.Add(eigenVectors[i]);
        }

        public double[] Transform(double[] dataItem)
        {
            if (_eigenVectors[0].Length != dataItem.Length)
                throw new ArgumentException("_eigenVectors[0].Length != dataItem.Length");

            double[] res = new double[_eigenVectors.Count];
            for (int i = 0; i < _eigenVectors.Count; i++)
            {
                res[i] = 0;
                for (int j = 0; j < dataItem.Length; j++)
                    res[i] += _eigenVectors[i][j] * dataItem[j];
            }

            return res;
        }

        public double[] Reconstruct(double[] transformedDataItem)
        {
            if (_eigenVectors.Count != transformedDataItem.Length)
                throw new ArgumentException("_eigenVectors.Count != transformedDataItem.Length");

            double[] res = new double[_eigenVectors[0].Length];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = 0;
                for (int j = 0; j < _eigenVectors.Count; j++)
                    res[i] += _eigenVectors[j][i] * transformedDataItem[j];
            }

            return res;
        }
    }
}
