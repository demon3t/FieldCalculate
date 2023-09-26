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

            foreach (var w in _inputData.Wires)
            {
                w.PropertyChanged += (sender, e) =>
                {
                    Update();
                };
            }

            Update();
        }

        /// <summary>
        /// Рассчитать данные.
        /// </summary>
        private void Update()
        {
            foreach (var w in _inputData.Wires )
            {
                w.InductionAbs = w.GetInduction();
                w.InductionDeg = w.GetRotatedDegree();
            }

            InductionAbsSum = Calculate.GetSummaryInduction(_inputData.Wires);
            InductionDegSum = Calculate.GetSummaryInductionDeg(_inputData.Wires);
        }

        /// <summary>
        /// Модуль индукции.
        /// </summary>
        public double InductionAbsSum
        {
            get { return _inductionAbs; }
            set { Set(ref _inductionAbs, value); }
        }

        private double _inductionAbs;

        /// <summary>
        /// Угол индукции.
        /// </summary>
        public double InductionDegSum
        {
            get { return _inductionDeg; }
            set { Set(ref _inductionDeg, value); }
        }

        private double _inductionDeg;

        public Complex ToComplex =>
            new(InductionAbsSum * Math.Cos(InductionDegSum * Calculate.ToRad), InductionAbsSum * Math.Sin(InductionDegSum * Calculate.ToRad));
    }
}
