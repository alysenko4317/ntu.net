
using Xunit;
using PCALib;
using System.Collections.Generic;

namespace PCALib_Tests
{
    public class DimensionalityReductionPCA_Tests
    {
        private IList<double[]> _data = null;
        private IDataTransformation<double[]> _transformation = null;
        private double[] _v = null;

        public DimensionalityReductionPCA_Tests()
        {
            _v = new double[] { 1, 0, 3 };

            _data = new List<double[]>() {
                new double[] {1, 2, 23},
                new double[] {-3, 17, 5},
                new double[] {13, -6, 7},
                new double[] {7, 8, -9}
            };

            _transformation = new DimensionalityReductionPCA(_data, 0.0001, 1000, 2);
        }

        [Fact]
        public void DimensionalityReductionPCATransformTest()
        {
            double[] reduced = _transformation.Transform(_v);
            double[] expReduced = new double[] { -2.75008, 0.19959 };
            Assert.True(Helper.AreEqualVectors(expReduced, reduced, 0.001));

            double[] reconstructed = _transformation.Reconstruct(reduced);
            double[] expReconstructed = new double[] { -0.21218, -0.87852, 2.60499 };
            Assert.True(Helper.AreEqualVectors(expReconstructed, reconstructed, 0.001));
        }
    }
}
