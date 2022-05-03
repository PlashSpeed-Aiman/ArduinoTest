using System;
using System.Collections.Generic;
using System.Text;

namespace ArduinoTest
{
    internal  class StaticsMethods
    {
        public static void SeparatePIN(String x, List<Double> y)
        {
            if(Double.TryParse(x,out double value)){
                y.Add(value);
            }
        } 
    }
}
