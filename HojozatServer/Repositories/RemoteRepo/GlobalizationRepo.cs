using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HojozatServer.Repositories.RepoOperations;
using Refit;

namespace HojozatServer.Repositories.RemoteRepo
{
    public class GlobalizationRepo
    {
        private static ObservableCollection<Models.Country> Countries { get; set; }
        public static bool IsFirstConnection { get; set; } = true;


        public static async  Task<ObservableCollection<string>> GetCountriesSearchNamesAsync(string countryNamePrefix)
        {
            var globalizationApis =
                  RestService.For<IGlobalizationRepoOps>(Common.URL);

            ObservableCollection<string> countriesSearchNames =
                await globalizationApis.GetCountriesSearchNamesOpAsync(countryNamePrefix, Common.Token);

            if (countriesSearchNames.Count > 0)
            {
                return countriesSearchNames;
            }

            var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext(); 
            resourceContext.QualifierValues["Language"] = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("MainViewStrings");
            string noResultFoundText = resourceMap.GetValue("NoResultFound", resourceContext).ValueAsString;

            countriesSearchNames.Add(noResultFoundText);

            return countriesSearchNames;
        }

        public static async Task<ObservableCollection<Models.Country>> GetCountriesAsync()
        {
            if (Countries == null)
            {
                var globalizationApis =
                    RestService.For<IGlobalizationRepoOps>(Common.URL);

                ObservableCollection<Models.Country> countries =
                    await globalizationApis.GetCountriesOpAsync(Common.Token);

                if (countries != null)
                {
                    Countries = countries;
                    return Countries;
                }
                return null;
            }
            else
                return Countries;
        }

        public static async Task<ObservableCollection<string>> GetCitiesSearchNamesAsync(byte countryId)
        {
            var globalizationApis =
                RestService.For<IGlobalizationRepoOps>(Common.URL);

            ObservableCollection<string> citiesSearchNames =
                await globalizationApis.GetCitiesSearchNamesOpAsync(countryId, Common.Token);

            return citiesSearchNames;
        }

        public static async Task<ObservableCollection<Models.City>> GetCitiesByCountryNameAsync(string countryName)
        {
            var globalizationApis =
               RestService.For<IGlobalizationRepoOps>(Common.URL);

            ObservableCollection<Models.City> cities =
                await globalizationApis.GetCitiesByCountryNameOpAsync(countryName, Common.Token);

            return cities;
        }

        public static async Task<ObservableCollection<Models.City>> GetCitiesAsync(byte countryId)
        {
            var globalizationApis =
                RestService.For<IGlobalizationRepoOps>(Common.URL);

            ObservableCollection<Models.City> cities =
                await globalizationApis.GetCitiesOpAsync(countryId, Common.Token);

            return cities;
        }
    }
}
