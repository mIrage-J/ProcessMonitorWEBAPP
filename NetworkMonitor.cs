using System.Collections.Generic;
using System.Diagnostics;
namespace NewWebAPP201910
{
    public class NetworkMonitor
    {
        private static List<PerformanceCounter> counters;
        


        public static float NetworkReceived()
        {
            
            float value=0;
            foreach (var counter in counters)
            {
                value+=counter.CounterName=="Bytes Received/sec"?counter.NextValue():0f;                
            }
            return value;
        }

        public static float NetworkSent()
        {
            
            float value=0;
            foreach (var counter in counters)
            {
                value+=counter.CounterName=="Bytes Sent/sec"?counter.NextValue():0f;                
            }
            return value;
        }
        
        public static void PerformanceControllerInitialise()
        {
            if (counters != null)
                return;
            else
            {
                counters=new List<PerformanceCounter>();
                foreach (var categ in PerformanceCounterCategory.GetCategories())
                {
                    if (categ.CategoryName == "Network Interface")
                    {

                        foreach (var ins in categ.GetInstanceNames())
                        {
                           counters.AddRange(categ.GetCounters(ins));
                        }
                    }
                }
            }
        }
    }
}