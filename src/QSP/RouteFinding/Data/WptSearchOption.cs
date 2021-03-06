﻿namespace QSP.RouteFinding.Data
{
    public class WptSearchOption
    {
        public double SearchRangeIncr { get; private set; }
        public double MaxSearchRange { get; private set; }
        public int TargetCount { get; private set; }

        public WptSearchOption(
           double SearchRangeIncr = 20.0,
           double MaxSearchRange = 500.0,
           int TargetCount = 30)
        {
            this.SearchRangeIncr = SearchRangeIncr;
            this.MaxSearchRange = MaxSearchRange;
            this.TargetCount = TargetCount;
        }        
    }
}
