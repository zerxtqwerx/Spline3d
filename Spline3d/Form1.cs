using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Spline3d
{
    public class CubicSpline
    {
        public double[] X { get; private set; }
        public double[] Y { get; private set; }
        public double[] A { get; private set; }
        public double[] B { get; private set; }
        public double[] C { get; private set; }
        public double[] D { get; private set; }

        public CubicSpline(double[] x, double[] y)
        {
            X = x;
            Y = y;
            int n = x.Length - 1;

            A = new double[n + 1];
            B = new double[n];
            C = new double[n + 1];
            D = new double[n];

            for (int i = 0; i < n; i++)
            {
                A[i] = Y[i];
            }

            double[] h = new double[n];
            for (int i = 0; i < n; i++)
            {
                h[i] = X[i + 1] - X[i];
            }

            double[] alpha = new double[n];
            for (int i = 1; i < n-1; i++)
            {
                alpha[i] = (3 / h[i]) * (A[i + 1] - A[i]) - (3 / h[i - 1]) * (A[i] - A[i - 1]);
            }
            A[A.Length - 1] = y[y.Length - 1];
            double[] c_l = new double[n + 1];
            double[] c_u = new double[n];
            double[] c_z = new double[n + 1];

            c_l[0] = 1;
            c_u[0] = 0;
            c_z[0] = 0;

            for (int i = 1; i < n; i++)
            {
                c_l[i] = 2 * (X[i + 1] - X[i - 1]) - h[i - 1] * c_u[i - 1];
                c_u[i] = h[i] / c_l[i];
                c_z[i] = (alpha[i] - h[i - 1] * c_z[i - 1]) / c_l[i];
            }

            c_l[n] = 1;
            c_z[n] = 0;

            for (int j = n - 1; j >= 0; j--)
            {
                C[j] = c_z[j] - c_u[j] * C[j + 1];
                B[j] = (A[j + 1] - A[j]) / h[j] - h[j] * (C[j + 1] + 2 * C[j]) / 3;
                D[j] = (C[j + 1] - C[j]) / (3 * h[j]);
            }
        }

        public double GetValue(double x)
        {
            int n = X.Length - 1;
            if (x < X[0] || x > X[n])
                throw new ArgumentOutOfRangeException("x is out of range");

            int i = 0;
            while (i < n && x > X[i + 1]) i++;

            double dx = x - X[i];
            return A[i] + B[i] * dx + C[i] * dx * dx + D[i] * dx * dx * dx;
        }
    }

    public partial class Form1 : Form
    {
        private double[] x;
        private double[] y;
        private readonly ZedGraphControl zgc;

        public Form1()
        {
            this.Text = "Cubic Spline Example with ZedGraph";
            this.Size = new Size(800, 600);

            zgc = new ZedGraphControl();
            zgc.Dock = DockStyle.Fill;
            this.Controls.Add(zgc);

            // Задаем функцию
            GenerateData();

            DrawPlot();
        }

        private void GenerateData()
        {
            int points = 10; 
            x = new double[points];
            y = new double[points];

            double step = 1 / points;
            for (int i = 0; i < points; i++)
            {
                x[i] = step++; 
                y[i] = Math.Pow(x[i], 3); 
            }
        }

        private void DrawPlot()
        {
            var spline = new CubicSpline(x, y);

            GraphPane pane = zgc.GraphPane;
            pane.Title.Text = "Cubic Spline Interpolation";
            pane.XAxis.Title.Text = "x";
            pane.YAxis.Title.Text = "y";

            // Узлы
            PointPairList points = new PointPairList();
            for (int i = 0; i < x.Length; i++)
            {
                points.Add(x[i], y[i]);
            }
            pane.AddCurve("Функция", points, Color.Red, SymbolType.Circle);

            // Сплайн
            PointPairList splinePoints = new PointPairList();
            for (double t = x[0]; t <= x[x.Length - 1]; t += 0.01) 
            {
                splinePoints.Add(t, spline.GetValue(t));
            }
            pane.AddCurve("Кубический сплайн", splinePoints, Color.Blue, SymbolType.None);

            zgc.AxisChange();
            zgc.Invalidate();
        }

    }
}