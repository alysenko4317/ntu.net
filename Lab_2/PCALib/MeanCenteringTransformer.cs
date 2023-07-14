using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCALib
{
    public class MeanCenteringTransformer : IDataTransformation<double[]>
    {
        private int _numFeatures;
        private int _numSamples;
        private double[] _featureMeans;

        public MeanCenteringTransformer(IList<double[]> dataSet)
        {
            _numFeatures = dataSet[0].Length;
            _numSamples = dataSet.Count;

            // Calculate the mean of each feature
            _featureMeans = new double[_numFeatures];

            foreach (double[] dataPoint in dataSet)
                for (int i = 0; i < _numFeatures; i++)
                    _featureMeans[i] += dataPoint[i];

            for (int i = 0; i < _numFeatures; i++)
                _featureMeans[i] /= _numSamples;
        }

        public double[] Transform(double[] dataItem)
        {
            double[] pt = new double[_numFeatures];
            for (int j = 0; j < _numFeatures; j++)
                pt[j] = dataItem[j] - _featureMeans[j];
            return pt;
        }

        public double[] Reconstruct(double[] transformedDataItem)
        {
            double[] pt = new double[_numFeatures];
            for (int j = 0; j < _numFeatures; j++)
                pt[j] = transformedDataItem[j] + _featureMeans[j];
            return pt;
        }
    }
}
