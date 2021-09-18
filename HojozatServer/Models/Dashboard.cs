using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Models
{
    public class Dashboard
    {
        public DashboardActivity Activity { get; set; }
        public DashboardAverageRegisteration AverageRegisteration { get; set; }
        public ObservableCollection<DashboardTopFourBrands> TopFourBrands { get; set; }
    }

    public class DashboardActivity
    {
        public DashboardProfits Profits { get; set; }
        public DashboardBookings Bookings { get; set; }
    }

    public class DashboardProfits
    {
        public ObservableCollection<DashboardProfitsPerMonth> profitsPerMonth { get; set; }
        public double TotalProfits { get; set; }
    }

    public class DashboardProfitsPerMonth
    {
        public int Month { get; set; }
        public double Profits { get; set; }
    }

    public class DashboardBookings
    {
        public ObservableCollection<DashboardBookingsPerMonth> BookingsPerMonth { get; set; }
        public int TotalBookings { get; set; }
    }

    public class DashboardBookingsPerMonth
    {
        public int Month { get; set; }
        public int Bookings { get; set; }
    }

    public class DashboardAverageRegisteration
    {
        public ObservableCollection<DashboardBrandsRegisteredPerMonth> BrandsRegisteredPerMonth { get; set; }
        public ObservableCollection<DashboardUsersRegisteredPerMonth> UsersRegisteredPerMonth { get; set; }
    }

    public class DashboardBrandsRegisteredPerMonth
    {
        public int Month { get; set; }
        public int NewBrands { get; set; }
    }

    public class DashboardUsersRegisteredPerMonth
    {
        public int Month { get; set; }
        public int NewUsers { get; set; }
    }

    public class DashboardTopFourBrands
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double Payments { get; set; }
        public int Bookings { get; set; }
    }
}
