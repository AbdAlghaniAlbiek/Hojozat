using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface IHotelsRepoOps
    {
        [Get("/getHotelsNames.php")]
        Task<ObservableCollection<string>> GetHotelsSearchNamesOpAsync([AliasAs("Host_Type")] string hostType, [AliasAs("Hotel_Name_Prefix")] string hotelNamePrefix, [Header("Authorization")] string authorization);

        [Get("/getHotelIdByName.php")]
        Task<int> GetHotelIdByNameOpAsync([AliasAs("Hotel_Name")] string hotelName, [Header("Authorization")] string authorization);

        [Get("/getHotels.php")]
        Task<ObservableCollection<Models.Hotel>> GetHotelsOpAsync([AliasAs("Host_Type")] string hostType, [Header("Authorization")] string authorization);

        [Get("/getHotelProfile.php")]
        Task<Models.HotelProfile> GetHotelProfileOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelPhotos.php")]
        Task<ObservableCollection<string>> GetHotelPhotosOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelFacilities.php")]
        Task<ObservableCollection<Models.HotelFacility>> GetHotelFacilitiesOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelRules.php")]
        Task<ObservableCollection<string>> GetHotelRulesOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelRoomsTypes.php")]
        Task<ObservableCollection<string>> GetHotelRoomsTypesOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelRoomsTypeDetails.php")]
        Task<Models.HotelRoomsTypeDetails> GetHotelRoomsTypeDetailsOpAsync([AliasAs("Hotel_Id")] int hotelId, [AliasAs("Room_Type")] string roomType, [Header("Authorization")] string authorization);

        [Get("/getHotelRoomsCountByType.php")]
        Task<ObservableCollection<Models.HotelRoomsCountByType>> GetHotelRoomsCountByTypeOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelActivityCurrentYear.php")]
        Task<ObservableCollection<Models.HotelActivityYearly>> GetHotelActivityCurrentYearOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelActivityLastYear.php")]
        Task<ObservableCollection<Models.HotelActivityYearly>> GetHotelActivityLastYearOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelAveragesActivity.php")]
        Task <Models.HotelAveragesActivity> GetHotelAveragesActivityOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelUsersComments.php")]
        Task <ObservableCollection<Models.HotelUserComment>> GetHotelUsersCommentsOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelUsersFavouritesHistory.php")]
        Task<Models.HotelUsersFavouritesHistory> GetHotelUsersFavouritesHistoryOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);

        [Get("/getHotelUsersRatings.php")]
        Task<ObservableCollection<Models.HotelUsersRating>> GetHotelUsersRatingsOpAsync([AliasAs("Hotel_Id")] int hotelId, [Header("Authorization")] string authorization);
    }
}
