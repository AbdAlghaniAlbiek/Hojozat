using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Repositories.RemoteRepo;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface IUsersRepoOps
    { 
        [Get("/getUserIdByName.php")]
        Task<int> GetUserIdByNameOpAsync([AliasAs("User_Name")] string userName, [Header("Authorization")] string authorization);

        [Get("/getUsersNames.php")]
        Task<ObservableCollection<string>> GetUsersSearchNamesOpAsync([AliasAs("User_Name_Prefix")] string userNamePrefix, [Header("Authorization")] string authorization);

        [Get("/getUsers.php")]
        Task<ObservableCollection<Models.User>> GetUsersOpAsync([Header("Authorization")] string authorization);

        [Get("/getUserProfile.php")]
        Task<Models.UserProfile> GetUserProfileOpAsync([AliasAs("User_Id")] int userId, [Header("Authorization")] string authorization);

        [Get("/getUserActivityCurrentYear.php")]
        Task<ObservableCollection<Models.UserActivityYearly>> GetUserActivityCurrentYearOpAsync([AliasAs("User_Id")] int userId, [Header("Authorization")] string authorization);

        [Get("/getUserActivityLastYear.php")]
        Task<ObservableCollection<Models.UserActivityYearly>> GetUserActivityLastYearOpAsync([AliasAs("User_Id")] int userId, [Header("Authorization")] string authorization);

        [Get("/getUserAveragesActivity.php")]
        Task<Models.UserAverageActivity> GetUserAveragesActivityOpAsync([AliasAs("User_Id")] int userId, [Header("Authorization")] string authorization);

        [Get("/getUserSocial.php")]
        Task<Models.UserSocial> GetUserSocialOpAsync([AliasAs("User_Id")] int userId, [Header("Authorization")] string authorization);

        [Get("/getUserComments.php")]
        Task<ObservableCollection<Models.UserComment>> GetUserCommentsOpAsync([AliasAs("User_Id")] int userId, [Header("Authorization")] string authorization);
    }
}
