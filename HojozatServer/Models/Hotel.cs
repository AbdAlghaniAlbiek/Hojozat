using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Stars { get; set; }
        public string CityName { get; set; }
        public int Rooms { get; set; }
        public int Favourites { get; set; }
        public string ImagePath { get; set; }
        public string BrandName { get; set; }
    }

    public class HotelsCollectedByBrand
    {
        public string BrandName { get; set; }
        public ObservableCollection<Hotel> Hotels { get; set; }
    }
}
