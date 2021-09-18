using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;

namespace HojozatServer.Models
{
    public class HotelDetails
    {
        public HotelProfile HotelProfile { get; set; }
        public HotelRooms HotelRooms { get; set; }
        public ObservableCollection<string> HotelPhotos { get; set; }
        public ObservableCollection<string> HotelRules { get; set; }
        public ObservableCollection<HotelFacility> HotelFacilities { get; set; }
        public HotelActivity HotelActivity { get; set; }
        public HotelUsersFeedback HotelUsersFeedback { get; set; }
    }

    public class HotelProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public decimal Stars { get; set; }
        public string CityName { get; set; }
        public int CheckIn { get; set; }
        public int CheckOut { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Type { get; set; }
        public string BrandName { get; set; }
        public string HostType { get; set; }
        public int Rooms { get; set; }
    }

    public class HotelFacility
    {
        public string Facility { get; set; }
        public int Count { get; set; }
    }

    public class HotelRooms
    {
        public ObservableCollection<string> HotelRoomsTypes { get; set; }
        public ObservableCollection<HotelRoomsCountByType> HotelRoomsCountByType { get; set; }
    }

    public class HotelRoomsTypeDetails
    {
        public string Type { get; set; }
        public int Visitors { get; set; }
        public int Space { get; set; }
        public double Price { get; set; }
        public string Features { get; set; }
        public ObservableCollection<RoomFacility> Facilities { get; set; }
        public ObservableCollection<string> Images { get; set; }
    }

    public class RoomFacility
    {
        public string Facility { get; set; }
        public byte Count { get; set; }
    }

    public class HotelRoomsCountByType
    {
        public string Type { get; set; }
        public int Rooms { get; set; }
    }

    public class HotelActivity
    {
        public ObservableCollection<HotelActivityYearly> HotelActivityCurrentYear { get; set; }
        public ObservableCollection<HotelActivityYearly> HotelActivityLastYear { get; set; }
        public HotelAveragesActivity HotelAveragesActivity { get; set; }
    }

    public class HotelActivityYearly
    {
        public byte Month { get; set; }
        public int Bookings { get; set; }
        public double Payments { get; set; }
    }

    public class HotelAveragesActivity
    {
        public int BookingsPerMonth { get; set; }
        public int BookingsPerYear { get; set; }
        public int TotalBookings { get; set; }
        public double PaymentsPerMonth { get; set; }
        public double PaymentsPerYear { get; set; }
        public double TotalPayments { get; set; }
    }

    public class HotelUsersFeedback
    {
        public ObservableCollection<HotelUserComment> HotelUsersComments { get; set; }
        public HotelUsersFavouritesHistory HotelFavouritesHistory { get; set; }
        public ObservableCollection<HotelUsersRating> HotelUsersRatings { get; set; }
    }

    public class HotelUserComment
    {
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }

    public class HotelUsersFavouritesHistory
    {
        public int Favourites { get; set; }
        public int History { get; set; }
    }

    public class HotelUsersRating
    {
        public int Star { get; set; }
        public int value { get; set; }
    }
}
