using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Models
{
    public class UserDetails
    {
        public UserProfile UserProfile { get; set; }
        public UserActivity UserActivity { get; set; }
        public UserCommunication UserCommunication { get; set; }
    }


    public class UserProfile
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateJoin { get; set; }
        public byte Verified { get; set; }
    }

    public class UserActivity
    {
        public ObservableCollection<UserActivityYearly> UserActivityCurrentYear { get; set; }
        public ObservableCollection<UserActivityYearly> UserActivityLastYear { get; set; }
        public UserAverageActivity UserAverageActivity { get; set; }
    }

    public class UserAverageActivity
    {
        public int BookingsPerMonth { get; set; }
        public int BookingsPerYear { get; set; }
        public int TotalBookings { get; set; }
        public double PaymentsPerMonth { get; set; }
        public double PaymentsPerYear { get; set; }
        public double TotalPayments { get; set; }
    }

    public class UserActivityYearly
    {
        public byte Month { get; set; }
        public double Payments { get; set; }
        public int Bookings { get; set; }
    }

    public class UserCommunication
    {
        public UserSocial UserSocial { get; set; }
        public ObservableCollection<UserComment> UserComments { get; set; }
    }

    public class UserComment 
    {
        public string HotelName { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
    }

    public class UserSocial
    {
        public int Id { get; set; }
        public int Favourites { get; set; }
        public int History { get; set; }
        public byte OneStar { get; set; }
        public byte TwoStar { get; set; }
        public byte ThreeStar { get; set; }
        public byte FourStar { get; set; }
        public byte FiveStar { get; set; }
    }
}
