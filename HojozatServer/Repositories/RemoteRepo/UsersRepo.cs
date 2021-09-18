using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Repositories.RepoOperations;
using HojozatServer.Models;

namespace HojozatServer.Repositories.RemoteRepo
{
    public class UsersRepo
    {
        private static ObservableCollection<Models.User> Users { get; set; }
        public static bool IsFirstConnection { get; set; } = true;


        public static async Task<int> GetUserIdByNameAsync(string userName)
        {
            var usersApis =
                 RestService.For<IUsersRepoOps>(Common.URL);

            int userId =
                await usersApis.GetUserIdByNameOpAsync(userName, Common.Token);

            return userId;
        }

        public static async Task<ObservableCollection<string>> GetUsersSearchNamesAsync(string userNamePrefix)
        {

            var usersApis =
                 RestService.For<IUsersRepoOps>(Common.URL);

            ObservableCollection<string> usersSearchNames =
                await usersApis.GetUsersSearchNamesOpAsync(userNamePrefix, Common.Token);

            if (usersSearchNames.Count > 0)
            {
                return usersSearchNames;
            }

            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext();
            resourceContext.QualifierValues["Language"] = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("MainViewStrings");
            string noUserFound = resourceMap.GetValue("NoResultFound", resourceContext).ValueAsString;

            usersSearchNames.Add(noUserFound);

            return usersSearchNames;
        }

        public static async Task<ObservableCollection<Models.User>> GetUsersAsync()
        {
            if (Users == null)
            {
                var UsersApis =
                      RestService.For<IUsersRepoOps>(Common.URL);

                var users =
                     await UsersApis.GetUsersOpAsync(Common.Token);

                if (users != null)
                {
                    Users = users;
                    return Users;
                }
                return null;
            }
            else
                return Users;
        }

        public static async Task<Models.UserDetails> GetUserDetailsAsync(int userId)
        {
            var userProfile = await  GetUserProfileAsync(userId);
            var userActivityCurrentYear = await  GetUserActivityCurrentYearAsync(userId);
            var userActivityLastYear = await GetUserActivityLastYearAsync(userId);
            var userAveragesActivity = await GetUserAveragesActivityAsync(userId);
            var userSocial = await GetUserSocialAsync(userId);
            var userComments = await GetUserCommentsAsync(userId);

            //while (!userProfile.IsCompleted || !userActivityCurrentYear.IsCompleted || !userActivityLastYear.IsCompleted ||
            //       !userAveragesActivity.IsCompleted || !userSocial.IsCompleted || !userComments.IsCompleted)
            //{ }

            return new Models.UserDetails()
            {
                UserProfile = userProfile,
                UserActivity = new Models.UserActivity()
                {
                    UserAverageActivity = userAveragesActivity,
                    UserActivityCurrentYear = GetUserActivityCurrentYearCustomized(userActivityCurrentYear),
                    UserActivityLastYear = GetUserActivityLastYearCustomized(userActivityLastYear),
                },
                UserCommunication = new Models.UserCommunication()
                {
                    UserSocial = userSocial,
                    UserComments = userComments
                }
            };
        }



        //All these Methods used by GetUserDetailsAsync method
        private static ObservableCollection<Models.UserActivityYearly> GetUserActivityCurrentYearCustomized(ObservableCollection<Models.UserActivityYearly> userActivityCurrentYear)
        {
            ObservableCollection<Models.UserActivityYearly> userActivityCurrentYearCustomized =
               new ObservableCollection<Models.UserActivityYearly>();

            bool IsMonthFound = false;
            byte objIndex = 0;

            for (byte i = 1; i <= 12; i++)
            {
                for (byte j = 0; j < userActivityCurrentYear.Count; j++)
                {
                    if (userActivityCurrentYear[j].Month == i)
                    {
                        IsMonthFound = true;
                        objIndex = j;
                    }
                }

                if (IsMonthFound)
                {
                    userActivityCurrentYearCustomized.Add(userActivityCurrentYear[objIndex]);
                    IsMonthFound = false;
                }
                else
                {
                    userActivityCurrentYearCustomized.Add(
                    new Models.UserActivityYearly() { Month = i, Bookings = 0, Payments = 0 });
                }
            }

            return userActivityCurrentYearCustomized;
        }

        private static ObservableCollection<Models.UserActivityYearly> GetUserActivityLastYearCustomized(ObservableCollection<Models.UserActivityYearly> userActivityLastYear)
        {
            ObservableCollection<Models.UserActivityYearly> userActivityLastYearCustomized =
               new ObservableCollection<Models.UserActivityYearly>();

            bool IsMonthFound = false;
            byte objIndex = 0;

            for (byte i = 1; i <= 12; i++)
            {
                for (byte j = 0; j < userActivityLastYear.Count; j++)
                {
                    if (userActivityLastYear[j].Month == i)
                    {
                        IsMonthFound = true;
                        objIndex = j;
                    }
                }

                if (IsMonthFound)
                {
                    userActivityLastYearCustomized.Add(userActivityLastYear[objIndex]);
                    IsMonthFound = false;
                }
                else
                {
                    userActivityLastYearCustomized.Add(
                       new Models.UserActivityYearly() { Month = i, Bookings = 0, Payments = 0 });
                }
            }

            return userActivityLastYearCustomized;
        }

        private static async Task<Models.UserProfile> GetUserProfileAsync(int userId)
        {
            var userAPIs =
                 RestService.For<IUsersRepoOps>(Common.URL);

            Models.UserProfile userProfile =
                await userAPIs.GetUserProfileOpAsync(userId, Common.Token);

            return userProfile;
        }

        private static async Task<ObservableCollection<Models.UserActivityYearly>> GetUserActivityCurrentYearAsync(int userId)
        {
            var userAPIs =
                 RestService.For<IUsersRepoOps>(Common.URL);

            ObservableCollection<Models.UserActivityYearly> userActivityCurrentYear =
                await userAPIs.GetUserActivityCurrentYearOpAsync(userId, Common.Token);

            return userActivityCurrentYear;
        }

        private static async Task<ObservableCollection<Models.UserActivityYearly>> GetUserActivityLastYearAsync(int userId)
        {
            var userAPIs =
                 RestService.For<IUsersRepoOps>(Common.URL);

            ObservableCollection<Models.UserActivityYearly> userActivityLastYear =
                await userAPIs.GetUserActivityLastYearOpAsync(userId, Common.Token);

            return userActivityLastYear;
        }

        private static async Task<Models.UserAverageActivity> GetUserAveragesActivityAsync(int userId)
        {
            var userAPIs =
                 RestService.For<IUsersRepoOps>(Common.URL);

            Models.UserAverageActivity userAverageActivity =
                await userAPIs.GetUserAveragesActivityOpAsync(userId, Common.Token);

            return userAverageActivity;
        }

        private static async Task<Models.UserSocial> GetUserSocialAsync(int userId)
        {
            var userAPIs =
                 RestService.For<IUsersRepoOps>(Common.URL);

            Models.UserSocial userSocial =
                await userAPIs.GetUserSocialOpAsync(userId, Common.Token);

            return userSocial;
        }

        private static async Task<ObservableCollection<Models.UserComment>> GetUserCommentsAsync(int userId)
        {
            var userAPIs =
                 RestService.For<IUsersRepoOps>(Common.URL);

            ObservableCollection<Models.UserComment> userComments =
                await userAPIs.GetUserCommentsOpAsync(userId, Common.Token);

            return userComments;
        }

    }
}
