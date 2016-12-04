using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using System;

namespace Benchmarks
{
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
    [RPlotExporter]
    [CsvMeasurementsExporter]
    public class ReadingBigStructs
    {
        public struct BigStruct
        {
            public BigStruct(long long1, long long2, long long3, long long4)
            {
                Long1 = long1;
                Long2 = long2;
                Long3 = long3;
                Long4 = long4;
            }

            public long Long1 { get; }
            public long Long2 { get; }
            public long Long3 { get; }
            public long Long4 { get; }
        }

        private static readonly Random random = new Random(12345);

        private BigStruct[] array;

        [Setup]
        public void Setup()
        {
            array = new BigStruct[100];
            for (int i = 0; i < 100; i++)
                array[i] = new BigStruct(random.Next(), random.Next(), random.Next(), random.Next());
        }

        [Benchmark]
        public long NormalLoop()
        {
            long sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i].Long1;
                sum += array[i].Long2;
                sum += array[i].Long3;
                sum += array[i].Long4;
            }
            return sum;
        }

        [Benchmark]
        public long LoopWithRef()
        {
            long sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                ref BigStruct reference = ref array[i];
                sum += reference.Long1;
                sum += reference.Long2;
                sum += reference.Long3;
                sum += reference.Long4;
            }
            return sum;
        }
    }
}
