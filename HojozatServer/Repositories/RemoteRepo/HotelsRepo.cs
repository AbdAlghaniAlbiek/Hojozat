using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using HojozatServer.Repositories.RepoOperations;

namespace HojozatServer.Repositories.RemoteRepo
{
    public class HotelsRepo
    {
        public static ObservableCollection<Models.HotelsCollectedByBrand> NormalHotels { get; set; }
        public static ObservableCollection<Models.HotelsCollectedByBrand> MediumHotels { get; set; }
        public static ObservableCollection<Models.HotelsCollectedByBrand> SuperHotels { get; set; }
        public static bool IsFirstConnection { get; set; } = true;



        public static async Task<int> GetHotelIdByNameAsync(string hotelName)
        {
            var hotelsApis =
                 RestService.For<IHotelsRepoOps>(Common.URL);

            int hotelId =
                await hotelsApis.GetHotelIdByNameOpAsync(hotelName, Common.Token);

            return hotelId;
        }

        public static async Task<ObservableCollection<string>> GetHotelsSearchNamesAsync(string hostType, string hotelNamePrefix)
        {
            var hotelsApis =
                 RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<string> hotelsSearchNames =
                await hotelsApis.GetHotelsSearchNamesOpAsync(hostType, hotelNamePrefix, Common.Token);

            if (hotelsSearchNames.Count > 0)
            {
                return hotelsSearchNames;
            }

            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext();
            resourceContext.QualifierValues["Language"] = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("MainViewStrings");
            string noUserFound = resourceMap.GetValue("NoResultFound", resourceContext).ValueAsString;

            hotelsSearchNames.Add(noUserFound);

            return hotelsSearchNames;
        }

        public async static Task<ObservableCollection<Models.HotelsCollectedByBrand>> GetHotelsCollectedByBrandAsync(string hostType)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            if (hostType == "Normal")
            {
                if (NormalHotels == null)
                {
                    ObservableCollection<Models.Hotel> normalHotels =
                         await hotelsAPIs.GetHotelsOpAsync("Normal", Common.Token);

                    if (normalHotels != null)
                    {
                        NormalHotels = await Task.Run(() => GetHotelsCollectedByBrandCustomized(normalHotels));
                        return NormalHotels;
                    }
                }
                else
                {
                    return NormalHotels;
                }
            }
            else if (hostType == "Medium")
            {
                if (MediumHotels == null)
                {
                    ObservableCollection<Models.Hotel> mediumHotels =
                          await hotelsAPIs.GetHotelsOpAsync("Medium", Common.Token);

                    if (mediumHotels != null)
                    {
                        MediumHotels = await Task.Run(() => GetHotelsCollectedByBrandCustomized(mediumHotels));
                        return MediumHotels;
                    }
                }
                else
                {
                    return MediumHotels;
                }
            }
            else if (hostType == "Super")
            {
                if (SuperHotels == null)
                {
                    ObservableCollection<Models.Hotel> superHotels =
                       await hotelsAPIs.GetHotelsOpAsync("Super", Common.Token);

                    if (superHotels != null)
                    {
                        SuperHotels = await Task.Run(() => GetHotelsCollectedByBrandCustomized(superHotels));
                        return SuperHotels;
                    }
                }
                else
                {
                    return SuperHotels;
                }
            }

            return null;
        }

        public async static Task<Models.HotelDetails> GetHotelDetailsAsync(int hotelId)
        {
            ObservableCollection<Models.HotelActivityYearly> hotelActivityCurrentYear = await GetHotelActivityCurrentYearAsync(hotelId);
            ObservableCollection<Models.HotelActivityYearly> hotelActivityLastYear = await GetHotelActivityLastYearAsync(hotelId);

            return new Models.HotelDetails()
            {
                HotelProfile = await GetHotelProfileAsync(hotelId),
                HotelFacilities = await GetHotelFacilitiesAsync(hotelId),
                HotelRules = await GetHotelRulesAsync(hotelId),
                HotelPhotos = await GetHotelPhotosAsync(hotelId),
                HotelRooms = new Models.HotelRooms()
                {
                    HotelRoomsTypes = await GetHotelRoomsTypesAsync(hotelId),
                    HotelRoomsCountByType = await GetHotelRoomsCountByTypeAsync(hotelId)
                },
                HotelActivity = new Models.HotelActivity()
                {
                    HotelActivityCurrentYear = await Task.Run(() => GetHotelActivityCurrentYearCustomized(hotelActivityCurrentYear)),
                    HotelActivityLastYear = await Task.Run(() => GetHotelActivityLastYearCustomized(hotelActivityLastYear)),
                    HotelAveragesActivity = await GetHotelAveragesActivityAsync(hotelId)
                },
                HotelUsersFeedback = new Models.HotelUsersFeedback()
                {
                    HotelUsersComments = await GetHotelUsersCommentsAsync(hotelId),
                    HotelFavouritesHistory = await GetHotelUsersFavouritesHistoryAsync(hotelId),
                    HotelUsersRatings = await GetHotelUsersRatingsAsync(hotelId)
                }
            };
        }

