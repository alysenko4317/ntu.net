
using System;
using System.Collections.Generic;

namespace PCALib
{
    public static class LinearAlgebra
    {
        public static double ScalarVectorProduct(double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vector lengths are not equal.");

            double product = 0.0;
            for (int i = 0; i < a.Length; i++)
                product += a[i] * b[i];

            return product;
        }

        public static double[] ScalarToVectorProduct(double scalar, double[] vector)
        {
            double[] product = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
                product[i] = scalar * vector[i];

            return product;
        }

        public static double[] VectorProjection(double[] a, double[] b)
        {
            double k = ScalarVectorProduct(a, b) / ScalarVectorProduct(b, b);
            return ScalarToVectorProduct(k, b);
        }

        public static double[][] DecomposeMatrixToColumnVectors(double[][] matrix)
        {
            int numRows = matrix.Length;
            int numCols = matrix[0].Length;

            double[][] columnVectors = new double[numCols][];

            for (int col = 0; col < numCols; col++)
            {
                columnVectors[col] = new double[numRows];
                for (int row = 0; row < numRows; row++)
                    columnVectors[col][row] = matrix[row][col];
            }

            return columnVectors;
        }

        public static double NormOfVector(double[] vector)
        {
            double sumOfSquares = 0.0;
            for (int i = 0; i < vector.Length; i++)
                sumOfSquares += vector[i] * vector[i];
            return Math.Sqrt(sumOfSquares);
        }

        public static double[][] Transpose(double[][] matrix)
        {
            int numRows = matrix.Length;
            int numCols = matrix[0].Length;

            double[][] transpose = new double[numCols][];

            for (int col = 0; col < numCols; col++)
            {
                transpose[col] = new double[numRows];
                for (int row = 0; row < numRows; row++)
                    transpose[col][row] = matrix[row][col];
            }

            return transpose;
        }

        public static List<double[][]> QRDecomposition(double[][] a)
        {
            List<double[]> av = new List<double[]>();
            foreach (double[] vector in LinearAlgebra.DecomposeMatrixToColumnVectors(a))
                av.Add(vector);

            List<double[]> u = new List<double[]>();
            u.Add(av[0]);

            List<double[]> e = new List<double[]>();
            e.Add(LinearAlgebra.ScalarToVectorProduct(1 / LinearAlgebra.NormOfVector(u[0]), u[0]));

            for (int i = 1; i < a.Length; i++)
            {
                double[] projAcc = new double[a.Length];
                for (int j = 0; j < projAcc.Length; j++)
                    projAcc[j] = 0;

                for (int j = 0; j < i; j++)
                {
                    double[] proj = VectorProjection(av[i], e[j]);
                    for (int k = 0; k < projAcc.Length; k++)
                        projAcc[k] += proj[k];
                }

                double[] ui = new double[a.Length];
                for (int j = 0; j < ui.Length; j++)
                    ui[j] = a[j][i] - projAcc[j];

                u.Add(ui);
                e.Add(ScalarToVectorProduct(1 / NormOfVector(u[i]), u[i]));
            }

            double[][] q = new double[a.Length][];
            for (int i = 0; i < q.Length; i++)
            {
                q[i] = new double[a.Length];
                for (int j = 0; j < q[i].Length; j++)
                    q[i][j] = e[j][i];
            }

            double[][] r = new double[a.Length][];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = new double[a.Length];
                for (int j = 0; j < r[i].Length; j++)
                {
                    if (i >= j)
                        r[i][j] = ScalarVectorProduct(e[j], av[i]);
                    else
                        r[i][j] = 0;
                }
            }

            r = LinearAlgebra.Transpose(r);
            List<double[][]> res = new List<double[][]>();
            res.Add(q);
            res.Add(r);

            return res;
        }

        public static double[][] MatricesProduct(double[][] a, double[][] b)
        {
            int rowsA = a.Length;
            int colsA = a[0].Length;
            int rowsB = b.Length;
            int colsB = b[0].Length;

            if (colsA != rowsB)
                throw new ArgumentException("The number of columns in the first matrix must match the number of rows in the second matrix.");

            double[][] result = new double[rowsA][];

            for (int i = 0; i < rowsA; i++)
            {
                result[i] = new double[colsB];
                for (int j = 0; j < colsB; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < colsA; k++)
                        sum += a[i][k] * b[k][j];
                    result[i][j] = sum;
                }
            }

            return result;
        }

        public static List<double[][]>
        QRIterativeEigenVectorValuesExtraction(double[][] a, double accuracy, int maxIterations)
        {
            double[][] aItr = a;
            double[][] q = null;

            for (int i = 0; i < maxIterations; i++)
            {
                IList<double[][]> qr = QRDecomposition(aItr);
                aItr = MatricesProduct(qr[1], qr[0]);
                if (q == null)
                {
                    q = qr[0];
                }
                else
                {
                    bool accuracyAcheived = true;
                    double[][] qNew = MatricesProduct(q, qr[0]);
                    for (int n = 0; n < q.Length; n++)
                    {
                        for (int m = 0; m < q[n].Length; m++)
                        {
                            if (Math.Abs(Math.Abs(qNew[n][m]) - Math.Abs(q[n][m])) > accuracy)
                            {
                                accuracyAcheived = false;
                                break;
                            }
                        }
                        if (!accuracyAcheived)
                            break;
                    }
                    q = qNew;
                    if (accuracyAcheived)
                        break;
                }
            }

            List<double[][]> res = new List<double[][]>();
            res.Add(q);
            res.Add(aItr);
            return res;
        }
    }
}