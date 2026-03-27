
using Xunit;
using PCALib;
using System.Collections.Generic;

namespace PCALib_Tests
{
    public class StatisticFunctionsTests
    {
        [Fact]
        public void CovarianceMatrixOfDataset_ReturnsCorrectResult()
        {
            IList<double[]> dataset = new List<double[]> {
                new double[] { 1.0, 2.0, 3.0 },
                new double[] { 4.0, 5.0, 6.0 },
                new double[] { 7.0, 8.0, 9.0 }
            };

            double[][] expected = new double[][] {
                new double[] { 9.0, 9.0, 9.0 },
                new double[] { 9.0, 9.0, 9.0 },
                new double[] { 9.0, 9.0, 9.0 }
            };

            double[][] result = StatisticFunctions.CovarianceMatrix(dataset);
            Assert.True(Helper.AreEqualMatrices(expected, result, precision: 0.0001));
        }
    }
}