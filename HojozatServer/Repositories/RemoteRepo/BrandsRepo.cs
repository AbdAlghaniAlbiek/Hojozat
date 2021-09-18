using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Repositories.RepoOperations;

namespace HojozatServer.Repositories.RemoteRepo
{
    public class BrandsRepo
    {
        private static ObservableCollection<Models.Brand> Brands { get; set; }
        public static bool IsFirstConnection { get; set; } = true;


        public static async Task<int> GetBrandIdByNameAsync(string brandName)
        {
            var brandsApis =
                 RestService.For<IBrandRepoOps>(Common.URL);

            int brandId =
                await brandsApis.GetBrandIdByNameOpAsync(brandName, Common.Token);

            return brandId;
        }

        public static async Task<ObservableCollection<string>> GetBrandsSearchNamesAsync(string brandNamePrefix)
        {

            var brandsApis =
                 RestService.For<IBrandRepoOps>(Common.URL);

            ObservableCollection<string> brandsSearchNames =
                await brandsApis.GetBrandsNamesOpAsync(brandNamePrefix, Common.Token);

            if (brandsSearchNames.Count > 0)
            {
                return brandsSearchNames;
            }

            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext();
            resourceContext.QualifierValues["Language"] = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("MainViewStrings");
            string noUserFound = resourceMap.GetValue("NoResultFound", resourceContext).ValueAsString;

            brandsSearchNames.Add(noUserFound);

            return brandsSearchNames;
        }

        public static async Task<ObservableCollection<Models.Brand>> GetBrandsAsync()
        {
            if (Brands == null)
            {
                var BrandsApis =
                      RestService.For<IBrandRepoOps>(Common.URL);

                var brands =
                     await BrandsApis.GetBrandsOpAsync(Common.Token);

                if (brands != null)
                {
                    Brands = brands;
                    return Brands;
                }
                return null;
            }
            else
                return Brands;
        }

        public static async Task<Models.BrandDetails> GetBrandDetailsAsync(int brandId)
        {
            var brandProfile = await GetBrandProfileAsync(brandId);
            var brandActivityCurrentYear = await GetBrandActivityCurrentYearAsync(brandId);
            var brandActivityLastYear = await GetBrandActivityLastYearAsync(brandId);
            var brandAveragesActivity = await GetBrandAveragesActivityAsync(brandId);
            var usersFavHis = await GetUsersFavouritesHistoryAsync(brandId);
            var usersRatings = await GetUsersRatingsAsync(brandId);
            var usersComments = await GetUsersCommentsAsync(brandId);
            var brandHotels = await GetBrandHotelsAsync(brandId);

            //while (!userProfile.IsCompleted || !userActivityCurrentYear.IsCompleted || !userActivityLastYear.IsCompleted ||
            //       !userAveragesActivity.IsCompleted || !userSocial.IsCompleted || !userComments.IsCompleted)
            //{ }

            return new Models.BrandDetails()
            {
                BrandProfile = brandProfile,
                BrandActivity = new Models.BrandActivity()
                {
                    BrandAveragesActivity = brandAveragesActivity,
                    BrandActivityCurrentYear = GetBrandActivityCurrentYearCustomized(brandActivityCurrentYear),
                    BrandActivityLastYear = GetBrandActivityLastYearCustomized(brandActivityLastYear)
                },
                UsersFeedback = new Models.UsersFeedback()
                {
                    HotelUsersComments = GetHotelUsersCommentsCustomized(usersComments),
                    UsersFavouritesHistory = usersFavHis,
                    UsersRatings = GetUsersRatingsCustomized(usersRatings)
                },
                BrandHotels = brandHotels
            };
        }



        //All these Methods used by GetBrandDetailsAsync method
        private static ObservableCollection<Models.BrandActivityYearly> GetBrandActivityCurrentYearCustomized(ObservableCollection<Models.BrandActivityYearly> BrandActivityCurrentYear)
        {
            ObservableCollection<Models.BrandActivityYearly> brandActivityCurrentYearCustomized =
               new ObservableCollection<Models.BrandActivityYearly>();

            bool IsMonthFound = false;
            byte objIndex = 0;

            for (byte i = 1; i <= 12; i++)
            {
                for (byte j = 0; j < BrandActivityCurrentYear.Count; j++)
                {
                    if (BrandActivityCurrentYear[j].Month == i)
                    {
                        IsMonthFound = true;
                        objIndex = j;
                    }
                }

                if (IsMonthFound)
                {
                    brandActivityCurrentYearCustomized.Add(BrandActivityCurrentYear[objIndex]);
                    IsMonthFound = false;
                }
                else
                {
                    brandActivityCurrentYearCustomized.Add(
                    new Models.BrandActivityYearly() { Month = i, Bookings = 0, Payments = 0 });
                }
            }

            return brandActivityCurrentYearCustomized;
        }

