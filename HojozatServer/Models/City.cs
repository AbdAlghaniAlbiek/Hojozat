using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Models
{
    public class City
    {
        public string Name { get; set; }
        public int TotalHotelsNumber { get; set; }
        public int NormalHotelsNumber { get; set; }
        public int MediumHotelsNumber { get; set; }
        public int SuperHotelsNumber { get; set; }
    }
}