        public static async Task<Models.HotelRoomsTypeDetails> GetHotelRoomsTypeDetailsAsync(int hotelId, string roomType)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            Models.HotelRoomsTypeDetails hotelRoomsTypeDetails =
                await hotelsAPIs.GetHotelRoomsTypeDetailsOpAsync(hotelId, roomType, Common.Token);

            if (hotelRoomsTypeDetails != null)
            {
                return hotelRoomsTypeDetails;
            }
            return null;
        }



        //These methods it called by GetHotelDetails Medthod

        private  static ObservableCollection<Models.HotelActivityYearly> GetHotelActivityCurrentYearCustomized(ObservableCollection<Models.HotelActivityYearly> hotelActivityCurrentYear)
        {
            ObservableCollection<Models.HotelActivityYearly> hotelActivityCurrentYearCustomized =
               new ObservableCollection<Models.HotelActivityYearly>();

            bool IsMonthFound = false;
            byte objIndex = 0;

            for (byte i = 1; i <= 12; i++)
            {
                for (byte j = 0; j < hotelActivityCurrentYear.Count; j++)
                {
                    if (hotelActivityCurrentYear[j].Month == i)
                    {
                        IsMonthFound = true;
                        objIndex = j;
                    }
                }

                if (IsMonthFound)
                {
                    hotelActivityCurrentYearCustomized.Add(hotelActivityCurrentYear[objIndex]);
                    IsMonthFound = false;
                }
                else
                {
                    hotelActivityCurrentYearCustomized.Add(
                    new Models.HotelActivityYearly() { Month = i, Bookings = 0, Payments = 0 });
                }
            }

            return hotelActivityCurrentYearCustomized;
        }

        private static ObservableCollection<Models.HotelActivityYearly> GetHotelActivityLastYearCustomized(ObservableCollection<Models.HotelActivityYearly> hotelActivityLastYear)
        {
            ObservableCollection<Models.HotelActivityYearly> hotelActivityLastYearCustomized =
               new ObservableCollection<Models.HotelActivityYearly>();

            bool IsMonthFound = false;
            byte objIndex = 0;

            for (byte i = 1; i <= 12; i++)
            {
                for (byte j = 0; j < hotelActivityLastYear.Count; j++)
                {
                    if (hotelActivityLastYear[j].Month == i)
                    {
                        IsMonthFound = true;
                        objIndex = j;
                    }
                }

                if (IsMonthFound)
                {
                    hotelActivityLastYearCustomized.Add(hotelActivityLastYear[objIndex]);
                    IsMonthFound = false;
                }
                else
                {
                    hotelActivityLastYearCustomized.Add(
                       new Models.HotelActivityYearly() { Month = i, Bookings = 0, Payments = 0 });
                }
            }

            return hotelActivityLastYearCustomized;
        }

        private static ObservableCollection<Models.HotelsCollectedByBrand> GetHotelsCollectedByBrandCustomized(ObservableCollection<Models.Hotel> hotels)
        {
            ObservableCollection<Models.HotelsCollectedByBrand> hotelsCollectedByBrand =
                new ObservableCollection<Models.HotelsCollectedByBrand>();

            bool isBrandFound = false;

            for (int i = 0; i < hotels.Count; i++)
            {
                isBrandFound = false;

                if (hotelsCollectedByBrand.Count != 0)
                {
                    for (int j = 0; j < hotelsCollectedByBrand.Count; j++)
                    {

                        if (hotelsCollectedByBrand[j].BrandName == hotels[i].BrandName)
                        {
                            hotelsCollectedByBrand[j].Hotels.Add(hotels[i]);
                            isBrandFound = true;
                            break;
                        }

                    }
                }
                if (!isBrandFound)
                {
                    hotelsCollectedByBrand.Add(
                              new Models.HotelsCollectedByBrand() { BrandName = hotels[i].BrandName, Hotels = new ObservableCollection<Models.Hotel>()});

                    hotelsCollectedByBrand[hotelsCollectedByBrand.Count - 1].Hotels.Add(hotels[i]);
                }
            }

            return hotelsCollectedByBrand;
        }

        private static async Task<Models.HotelProfile> GetHotelProfileAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            Models.HotelProfile hotelProfile =
                await hotelsAPIs.GetHotelProfileOpAsync(hotelId, Common.Token);

            if (hotelProfile != null)
            {
                return hotelProfile;
            }
            return null;
        }

        private static async Task<ObservableCollection<string>> GetHotelPhotosAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<string> hotelRules =
                await hotelsAPIs.GetHotelPhotosOpAsync(hotelId, Common.Token);

            if (hotelRules != null)
            {
                return hotelRules;
            }
            return null;
        }

        private static async Task<ObservableCollection<Models.HotelFacility>> GetHotelFacilitiesAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<Models.HotelFacility> hotelFacilities =
                await hotelsAPIs.GetHotelFacilitiesOpAsync(hotelId, Common.Token);

            if (hotelFacilities != null)
            {
                return hotelFacilities;
            }
            return null;
        }

        private static async Task<ObservableCollection<string>> GetHotelRulesAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<string> hotelRules =
                await hotelsAPIs.GetHotelRulesOpAsync(hotelId, Common.Token);

            if (hotelRules != null)
            {
                return hotelRules;
            }
            return null;
        }

        private static async Task<ObservableCollection<string>> GetHotelRoomsTypesAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<string> hotelRoomsTypes =
                await hotelsAPIs.GetHotelRoomsTypesOpAsync(hotelId, Common.Token);

            if (hotelRoomsTypes != null)
            {
                return hotelRoomsTypes;
            }
            return null;
        }

        private static async Task<ObservableCollection<Models.HotelRoomsCountByType>> GetHotelRoomsCountByTypeAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<Models.HotelRoomsCountByType> hotelRoomsNumberByType =
                await hotelsAPIs.GetHotelRoomsCountByTypeOpAsync(hotelId, Common.Token);

            if (hotelRoomsNumberByType != null)
            {
                return hotelRoomsNumberByType;
            }
            return null;
        }

        private static async Task<ObservableCollection<Models.HotelActivityYearly>> GetHotelActivityCurrentYearAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<Models.HotelActivityYearly> hotelActivityCurrentYear =
                await hotelsAPIs.GetHotelActivityCurrentYearOpAsync(hotelId, Common.Token);

            if (hotelActivityCurrentYear != null)
            {
                return hotelActivityCurrentYear;
            }
            return null;
        }

        private static async Task<ObservableCollection<Models.HotelActivityYearly>> GetHotelActivityLastYearAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<Models.HotelActivityYearly> hotelActivityLastYear =
                await hotelsAPIs.GetHotelActivityLastYearOpAsync(hotelId, Common.Token);

            if (hotelActivityLastYear != null)
            {
                return hotelActivityLastYear;
            }
            return null;
        }

        private static async Task<Models.HotelAveragesActivity> GetHotelAveragesActivityAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            Models.HotelAveragesActivity hotelAveragesActivity =
                await hotelsAPIs.GetHotelAveragesActivityOpAsync(hotelId, Common.Token);

            if (hotelAveragesActivity != null)
            {
                return hotelAveragesActivity;
            }
            return null;
        }

        private static async Task<ObservableCollection<Models.HotelUserComment>> GetHotelUsersCommentsAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<Models.HotelUserComment> hotelUsersComments =
                await hotelsAPIs.GetHotelUsersCommentsOpAsync(hotelId, Common.Token);

            if (hotelUsersComments != null)
            {
                return hotelUsersComments;
            }
            return null;
        }

        private static async Task<Models.HotelUsersFavouritesHistory> GetHotelUsersFavouritesHistoryAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

           Models.HotelUsersFavouritesHistory hotelFavHis =
                await hotelsAPIs.GetHotelUsersFavouritesHistoryOpAsync(hotelId, Common.Token);

            if (hotelFavHis != null)
            {
                return hotelFavHis;
            }
            return null;
        }

        private static async Task<ObservableCollection<Models.HotelUsersRating>> GetHotelUsersRatingsAsync(int hotelId)
        {
            var hotelsAPIs =
                RestService.For<IHotelsRepoOps>(Common.URL);

            ObservableCollection<Models.HotelUsersRating> hotelUsersRatings =
                 await hotelsAPIs.GetHotelUsersRatingsOpAsync(hotelId, Common.Token);

            if (hotelUsersRatings != null)
            {
                return hotelUsersRatings;
            }
            return null;
        }
    }
}
