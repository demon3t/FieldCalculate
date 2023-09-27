using WpfFieldCalculate.Infrastructure;
using System.Windows;
using System.Numerics;
using System;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Annotations;
using HandyControl.Controls;

namespace WpfFieldCalculate.Models
{
    /// <summary>
    /// Входные данные.
    /// </summary>
    internal class WireData : ViewModel
    {
        public WireData(double i, Point point)
        {
            Circle.Points.Add(new DataPoint(X, Y));
            Mark.Points.Add(new DataPoint(X, Y));
            Vector.Points.Add(new DataPoint(X, Y));
            Vector.Points.Add(new DataPoint(0, 0));

            I = i;
            X = point.X;
            Y = point.Y;
            InductionAbs = this.GetInduction();
            InductionDeg = this.GetRotatedDegree();
        }

        public Complex ToComplex =>
            new(InductionAbs * Math.Cos(InductionDeg * Calculate.ToRad), InductionAbs * Math.Sin(InductionDeg * Calculate.ToRad));

        #region Для графика

        public LineSeries Circle = new LineSeries()
        {
            MarkerType = MarkerType.Circle,
            LineStyle = LineStyle.Solid,
            MarkerSize = 10,
            MarkerFill = OxyColors.White,
            MarkerStrokeThickness = 2,
            RenderInLegend = false,
            TrackerFormatString = "X: {2:F0} см,\nY: {4:F0} см",
            LabelMargin = 15,
        };

        public LineSeries Mark = new LineSeries()
        {
            LineStyle = LineStyle.Solid,
            MarkerSize = 3,
            MarkerStrokeThickness = 2,
            TrackerFormatString = "X: {2:F0} см,\nY: {4:F0} см",
            RenderInLegend = false,
        };

        public LineSeries Vector = new LineSeries()
        {
            MarkerType = MarkerType.None,
            LineStyle = LineStyle.LongDash,
            StrokeThickness = 1.5,
            TrackerFormatString = "X: {2:F0} см,\nY: {4:F0} см",
            RenderInLegend = false,
        };

        public ArrowAnnotation Arrow = new ArrowAnnotation()
        {
            StartPoint = new DataPoint(0, 0),
            Color = OxyColors.Black,
            StrokeThickness = 1,
            HeadLength = 7,
            HeadWidth = 2
        };

        #endregion

        #region Свойства

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                Circle.LabelFormatString = value;
                Set(ref _name, value);
            }
        }

        public string _name;

        /// <summary>
        /// Ток в линии
        /// </summary>
        public double I
        {
            get { return _i; }
            set
            {
                Set(ref _i, value);
                InductionAbs = this.GetInduction();
                InductionDeg = this.GetRotatedDegree();
            }
        }

        public double _i;

        /// <summary>
        /// Координата по горизонтали.
        /// </summary>
        public double X
        {
            get { return _x; }
            set
            {
                Set(ref _x, value);
                InductionAbs = this.GetInduction();
                InductionDeg = this.GetRotatedDegree();
            }
        }

        public double _x;

        /// <summary>
        /// Координата по вертикали.
        /// </summary>
        public double Y
        {
            get { return _y; }
            set
            {
                Set(ref _y, value);
                InductionAbs = this.GetInduction();
                InductionDeg = this.GetRotatedDegree();
            }
        }

        public double _y;

        /// <summary>
        /// Модуль индукции.
        /// </summary>
        public double InductionAbs
        {
            get { return _inductionAbs; }
            set { Set(ref _inductionAbs, value); }
        }

        private double _inductionAbs;

        /// <summary>
        /// Угол индукции.
        /// </summary>
        public double InductionDeg
        {
            get { return _inductionDeg; }
            set { Set(ref _inductionDeg, value); }
        }

        private double _inductionDeg;

        #endregion

    }
}
