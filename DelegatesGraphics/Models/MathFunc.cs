using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatesGraphics.Models {
    class MathFunc {
        #region Fields
        private Func<double, double> function;
        private List<Point> points;
        private Point min;
        private Point max;
        #endregion

        #region Properties
        public List<Point> Points { get => points; }
        public Func<double, double> Function { get => function; }
        public Point Min { get => min; }
        public Point Max { get => max; }
        #endregion

        public MathFunc(Func<double, double> function) {
            this.function = function;
            this.max = new Point() { X = 0, Y = 0 };
            this.min = new Point() { X = 0, Y = 0 };
        }

        public List<Point> Tabulate(double start, double stop, double step = 0.5) {
            points = new List<Point>();
            for (double i = start; i <= stop; i += step) {
                Point point = new Point() { X = i, Y = function(i) };
                if (point.Y < min.Y) min = new Point() { X = point.X, Y = point.Y };
                if (point.Y > max.Y) max = new Point() { X = point.X, Y = point.Y };
                points.Add(point);
            }
            return points;
        }
    }
}
