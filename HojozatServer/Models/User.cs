using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Models
{
    public sealed class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
