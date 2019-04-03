using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util{
    public static class StandardDeviation
    {
        public static double Calc(int[] values)
        {
            // Get the mean.
            double mean = values.Sum() / values.Length;

            // Get the sum of the squares of the differences
            // between the values and the mean.
            var squares_query =
                from int value in values
                select (value - mean) * (value - mean);
            double sum_of_squares = squares_query.Sum();

            return Math.Sqrt(sum_of_squares / values.Count());            
        }
    }
}
