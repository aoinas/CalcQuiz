using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationQuiz
{
    class MultiplicationCalculation : Calculation
    {
        public MultiplicationCalculation(params Decimal[] values) : base(values)
        {
        }

        protected override string Separator { get { return "·"; } }

        public override Decimal CalculatedResult()
        {
            Decimal result = 1.0M;

            foreach (Decimal value in Values)
                result *= value;

            return Math.Round(result, 0);
        }
    }
}