        private static ObservableCollection<Models.BrandActivityYearly> GetBrandActivityLastYearCustomized(ObservableCollection<Models.BrandActivityYearly> BrandActivityLastYear)
        {
            ObservableCollection<Models.BrandActivityYearly> brandActivityLastYearCustomized =
               new ObservableCollection<Models.BrandActivityYearly>();

            bool IsMonthFound = false;
            byte objIndex = 0;

            for (byte i = 1; i <= 12; i++)
            {
                for (byte j = 0; j < BrandActivityLastYear.Count; j++)
                {
                    if (BrandActivityLastYear[j].Month == i)
                    {
                        IsMonthFound = true;
                        objIndex = j;
                    }
                }

                if (IsMonthFound)
                {
                    brandActivityLastYearCustomized.Add(BrandActivityLastYear[objIndex]);
                    IsMonthFound = false;
                }
                else
                {
                    brandActivityLastYearCustomized.Add(
                       new Models.BrandActivityYearly() { Month = i, Bookings = 0, Payments = 0 });
                }
            }

            return brandActivityLastYearCustomized;
        }

        private static ObservableCollection<Models.UsersRating> GetUsersRatingsCustomized(ObservableCollection<int> usersRatings)
        {
            return new ObservableCollection<Models.UsersRating>()
            {
                new Models.UsersRating(){ Star = 1, RatingValue = usersRatings[0]},
                new Models.UsersRating(){ Star = 2, RatingValue = usersRatings[1]},
                new Models.UsersRating(){ Star = 3, RatingValue = usersRatings[2]},
                new Models.UsersRating(){ Star = 4, RatingValue = usersRatings[3]},
                new Models.UsersRating(){ Star = 5, RatingValue = usersRatings[4]},
            };
        }

        private static ObservableCollection<Models.HotelUsersComments> GetHotelUsersCommentsCustomized(ObservableCollection<Models.UsersComment> usersComments)
        {
            ObservableCollection<Models.HotelUsersComments> totalHotelUsersComments =
                new ObservableCollection<Models.HotelUsersComments>();

            ObservableCollection<Models.UsersComment> hotelUsersComments = null;

            for (int i = 0; i < usersComments.Count; i += 3)
            {
                hotelUsersComments = new ObservableCollection<Models.UsersComment>();

                hotelUsersComments.Add(usersComments[i]);
                hotelUsersComments.Add(usersComments[i + 1]);
                hotelUsersComments.Add(usersComments[i + 2]);

                totalHotelUsersComments.Add(new Models.HotelUsersComments() { HotelName = usersComments[i].HotelName, UsersComments = hotelUsersComments });

                hotelUsersComments = null;
            }

            return totalHotelUsersComments;
        }

        private static async Task<Models.BrandProfile> GetBrandProfileAsync(int brandId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            Models.BrandProfile brandProfile =
                await brandsAPIs.GetBrandProfileOpAsync(brandId, Common.Token);

            return brandProfile;
        }

        private static async Task<ObservableCollection<Models.BrandActivityYearly>> GetBrandActivityCurrentYearAsync(int brandId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            ObservableCollection<Models.BrandActivityYearly> brandActivityCurrentYear =
                await brandsAPIs.GetBrandActivityCurrentYearOpAsync(brandId, Common.Token);

            return brandActivityCurrentYear;
        }

        private static async Task<ObservableCollection<Models.BrandActivityYearly>> GetBrandActivityLastYearAsync(int brandId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            ObservableCollection<Models.BrandActivityYearly> brandActivityLastYear =
                await brandsAPIs.GetBrandActivityLastYearOpAsync(brandId, Common.Token);

            return brandActivityLastYear;
        }

        private static async Task<Models.BrandAveragesActivity> GetBrandAveragesActivityAsync(int brandId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            Models.BrandAveragesActivity brandAverageActivity =
                await brandsAPIs.GetBrandAveragesActivityOpAsync(brandId, Common.Token);

            return brandAverageActivity;
        }

        private static async Task<Models.UsersFavouritesHistory> GetUsersFavouritesHistoryAsync(int userId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            Models.UsersFavouritesHistory UsersFavHis =
                await brandsAPIs.GetUsersFavouritesHistoryOpAsync(userId, Common.Token);

            return UsersFavHis;
        }

        private static async Task<ObservableCollection<Models.UsersComment>> GetUsersCommentsAsync(int userId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            ObservableCollection<Models.UsersComment> usersComments =
                await brandsAPIs.GetUsersCommentsOpAsync(userId, Common.Token);

            return usersComments;
        }

        private static async Task<ObservableCollection<int>> GetUsersRatingsAsync(int userId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            ObservableCollection<int> usersRatings =
                await brandsAPIs.GetUsersRatingsOpAsync(userId, Common.Token);

            return usersRatings;
        }

        private static async Task<ObservableCollection<Models.BrandHotels>> GetBrandHotelsAsync(int userId)
        {
            var brandsAPIs =
                 RestService.For<IBrandRepoOps>(Common.URL);

            ObservableCollection<Models.BrandHotels> brandHotels =
                await brandsAPIs.GetBrandHotelsOpAsync(userId, Common.Token);

            return brandHotels;
        }
    }
}
