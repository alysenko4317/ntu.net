
using PCALib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PCALib_Graph
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

    public partial class MainForm : Form
    {
        private int[] _x = Enumerable.Range(1, 10).ToArray();

        private double[] _y = {
            1.07316461, 3.06854049, 6.48392454, 4.17343951, 6.55016433,
            10.87542494, 11.97433776, 16.62849467, 16.18395185, 17.1753926
        };

        private IList<double[]> _X;
        private IList<double[]> _Xcentered;
        private IList<double[]> _Xreduced;
        private IList<double[]> _Xrestored;

        public MainForm()
        {
            InitializeComponent();

            // construct the 2D dataset
            _X = Enumerable.Zip(_x, _y, (a, b) => new double[] { a, b }).ToList();

            // centering the dataset
            IDataTransformation<double[]> mct = new MeanCenteringTransformer(_X);
            _Xcentered = mct.Transform(_X);

            // applying PCA, dimensionality reduction to 1D
            IDataTransformation<double[]> pca = new DimensionalityReductionPCA(_Xcentered, 0.0001, 1000, 1);
            _Xreduced = pca.Transform(_Xcentered);

            // reconstruct the dataset to original dimensionality
            _Xrestored = mct.Reconstruct(pca.Reconstruct(_Xreduced));

            textBox1.Text = _X.ToString(2);
            textBox2.Text = _Xreduced.ToString(2);
            textBox3.Text = _Xrestored.ToString(2);

            this.Text = "Data Chart";
        }

        static void
        scatterPlot(PictureBox ctrl, Graphics g,
                    double[] x, double[] y,
                    double xMin, double xMax,
                    double yMin, double yMax)
        {
            // calculate the scale and dimensions for the chart
            int chartWidth = ctrl.ClientSize.Width - 40;
            int chartHeight = ctrl.ClientSize.Height - 40;
            float xScale = chartWidth / (float)(xMax - xMin);
            float yScale = chartHeight / (float)(yMax - yMin);

            // draw the chart axes
            g.DrawLine(Pens.Black, 20, 20, 20, 20 + chartHeight); // vertical axis
            g.DrawLine(Pens.Black, 20, 20 + chartHeight, 20 + chartWidth, 20 + chartHeight); // horizontal axis

            // draw the numeric labels on the axes
            using (Font font = new Font("Arial", 8))
            {
                // draw x-axis labels
                int numXLabels = 5; // number of x-axis labels to draw
                for (int i = 0; i < numXLabels; i++)
                {
                    float xPos = 20 + i * (chartWidth / (float)(numXLabels - 1));
                    float yPos = 20 + chartHeight;

                    double labelValue = xMin + i * (xMax - xMin) / (numXLabels - 1);
                    string label = labelValue.ToString("0.0");
                    SizeF labelSize = g.MeasureString(label, font);
                    float labelX = xPos - labelSize.Width / 2;
                    float labelY = yPos + 5;

                    g.DrawString(label, font, Brushes.Black, labelX, labelY);
                    g.DrawLine(Pens.Black, xPos, yPos - 5, xPos, yPos + 5);  // tick mark
                }

                // draw y-axis labels
                int numYLabels = 5; // number of y-axis labels to draw
                for (int i = 0; i < numYLabels; i++)
                {
                    float xPos = 20;
                    float yPos = 20 + i * (chartHeight / (float)(numYLabels - 1));

                    double labelValue = yMin + i * (yMax - yMin) / (numYLabels - 1);
                    string label = labelValue.ToString("0.0");
                    SizeF labelSize = g.MeasureString(label, font);
                    float labelX = xPos - labelSize.Width+1;
                    float labelY = yPos - labelSize.Height / 2;

                    labelY = 20 + chartHeight - labelY;  // invert the labelY position

                    g.DrawString(label, font, Brushes.Black, labelX, labelY);
                    g.DrawLine(Pens.Black, xPos - 5, yPos, xPos + 5, yPos);  // tick mark
                }
            }

            Pen tickPen = new Pen(Color.Blue, 2);

            // draw the data points as a scatter plot
            for (int i = 0; i < x.Length; i++)
            {
                float xPos = 20 + (float)((x[i] - xMin) * xScale);
                float yPos = 20 + chartHeight - (float)((y[i] - yMin) * yScale);
                //g.FillEllipse(Brushes.Blue, xPos - 3, yPos - 3, 6, 6);
                int markSize = 3;
                g.DrawLine(tickPen, xPos - markSize, yPos - markSize, xPos + markSize, yPos + markSize);
                g.DrawLine(tickPen, xPos + markSize, yPos - markSize, xPos - markSize, yPos + markSize);
            }
        }


        static void scatterPlot(PictureBox ctrl, Graphics g, double[] x, double[] y) {
            scatterPlot(ctrl, g, x, y, x.Min(), x.Max(), y.Min(), y.Max());
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            scatterPlot(pictureBox1, e.Graphics, Array.ConvertAll(_x, e => (double)e), _y);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            IList<double[]> X = _Xrestored;

            double[] x = X.Select(arr => arr[0]).ToArray();
            double[] y = X.Select(arr => arr[1]).ToArray();

            scatterPlot(pictureBox2, e.Graphics, x, y);
        }

        private static double[] generateRandomData(int length, double scale)
        {
            Random random = new Random();
            double[] data = new double[length];
            for (int i = 0; i < length; i++)
                data[i] = random.NextDouble() * scale;
            return data;
        }
    }
}
