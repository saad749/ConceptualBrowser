using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Performance
{
    public class PerformanceMonitor
    {
        public Stopwatch Stopwatch { get; set; }
        public List<Tuple<string, long>> Checkpoints { get; set; }

        public PerformanceMonitor()
        {
            Checkpoints = new List<Tuple<string, long>>();
            Stopwatch = new Stopwatch();
        }

        public void Start()
        {
            Stopwatch.Start();
            Checkpoints.Add(new Tuple<string, long>("Start", Stopwatch.ElapsedMilliseconds));
        }

        public void Stop() 
        { 
            Stopwatch.Stop();
            Checkpoints.Add(new Tuple<string, long>("Stop", Stopwatch.ElapsedMilliseconds));
        }

        public void WriteLog()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tuple in Checkpoints)
            {
                sb.Append(tuple.Item1);
                sb.Append("\t");
                sb.Append(tuple.Item2);
                sb.AppendLine().AppendLine();
            }
            File.AppendAllText("Output\\PerformanceLog.txt", sb.ToString());
        }
    }
}
