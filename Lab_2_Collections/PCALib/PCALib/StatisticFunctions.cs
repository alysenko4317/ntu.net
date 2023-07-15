
using System.Collections.Generic;

namespace PCALib
{
    public class StatisticFunctions
    {
        public static double[][] CovarianceMatrix(IList<double[]> dataSet)
        {
            int numRows = dataSet.Count;
            int numCols = dataSet[0].Length;

            double[] meanVector = new double[numCols];
            for (int col = 0; col < numCols; col++)
            {
                double sum = 0.0;
                for (int row = 0; row < numRows; row++)
                    sum += dataSet[row][col];
                meanVector[col] = sum / numRows;
            }

            double[][] covarianceMatrix = new double[numCols][];
            for (int i = 0; i < numCols; i++)
            {
                covarianceMatrix[i] = new double[numCols];
                for (int j = 0; j < numCols; j++)
                {
                    double sum = 0.0;
                    for (int row = 0; row < numRows; row++)
                        sum += (dataSet[row][i] - meanVector[i]) * (dataSet[row][j] - meanVector[j]);
                    covarianceMatrix[i][j] = sum / (numRows - 1);
                }
            }

            return covarianceMatrix;
        }
    }
}
