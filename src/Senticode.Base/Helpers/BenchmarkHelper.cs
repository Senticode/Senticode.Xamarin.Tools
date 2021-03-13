using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Senticode.Base.Helpers
{
    public class BenchmarkHelper
    {
        private DateTime _lastTime;

        private BenchmarkHelper(string ticket, DateTime start)
        {
            Ticket = ticket;
            StartTime = start;
            _lastTime = start;
        }

        public string Ticket { get; }
        public DateTime StartTime { get; }

        public string ClassName { get; private set; } = string.Empty;

        public static BenchmarkHelper Start([CallerMemberName] string ticket = null)
        {
            var result = new BenchmarkHelper(ticket, DateTime.UtcNow);
            return result;
        }

        public static BenchmarkHelper Start<T>([CallerMemberName] string ticket = null) where T : class
        {
            var result = new BenchmarkHelper(ticket, DateTime.UtcNow)
            {
                ClassName = typeof(T).Name
            };
            return result;
        }

        public TimeSpan Stop()
        {
            var result = DateTime.UtcNow - StartTime;
            Debug.WriteLine($"[Benchmark] {ClassName}.{Ticket}: {result}");
            return result;
        }


        public TimeSpan Calculate([CallerMemberName] string subtitle = null)
        {
            var result = DateTime.UtcNow - _lastTime;
            _lastTime = DateTime.UtcNow;
            Debug.WriteLine($"[Benchmark] {ClassName}.{Ticket} - {subtitle}: {result}");
            return result;
        }
    }
}