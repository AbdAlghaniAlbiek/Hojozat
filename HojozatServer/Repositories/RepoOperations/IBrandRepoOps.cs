using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface IBrandRepoOps
    {
        [Get("/getBrandsNames.php")]
        Task<ObservableCollection<string>> GetBrandsNamesOpAsync([AliasAs("Brand_Name_Prefix")] string brandNamePrefix, [Header("Authorization")] string authorization);

        [Get("/getBrandIdByName.php")]
        Task<int> GetBrandIdByNameOpAsync([AliasAs("Brand_Name")] string brandName, [Header("Authorization")] string authorization);

        [Get("/getBrands.php")]
        Task<ObservableCollection<Models.Brand>> GetBrandsOpAsync([Header("Authorization")] string authorization);

        [Get("/getBrandProfile.php")]
        Task<Models.BrandProfile> GetBrandProfileOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandAveragesActivity.php")]
        Task<Models.BrandAveragesActivity> GetBrandAveragesActivityOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandActivityCurrentYear.php")]
        Task<ObservableCollection<Models.BrandActivityYearly>> GetBrandActivityCurrentYearOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandActivityLastYear.php")]
        Task<ObservableCollection<Models.BrandActivityYearly>> GetBrandActivityLastYearOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandFavouritesHistory.php")]
        Task<Models.UsersFavouritesHistory> GetUsersFavouritesHistoryOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandRatings.php")]
        Task<ObservableCollection<int>> GetUsersRatingsOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandHotels.php")]
        Task<ObservableCollection<Models.BrandHotels>> GetBrandHotelsOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);

        [Get("/getBrandComments.php")]
        Task<ObservableCollection<Models.UsersComment>> GetUsersCommentsOpAsync([AliasAs("Brand_Id")] int brandId, [Header("Authorization")] string authorization);
    }
}
