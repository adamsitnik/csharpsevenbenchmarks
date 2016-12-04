using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{

    /// <summary>
    /// disclaimer: this benchmark is based on problem reported and described by Jon Skeet
    /// link: https://codeblog.jonskeet.uk/2014/07/16/micro-optimization-the-surprising-inefficiency-of-readonly-fields/
    /// 
    /// int result = someField.Foo();
    /// 
    /// is converted to following code when someField is readonly:
    /// 
    /// var tmp = someField; 
    /// int result = tmp.Foo();
    /// 
    /// summary: C# 7.0 does not help because you can't get a reference to readonly field...
    /// </summary>
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
    [RPlotExporter]
    [CsvMeasurementsExporter]
    public class ReadOnlyFields
    {
        public struct Int256
        {
            private readonly long bits0;
            private readonly long bits1;
            private readonly long bits2;
            private readonly long bits3;

            public Int256(long bits0, long bits1, long bits2, long bits3)
            {
                this.bits0 = bits0;
                this.bits1 = bits1;
                this.bits2 = bits2;
                this.bits3 = bits3;
            }

            public long Bits0 { get { return bits0; } }
            public long Bits1 { get { return bits1; } }
            public long Bits2 { get { return bits2; } }
            public long Bits3 { get { return bits3; } }
        }

        private readonly Int256 readOnlyField = new Int256(1, 2, 3, 4);
        private Int256 mutableField = new Int256(1, 2, 3, 4);

        [Benchmark]
        public long ReadOnlyField()
        {
            return readOnlyField.Bits0 + readOnlyField.Bits1 + readOnlyField.Bits2 + readOnlyField.Bits3;
        }

        [Benchmark]
        public long MutableField()
        {
            return mutableField.Bits0 + mutableField.Bits1 + mutableField.Bits2 + mutableField.Bits3;
        }

        [Benchmark(Baseline = true)]
        public long ReferenceToMutableField()
        {
            ref Int256 reference = ref mutableField;
            return reference.Bits0 + reference.Bits1 + reference.Bits2 + reference.Bits3;
        }
    }
}
