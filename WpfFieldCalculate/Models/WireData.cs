using WpfFieldCalculate.Infrastructure;
using System.Windows;
using System.Numerics;
using System.Threading;
using System;

namespace WpfFieldCalculate.Models
{
    /// <summary>
    /// Входные данные.
    /// </summary>
    internal class WireData : ViewModel
    {
        public WireData(double i, Point point)
        {
            I = i;
            Coordinate = point;
        }

        public Complex ToCompex => 
            new (InductionAbs * Math.Cos(InductionDeg * Calculate.ToRad), InductionAbs * Math.Sin(InductionDeg * Calculate.ToRad));

        /// <summary>
        /// Название
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public string _name;

        /// <summary>
        /// Ток в линии
        /// </summary>
        public double I
        {
            get { return _i;}
            set { Set(ref _i, value); }
        }

        public double _i;

        /// <summary>
        /// Координата по горизонтали.
        /// </summary>
        public double X
        {
            get { return Coordinate.X; }
            set
            {
                Coordinate.X = value;
                Set(ref _x, value);
            }
        }

        public double _x;

        /// <summary>
        /// Координата по вертикали.
        /// </summary>
        public double Y
        {
            get { return Coordinate.Y; }
            set
            {
                Coordinate.Y = value;
                Set(ref _y, value);
            }
        }

        public double _y;

        /// <summary>
        /// Расположение провода.
        /// </summary>
        internal Point Coordinate;

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
    }
}
