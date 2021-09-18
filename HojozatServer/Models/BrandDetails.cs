using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Models
{
    public class BrandDetails
    {
        public BrandProfile BrandProfile { get; set; }
        public BrandActivity BrandActivity { get; set; }
        public UsersFeedback UsersFeedback { get; set; }
        public ObservableCollection<BrandHotels> BrandHotels { get; set; }
    }

    public class BrandProfile
    {
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        public string Slogan { get; set; }
        public string Description { get; set; }
        public DateTime DateJoin { get; set; }
        public byte Verified { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int HotelsNumber { get; set; }
    }

    public class BrandActivity
    {
        public BrandAveragesActivity BrandAveragesActivity { get; set; }
        public ObservableCollection<BrandActivityYearly> BrandActivityCurrentYear { get; set; }
        public ObservableCollection<BrandActivityYearly> BrandActivityLastYear { get; set; }
    }

    public class BrandAveragesActivity
    {
        public double BookingsPerMonth { get; set; }
        public double BookingsPerYear { get; set; }
        public int TotalBookings { get; set; }
        public decimal PaymentsPerMonth { get; set; }
        public decimal PaymentsPerYear { get; set; }
        public decimal TotalPayments { get; set; }

    }

    public class BrandActivityYearly
    {
        public byte Month { get; set; }
        public int Bookings { get; set; }
        public double Payments { get; set; }
    }


    public class UsersFeedback
    {
        public ObservableCollection<UsersRating> UsersRatings { get; set; }
        public UsersFavouritesHistory UsersFavouritesHistory { get; set; }
        public ObservableCollection<HotelUsersComments> HotelUsersComments { get; set; }
    }

    public class UsersFavouritesHistory
    {
        public int Favourites { get; set; }
        public int History { get; set; }
    }

    public class UsersRating
    {
        public int Star { get; set; }
        public int RatingValue { get; set; }
    }

    public class UsersComment
    {
        public string HotelName { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }

    public class HotelUsersComments
    {
        public string HotelName { get; set; }
        public ObservableCollection<UsersComment> UsersComments { get; set; }
    }

    public class BrandHotels
    {
        public string HotelName { get; set; }
        public double Stars { get; set; }
        public string ImagePath { get; set; }
        public string CityName { get; set; }
        public int Rooms { get; set; }
    }
}
