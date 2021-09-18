using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface IDashboardRepoOps
    {
        [Get("/getDashboardBookings.php")]
        Task<Models.DashboardBookings> GetDashboardBookingsOpAsync([Header("Authorization")] string authorization);
        
        [Get("/getDashboardNumberBrandsRegisteredPerMonth.php")]
        Task<ObservableCollection<Models.DashboardBrandsRegisteredPerMonth>> GetDashboardNumberBrandsRegisteredPerMonthOpAsync([Header("Authorization")] string authorization);

        [Get("/getDashboardNumberUsersRegisteredPerMonth.php")]
        Task<ObservableCollection<Models.DashboardUsersRegisteredPerMonth>> GetDashboardNumberUsersRegisteredPerMonthOpAsync([Header("Authorization")] string authorization);

        [Get("/getDashboardProfits.php")]
        Task<Models.DashboardProfits> GetDashboardProfitsOpAsync([Header("Authorization")] string authorization); 

        [Get("/getDashboardTopFourBrands.php")]
        Task<ObservableCollection<Models.DashboardTopFourBrands>> GetDashboardTopFourBrandsOpAsync([Header("Authorization")] string authorization);

        [Get("/getDashboardLatestBrand.php")]
        Task<Models.Brand> GetDashboardLatesBrandOpAsync([Header("Authorization")] string authorization);

        [Get("/getDashboardLatestBrandsProfiles.php")]
        Task<ObservableCollection<Models.BrandProfile>> GetDashboardLatesBrandsProfilesOpAsync([AliasAs("Old_Brand_Id")] int oldBrandId, [Header("Authorization")] string authorization);

        [Get("/getDashboardLatestUser.php")]
        Task<Models.User> GetDashboardLatesUserOpAsync([Header("Authorization")] string authorization);

        [Get("/getDashboardLatestUsersProfiles.php")]
        Task<ObservableCollection<Models.UserProfile>> GetDashboardLatesUsersProfilesOpAsync([AliasAs("Old_User_Id")] int oldBrandId, [Header("Authorization")] string authorization);
    }
}
