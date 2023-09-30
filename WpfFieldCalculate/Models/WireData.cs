using WpfFieldCalculate.Infrastructure;
using System.Windows;
using System.Numerics;
using System;
using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Annotations;
using HandyControl.Controls;
using System.Windows.Input;

namespace WpfFieldCalculate.Models
{
    /// <summary>
    /// Входные данные.
    /// </summary>
    internal class WireData : ViewModel
    {
        private readonly InputData _inputData;

        private readonly PlotModel _plotModel;

        public WireData(PlotModel plotModel, InputData inputData)
        {
            _inputData = inputData;
            _plotModel = plotModel;

            Circle.Points.Add(new DataPoint(X, Y));
            Mark.Points.Add(new DataPoint(X, Y));
            Vector.Points.AddRange(new DataPoint[2] { new DataPoint(X, Y), new DataPoint(0, 0) });


            plotModel.Series.Add(Vector);
            plotModel.Series.Add(Circle);
            plotModel.Series.Add(Mark);
            plotModel.Annotations.Add(Arrow);

            InductionAbs = this.GetInduction();
            InductionDeg = this.GetRotatedDegree();
        }

        public void OnSelecteted()
        {
            Circle.Color = OxyColors.Blue;
            Circle.MarkerStroke = OxyColors.Blue;
            Circle.TextColor = OxyColors.Blue;
            Mark.Color = OxyColors.Blue;
            Mark.MarkerStroke = OxyColors.Blue;
            Mark.TextColor = OxyColors.Blue;
            Mark.MarkerFill = OxyColors.Blue;
            Vector.Color = OxyColors.Blue;
            Arrow.Color = OxyColors.Blue;

            _plotModel.InvalidatePlot(true);
        }

        public void OnUnselecteted()
        {
            _inputData.UpdateStyle(this);
            Arrow.Color = OxyColors.Black;
            _plotModel.InvalidatePlot(true);
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
            TrackerFormatString = "Провод\nX: {2:F0} см,\nY: {4:F0} см",
            LabelMargin = 15,
        };

        public LineSeries Mark = new LineSeries()
        {
            LineStyle = LineStyle.Solid,
            MarkerSize = 3,
            MarkerStrokeThickness = 2,
            TrackerFormatString = "Провод\nX: {2:F0} см,\nY: {4:F0} см",
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
            StrokeThickness = 1.5,
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
