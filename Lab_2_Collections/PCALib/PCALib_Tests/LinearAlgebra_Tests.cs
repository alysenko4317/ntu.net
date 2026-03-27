
using Xunit;
using System;
using System.Linq;
using System.Collections.Generic;
using PCALib;

namespace PCALib_Tests
{
    public class LinearAlgebraTests
    {
        [Fact]
        public void QRDecompositionTest()
        {
            double[][] a = new double[][] {
                new double[] {12, -51, 4},
                new double[] {6, 167, -68},
                new double[] {-4, 24, -41}
            };

            IList<double[][]> res = LinearAlgebra.QRDecomposition(a);

            double[][] expQ = new double[][] {
                new double[] {6.0/7, -69.0/175, -58.0/175},
                new double[] {3.0/7, 158.0/175, 6.0/175},
                new double[] {-2.0/7, 6.0/35, -33.0/35}
            };

            double[][] expR = new double[][] {
                new double[] {14, 21, -14},
                new double[] {0, 175, -70},
                new double[] {0, 0, 35}
            };

            Assert.True(Helper.AreEqualMatrices(expQ, res[0], 0.0001), "expQ != Q");
            Assert.True(Helper.AreEqualMatrices(expR, res[1], 0.0001), "expR != R");
        }

        [Fact]
        public void QRIterativeEigenVectorValuesExtractionTest()
        {
            double[][] a = new double[][] {
                new double[] {1, 2, 4},
                new double[] {2, 9, 8},
                new double[] {4, 8, 2}
            };

            IList<double[][]> ev = LinearAlgebra.QRIterativeEigenVectorValuesExtraction(a, 0.001, 1000);

            double expEV00 = 15.2964;
            double expEV11 = 4.3487;
            double expEV22 = 1.0523;

            Assert.Equal(expEV00, Math.Round(Math.Abs(ev[1][0][0]), 4));
            Assert.Equal(expEV11, Math.Round(Math.Abs(ev[1][1][1]), 4));
            Assert.Equal(expEV22, Math.Round(Math.Abs(ev[1][2][2]), 4));
        }

        [Fact]
        public void ScalarVectorProduct_ReturnsCorrectResult()
        {
            double[] a = { 1.0, 2.0, 3.0 };
            double[] b = { 4.0, 5.0, 6.0 };

            Assert.Equal(32.0, LinearAlgebra.ScalarVectorProduct(a, b));
        }

        [Fact]
        public void ScalarVectorProduct_ThrowsArgumentExceptionForUnequalVectorLengths()
        {
            double[] a = { 1.0, 2.0, 3.0 };
            double[] b = { 4.0, 5.0 };

            Assert.Throws<ArgumentException>(() => LinearAlgebra.ScalarVectorProduct(a, b));
        }

        [Fact]
        public void ScalarToVectorProduct_ReturnsCorrectResult()
        {
            double scalar = 2.0;
            double[] vector = { 1.0, 2.0, 3.0 };
            double[] expected = { 2.0, 4.0, 6.0 };

            Assert.Equal(expected, LinearAlgebra.ScalarToVectorProduct(scalar, vector));
        }

        [Fact]
        public void VectorProjection_ReturnsCorrectResult()
        {
            double[] a = { 1.0, 2.0, 3.0 };
            double[] b = { 4.0, 5.0, 6.0 };

            double[] result = LinearAlgebra.VectorProjection(a, b);
            double[] expected = { 1.66233766, 2.07792208, 2.49350649 };

            Assert.True(expected.Zip(result, (e, r) => Math.Abs(e - r) <= 0.001).All(b => b));
        }

        [Fact]
        public void DecomposeMatrixToColumnVectors_ReturnsCorrectResult()
        {
            double[][] matrix = new double[][] {
                new double[] { 1.0, 2.0, 3.0 },
                new double[] { 4.0, 5.0, 6.0 },
                new double[] { 7.0, 8.0, 9.0 }
            };

            double[][] result = LinearAlgebra.DecomposeMatrixToColumnVectors(matrix);

            double[][] expected = {
            new double[] { 1.0, 4.0, 7.0 },
            new double[] { 2.0, 5.0, 8.0 },
            new double[] { 3.0, 6.0, 9.0 }
        };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NormOfVector_ReturnsCorrectResult()
        {
            double[] vector = { 3.0, 4.0 };
            Assert.Equal(5.0, LinearAlgebra.NormOfVector(vector));
        }

        [Fact]
        public void Transpose_ReturnsCorrectResult()
        {
            double[][] matrix = {
                new double[] { 1.0, 2.0, 3.0 },
                new double[] { 4.0, 5.0, 6.0 }
            };

            double[][] expected = {
                new double[] { 1.0, 4.0 },
                new double[] { 2.0, 5.0 },
                new double[] { 3.0, 6.0 }
            };

            Assert.Equal(expected, LinearAlgebra.Transpose(matrix));
        }

        [Fact]
        public void MatricesProduct_ReturnsCorrectResult()
        {
            double[][] a = new double[][] {
                new double[] { 1.0, 2.0 },
                new double[] { 3.0, 4.0 },
                new double[] { 5.0, 6.0 }
            };

            double[][] b = new double[][] {
                new double[] { 7.0, 8.0 },
                new double[] { 9.0, 10.0 }
            };

            double[][] expected = new double[][] {
                new double[] { 25.0, 28.0 },
                new double[] { 57.0, 64.0 },
                new double[] { 89.0, 100.0 }
            };

            Assert.Equal(expected, LinearAlgebra.MatricesProduct(a, b));
        }
    }
}
