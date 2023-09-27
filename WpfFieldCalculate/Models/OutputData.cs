using HandyControl.Controls;
using OxyPlot;
using OxyPlot.Annotations;
using System;
using System.Numerics;
using WpfFieldCalculate.Infrastructure;

namespace WpfFieldCalculate.Models
{
    /// <summary>
    /// Расчётные данные.
    /// </summary>
    internal class OutputData : ViewModel
    {
        private readonly InputData _inputData;
        
        public OutputData(InputData inputData)
        {
            _inputData = inputData;

        }

        public ArrowAnnotation Arrow = new ArrowAnnotation
        {
            StartPoint = new DataPoint(0, 0),
            EndPoint = new DataPoint(0, 0),
            Color = OxyColors.Green,
            StrokeThickness = 1.5,
            HeadLength = 7,
            HeadWidth = 2
        };

        public void Caculate()
        {
            InductionAbsSum = Calculate.GetSummaryInduction(_inputData.Wires);
            InductionDegSum = Calculate.GetSummaryInductionDeg(_inputData.Wires);
        }

        public void UpdateCoordinate()
        {
            Arrow.EndPoint = new DataPoint(ToComplex.Real * 100000000, ToComplex.Imaginary * 100000000);
        }

        public void UpdateStyle()
        {
            Arrow.Color = InductionDegSum == 0 ? OxyColors.Transparent : OxyColors.Green;
        }

        /// <summary>
        /// Модуль индукции.
        /// </summary>
        public double InductionAbsSum
        {
            get { return _inductionAbs; }
            set
            {
                Set(ref _inductionAbs, value);
            }
        }

        private double _inductionAbs;

        /// <summary>
        /// Угол индукции.
        /// </summary>
        public double InductionDegSum
        {
            get { return _inductionDeg; }
            set
            {
                Set(ref _inductionDeg, value);
            }
        }

        private double _inductionDeg;

        public Complex ToComplex =>
            new(InductionAbsSum * Math.Cos(InductionDegSum * Calculate.ToRad), InductionAbsSum * Math.Sin(InductionDegSum * Calculate.ToRad));
    }
}
