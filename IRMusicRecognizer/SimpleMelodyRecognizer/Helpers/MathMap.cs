using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMelodyRecognizer.Helpers
{
    static class MathFuncs
    {
        public static double Map(double num, double fromBottom, double fromTop, double toBottom, double toTop)
        {
            double a = num - fromBottom;
            a *= (toTop - toBottom) / (fromTop - fromBottom);
            a += toBottom;
            return a;
        }
        public static double Round(double num, int digits)
        {
            var pow = Math.Pow(10, digits);
            return Math.Round(num * pow) / pow;
        }
    }
}
