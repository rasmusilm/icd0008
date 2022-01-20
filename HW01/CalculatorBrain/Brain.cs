using System;

namespace CalculatorBrain
{
    public class Brain
    {
        public double CurrentValue { get; private set; }

        public Brain()
        {
            
        }

        public double Add(double value)
        {
            CurrentValue += value;
            return CurrentValue;
        }

        public void Clear()
        {
            CurrentValue = 0;
        }

        public double Apply(Func<double, double, double> toApply, double value)
        {
            CurrentValue = Convert.ToDouble(toApply(CurrentValue, value));
            return CurrentValue;
        }
    }
}