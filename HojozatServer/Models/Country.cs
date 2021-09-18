using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Refit;
using Newtonsoft.Json;

namespace HojozatServer.Models
{
    public class Country
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

    }
}
