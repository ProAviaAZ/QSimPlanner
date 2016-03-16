﻿using QSP.MathTools.Interpolation;

namespace QSP.MathTools.Tables
{
    public class Table2D
    {
        public double[] x { get; set; }
        public double[] y { get; set; }
        public double[][] f { get; set; }

        public Table2D(double[] x, double[] y, double[][] f)
        {
            this.x = x;
            this.y = y;            
            this.f = f;
        }

        public double ValueAt(double x, double y)
        {
            return Interpolate2D.Interpolate(this.x, this.y, x, y, f);
        }
    }
}