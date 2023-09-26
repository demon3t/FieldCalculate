using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WpfFieldCalculate.Models
{
    internal static class Calculate
    {
        public const double μ0 = 0.0000012566370614359173;

        public const double ToDegree = 180 / Math.PI;

        public const double ToRad = Math.PI / 180;

        public static double GetDistance(this WireData wire)
        {
            return Math.Sqrt(Math.Pow(wire.Coordinate.X, 2) + Math.Pow(wire.Coordinate.Y, 2));
        }

        public static double GetDegree(this WireData wire)
        {
            return Math.Atan(wire.Coordinate.Y / wire.Coordinate.X) * ToDegree;
        }

        public static double GetRotatedDegree(this WireData wire)
        {
            return wire.I == 0 ? GetDegree(wire) :
                wire.I > 0 ? GetDegree(wire) - 90 : GetDegree(wire) + 90;
        }

        public static double GetInduction(this WireData wire)
        {
            return (μ0 * wire.I) / (2 * Math.PI * wire.GetDistance());
        }

        public static double GetSummaryInduction(IEnumerable<WireData> wires)
        {
            var result = new Complex(0, 0);

            foreach (var w in wires)
            {
                result += w.ToCompex;
            }

            return result.Magnitude;
        }

        public static double GetSummaryInductionDeg(IEnumerable<WireData> wires)
        {
            var result = new Complex(0, 0);

            foreach (var w in wires)
            {
                result += w.ToCompex;
            }

            return result.Phase * ToDegree;
        }
    }
}
