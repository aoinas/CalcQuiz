using System;
using System.Collections.Generic;
using System.Text;

using GalaSoft.MvvmLight;

namespace CalculationQuiz
{
    abstract class Calculation : ViewModelBase
    {
        protected Calculation(params Decimal[] values)
        {
            Values.AddRange(values);
        }

        public List<Decimal> Values { get; set; } = new List<Decimal>();

        public String AsText { get { return String.Join( String.Format(" {0} ", Separator), Values) + " = "; } }

        public abstract Decimal CalculatedResult();
        protected abstract String Separator { get; }

        public bool IsSame(Calculation c)
        {
            return c != null && Values.Equals(c.Values);
        }
        
    }
}
