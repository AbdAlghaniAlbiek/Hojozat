using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace HojozatServer.Repositories.RepoOperations
{
    public interface IGlobalizationRepoOps
    {
        [Get("/getCountriesNames.php")]
        Task<ObservableCollection<string>> GetCountriesSearchNamesOpAsync([AliasAs("Country_Name_Prefix")] string countryNamePrefix, [Header("Authorization")] string authorization);
        
        [Get("/getCountries.php")]
        Task<ObservableCollection<Models.Country>> GetCountriesOpAsync([Header("Authorization")] string authorization);

        [Get("/getCitiesNames.php")]
        Task<ObservableCollection<string>> GetCitiesSearchNamesOpAsync([AliasAs("Country_Id")] byte countryId, [Header("Authorization")] string authorization);

        [Get("/getCitiesByCountryName.php")]
        Task<ObservableCollection<Models.City>> GetCitiesByCountryNameOpAsync([AliasAs("Country_Name")] string countryName, [Header("Authorization")] string authorization);

        [Get("/getCities.php")]
        Task<ObservableCollection<Models.City>> GetCitiesOpAsync([AliasAs("Country_Id")] byte countryId, [Header("Authorization")] string authorization);
    }
}
