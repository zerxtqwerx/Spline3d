using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZedGraph;

namespace Spline3d
{ 
    public partial class Form1 : Form
    {
        ZedGraphControl zedGraphControl1 = null;

        public Form1()
        {
            int n = 10;
            zedGraphControl1 = new ZedGraphControl { Dock = DockStyle.Fill };
            this.Controls.Add(zedGraphControl1);
            GraphPane graphPane = zedGraphControl1.GraphPane;
            graphPane.Title.Text = "График функции и кубического сплайна";
            graphPane.XAxis.Title.Text = "x";
            graphPane.YAxis.Title.Text = "f(x)";

            // Данные для функции f(x) = x^3 - x
            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();
            for (double x = -1; x <= 1; x += 0.1)
            {
                xValues.Add(x);
                yValues.Add(Function(x));
            }

            // Добавление функции в график
            LineItem functionCurve = graphPane.AddCurve("f(x)", xValues.ToArray(), yValues.ToArray(), System.Drawing.Color.Blue, SymbolType.None);

            // Параметры для кубического сплайна
            double[] splineX = new double[n];
            double[] splineY = new double[n];

            // Интерполяция для создания сплайна
            for (int i = 0; i < n; i++)
            {
                double x = -1 + (2.0 / (n - 1)) * i; // Линейно распределенные значения в диапазоне [-1, 1]
                splineX[i] = x;
                splineY[i] = Function(x);
            }

            // Расчет и отображение кубического сплайна
            List<double> splinePointsX = new List<double>();
            List<double> splinePointsY = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                for (double j = 0; j <= 1; j += 0.01)
                {
                    double xi = splineX[i] + j * (splineX[i + 1] - splineX[i]);
                    double yi = InterpolatedSplineValue(splineX, splineY, xi);
                    splinePointsX.Add(xi);
                    splinePointsY.Add(yi);
                }
            }

            // Добавление кубического сплайна в график
            graphPane.AddCurve("Cubic Spline", splinePointsX.ToArray(), splinePointsY.ToArray(), System.Drawing.Color.Red, SymbolType.None);

            InitializeComponent();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }


        private void PlotFunctionAndSpline(int n)
        {
            // Создание графика
            
            GraphPane graphPane = zedGraphControl1.GraphPane;
            graphPane.Title.Text = "График функции и кубического сплайна";
            graphPane.XAxis.Title.Text = "x";
            graphPane.YAxis.Title.Text = "f(x)";

            // Данные для функции f(x) = x^3 - x
            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();
            for (double x = -1; x <= 1; x += 0.1)
            {
                xValues.Add(x);
                yValues.Add(Function(x));
            }

            // Добавление функции в график
            LineItem functionCurve = graphPane.AddCurve("f(x)", xValues.ToArray(), yValues.ToArray(), System.Drawing.Color.Blue, SymbolType.None);

            // Параметры для кубического сплайна
            double[] splineX = new double[n];
            double[] splineY = new double[n];

            // Интерполяция для создания сплайна
            for (int i = 0; i < n; i++)
            {
                double x = -1 + (2.0 / (n - 1)) * i; // Линейно распределенные значения в диапазоне [-1, 1]
                splineX[i] = x;
                splineY[i] = Function(x);
            }

            // Расчет и отображение кубического сплайна
            List<double> splinePointsX = new List<double>();
            List<double> splinePointsY = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                for (double j = 0; j <= 1; j += 0.01)
                {
                    double xi = splineX[i] + j * (splineX[i + 1] - splineX[i]);
                    double yi = InterpolatedSplineValue(splineX, splineY, xi);
                    splinePointsX.Add(xi);
                    splinePointsY.Add(yi);
                }
            }

            // Добавление кубического сплайна в график
            graphPane.AddCurve("Cubic Spline", splinePointsX.ToArray(), splinePointsY.ToArray(), System.Drawing.Color.Red, SymbolType.None);

            // Обновить график
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
        }

        // Пример функции f(x) = x^3 - x
        private double Function(double x)
        {
            return Math.Pow(x, 3) - x; // Измените на вашу функцию
        }

        // Метод для интерполяции с использованием кубического сплайна
        private double InterpolatedSplineValue(double[] x, double[] y, double xi)
        {
            // Простейшая линейная интерполяция для демонстрации, вы можете разместить
            // здесь алгоритм для кубического сплайна
            for (int i = 0; i < x.Length - 1; i++)
            {
                if (xi >= x[i] && xi <= x[i + 1])
                {
                    double t = (xi - x[i]) / (x[i + 1] - x[i]);
                    return y[i] * (1 - t) + y[i + 1] * t; // Линейная интерполяция
                }
            }
            return 0; // Если xi вне диапазона
        }
    }
}