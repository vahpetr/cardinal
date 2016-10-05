using System;

namespace Cardinal.Trainings
{
    public class CopmutedError
    {
        public double Value { get; set; } = double.MaxValue;
        public double PrevValue { get; set; }
        public double Delta => Math.Abs(PrevValue - Value);
        public double Accuracy => 1 - Value;

        /// <summary>
        /// Безконечно ли значение
        /// </summary>
        /// <returns>Безконечно?</returns>
        public bool IsInfinite()
        {
            return double.IsNaN(Value) || double.IsInfinity(Value);
        }
    }
}