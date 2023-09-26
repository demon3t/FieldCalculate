using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfFieldCalculate.Models
{
    internal static class CompexHelper
    {
        private const double ToDegree = 180 / Math.PI;

        public static double GetDegree(this Complex current)
        {
            return current.Phase * ToDegree;
        }

        public static double GetRotatedDegree(this Complex current, bool clockwise)
        {
            return clockwise ? current.GetDegree() + 90 : current.GetDegree() - 90;
        }
    }
}
