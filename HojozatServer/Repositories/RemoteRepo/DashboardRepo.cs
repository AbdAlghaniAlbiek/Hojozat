using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Models;
using HojozatServer.Repositories.RepoOperations;
using Refit;

namespace HojozatServer.Repositories.RemoteRepo
{
    public class DashboardRepo
    {
        public static bool IsFirstConnection { get; set; } = true;
        private static Models.Dashboard Dashboard { get; set; }
        public static Models.Brand Brand { get; set; }
        public static Models.User User { get; set; }



        public async static Task<Models.Brand> GetDashboardLatestBrandAsync()
        {
            var dashboardApis =
                RestService.For<IDashboardRepoOps>(Common.URL);

            var brand =
                await dashboardApis.GetDashboardLatesBrandOpAsync(Common.Token);

            return brand;
        }
        public async static Task<ObservableCollection<Models.BrandProfile>> GetDashboardLatestBrandsProfilesAsync()
        {
            var dashboardApis =
                RestService.For<IDashboardRepoOps>(Common.URL);

            var brandsProfiles =
                await dashboardApis.GetDashboardLatesBrandsProfilesOpAsync(Brand.Id, Common.Token);

            return brandsProfiles;
        }

        public async static Task<Models.User> GetDashboardLatestUserAsync()
        {
            var dashboardApis =
                RestService.For<IDashboardRepoOps>(Common.URL);

            var user =
                await dashboardApis.GetDashboardLatesUserOpAsync(Common.Token);

            return user;
        }
        public async static Task<ObservableCollection<Models.UserProfile>> GetDashboardLatestUsersProfilesAsync()
        {
            var dashboardApis =
                RestService.For<IDashboardRepoOps>(Common.URL);

            var usersProfiles =
                await dashboardApis.GetDashboardLatesUsersProfilesOpAsync(User.Id, Common.Token);

            return usersProfiles;
        }


        public async static Task<Models.Dashboard> GetDashboardAsync()
        {
            if (Dashboard == null)
            {
                var dashboardApis =
               RestService.For<IDashboardRepoOps>(Common.URL);

                Models.Dashboard dashboard = new Models.Dashboard()
                {
                    Activity = new Models.DashboardActivity()
                    {
                        Profits = await dashboardApis.GetDashboardProfitsOpAsync(Common.Token),
                        Bookings = await dashboardApis.GetDashboardBookingsOpAsync(Common.Token)
                    },
                    AverageRegisteration = new Models.DashboardAverageRegisteration()
                    {
                        BrandsRegisteredPerMonth = await GetBrandsRegisteredPerMonthCustomized(),
                        UsersRegisteredPerMonth = await GetUsersRegisteredPerMonthCustomized()
                    },
                    TopFourBrands = await dashboardApis.GetDashboardTopFourBrandsOpAsync(Common.Token)
                };

                if (dashboard != null)
                {
                    Dashboard = dashboard;
                    return Dashboard;
                }
                return null;
            }
            return Dashboard; 
        }

        private async static Task<ObservableCollection<Models.DashboardUsersRegisteredPerMonth>> GetUsersRegisteredPerMonthCustomized()
        {
            var dashboardApis =
             RestService.For<IDashboardRepoOps>(Common.URL);

            ObservableCollection<Models.DashboardUsersRegisteredPerMonth> newUsers =
                await dashboardApis.GetDashboardNumberUsersRegisteredPerMonthOpAsync(Common.Token);

            ObservableCollection<Models.DashboardUsersRegisteredPerMonth> newUsersCustomized =
                new ObservableCollection<Models.DashboardUsersRegisteredPerMonth>();

            for (int i = 0; i <= 12; i++)
            {
                bool isMonthFound = false;
                int indexUser = 0;

                for (int j = 0; j < newUsers.Count; j++)
                {
                    if (newUsers[j].Month == i)
                    {
                        isMonthFound = true;
                        indexUser = j;
                        break;
                    }
                }
               
                if (isMonthFound)
                {
                    newUsersCustomized.Add(newUsers[indexUser]);
                }
                else
                {
                    newUsersCustomized.Add(new DashboardUsersRegisteredPerMonth() { Month = i, NewUsers = 0 });
                }
            }

            return newUsersCustomized;
        }

        private async static Task<ObservableCollection<DashboardBrandsRegisteredPerMonth>> GetBrandsRegisteredPerMonthCustomized()
        {
            var dashboardApis =
                RestService.For<IDashboardRepoOps>(Common.URL);

            ObservableCollection<Models.DashboardBrandsRegisteredPerMonth> newBrands =
                await dashboardApis.GetDashboardNumberBrandsRegisteredPerMonthOpAsync(Common.Token);

            ObservableCollection<Models.DashboardBrandsRegisteredPerMonth> newBrandsCustomized =
                new ObservableCollection<Models.DashboardBrandsRegisteredPerMonth>();

            for (int i = 0; i <= 12; i++)
            {
                bool isMonthFound = false;
                int indexBrand = 0;

                for (int j = 0; j < newBrands.Count; j++)
                {
                    if (newBrands[j].Month == i)
                    {
                        isMonthFound = true;
                        indexBrand = j;
                        break;
                    }
                } 

                if (isMonthFound)
                {
                    newBrandsCustomized.Add(newBrands[indexBrand]);
                }
                else
                {
                    newBrandsCustomized.Add(new DashboardBrandsRegisteredPerMonth() { Month = i, NewBrands = 0 });
                }
            }

            return newBrandsCustomized;
        }
    }
}
