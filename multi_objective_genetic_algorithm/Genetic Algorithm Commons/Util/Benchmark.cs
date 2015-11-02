using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetic_Algorithm_Commons.Util {
    public sealed class Benchmark : IEnumerable<long> {
        private readonly Action subject;
        private Benchmark(Action subject) { this.subject = subject; }

        public static Benchmark This(Action subject) {
            return new Benchmark(subject);
        }

        public IEnumerator<long> GetEnumerator() {
            var watch = new Stopwatch();
            while (true) {
                watch.Reset();
                watch.Start();
                subject();
                watch.Stop();
                yield return watch.ElapsedMilliseconds;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
