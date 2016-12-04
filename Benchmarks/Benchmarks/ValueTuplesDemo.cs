using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Benchmarks
{
    class ValueTuplesDemo
    {
        (double min, double max, double avg, double sum) GetStats(double[] numbers)
        {
            double min = double.MaxValue, max = double.MinValue, sum = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > max) max = numbers[i];
                if (numbers[i] < min) min = numbers[i];
                sum += numbers[i];
            }
            double avg = numbers.Length != 0 ? sum / numbers.Length : double.NaN;

            return (min, max, avg, sum);
        }

        ref int Max(ref int first, ref int second, ref int third)
        {
            ref int max = ref first;

            if (first < second) max = second;
            if (second < third) max = third;

            return ref max;
        }
    }
}
