using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HojozatServer.Models;
using HojozatServer.Repositories.RemoteRepo;
using Windows.Networking.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Toolkit.Uwp;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Microsoft.Toolkit.Uwp.Helpers;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GlobalizationView : Page
    {


        public GlobalizationView()
        {
            this.InitializeComponent();

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        private  DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            if (GlobalizationRepo.IsFirstConnection)
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        loadingGlobData.IsLoading = true;

                        globalizationMasterDetails.ItemsSource = await GlobalizationRepo.GetCountriesAsync();

                        loadingGlobData.IsLoading = false;

                        GlobalizationRepo.IsFirstConnection = false;
                    });
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        networkingIssueConnecting.IsLoading = true;
                    });
                }
            }
            else
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    loadingGlobData.IsLoading = true;

                    globalizationMasterDetails.ItemsSource = await GlobalizationRepo.GetCountriesAsync();

                    loadingGlobData.IsLoading = false;
                });    
            }
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (GlobalizationRepo.IsFirstConnection)
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        networkingIssueConnecting.IsLoading = false;

                        loadingGlobData.IsLoading = true;

                        globalizationMasterDetails.ItemsSource = await GlobalizationRepo.GetCountriesAsync();

                        loadingGlobData.IsLoading = false;

                        GlobalizationRepo.IsFirstConnection = false;
                    }); 
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        networkingIssueConnecting.IsLoading = true;
                    });
                }
            }
            else
            {

                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        GridView citiesDetailsGridView =
                               (GridView)FindChildControl<GridView>(this, "gridViewCitiesDetails");

                        if (globalizationMasterDetails.SelectedItem != null && citiesDetailsGridView.ItemsSource == null)
                        {
                            AutoSuggestBox txtSearchCities =
                                (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchCities");
                            Loading loadingCitiesData =
                                (Loading)FindChildControl<Loading>(this, "loadingCitiesData");
                            Loading networkingIssueConnDetails =
                                (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                            networkingIssueConnDetails.IsLoading = false;

                            loadingCitiesData.IsLoading = true;

                            txtSearchCities.ItemsSource =
                                await GlobalizationRepo.GetCitiesSearchNamesAsync(((Country)globalizationMasterDetails.SelectedItem).Id);

                            citiesDetailsGridView.ItemsSource =
                                await GlobalizationRepo.GetCitiesAsync(((Country)globalizationMasterDetails.SelectedItem).Id);

                            loadingCitiesData.IsLoading = false;
                        }
                    });
                }
            }
        }

        private async void GlobalizationMasterDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);

            if (((MasterDetailsView)sender).SelectedItem != null)
            {
                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        GridView mainCitiesDetails =
                                 (GridView)FindChildControl<GridView>(this, "gridViewCitiesDetails");
                        Loading loadingUserCitiesDetailsData =
                            (Loading)FindChildControl<Loading>(this, "loadingCitiesData");
                        Loading networkingIssueConnDetails =
                            (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                        networkingIssueConnDetails.IsLoading = false;

                        loadingUserCitiesDetailsData.IsLoading = true;

                        mainCitiesDetails.ItemsSource =
                            await GlobalizationRepo.GetCitiesAsync(((Country)globalizationMasterDetails.SelectedItem).Id);

                        loadingUserCitiesDetailsData.IsLoading = false;
                    });
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        GridView mainCitiesDetails =
                                 (GridView)FindChildControl<GridView>(this, "gridViewCitiesDetails");

                        Loading networkingIssueConnDetails =
                              (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                        networkingIssueConnDetails.IsLoading = true;
                        mainCitiesDetails.ItemsSource = null;
                    });
                }
            }
        }

      
        private async void txtSearchCountries_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Down && e.Key != Windows.System.VirtualKey.Up)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {

                    if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                    {
                        AutoSuggestBox txtSearchCountries =
                             (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchCountries");
                        FontIcon networkIssueIcon =
                             (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");
                        TextBlock txtNetworkIssue =
                             (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                        if (!string.IsNullOrEmpty(txtSearchCountries.Text))
                        {
                            networkIssueIcon.Visibility = Visibility.Collapsed;
                            txtNetworkIssue.Visibility = Visibility.Collapsed;

                            txtSearchCountries.ItemsSource =
                                await GlobalizationRepo.GetCountriesSearchNamesAsync(txtSearchCountries.Text);
                        }
                        else
                        {
                            txtSearchCountries.ItemsSource = null;
                        }
                    }
                    else
                    {
                        AutoSuggestBox txtSearchCountries =
                                 (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchCountries");
                        FontIcon networkIssueIcon =
                                (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");
                        TextBlock txtNetworkIssue =
                           (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                        txtSearchCountries.ItemsSource = null;

                        networkIssueIcon.Visibility = Visibility.Visible;
                        txtNetworkIssue.Visibility = Visibility.Visible;
                    }
                });

            }
        }

        private async void txtSearchCountries_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                var resourceContext = new Windows.ApplicationModel.Resources.Core.ResourceContext();
                resourceContext.QualifierValues["Language"] = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
                var resourceMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetSubtree("MainViewStrings");
                string noResultFoundText = resourceMap.GetValue("NoResultFound", resourceContext).ValueAsString;
              
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    if (sender.Text != noResultFoundText)
                    {
                        if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                        {
                            AutoSuggestBox txtSearchCountries =
                                   (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchCountries");
                            GridView citiesDetailsGridView =
                              (GridView)FindChildControl<GridView>(this, "gridViewCitiesDetails");
                            Loading loadingCitiesData =
                                (Loading)FindChildControl<Loading>(this, "loadingCitiesData");
                            Loading networkingIssueConnDetails =
                                (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                            networkingIssueConnDetails.IsLoading = false;

                            loadingCitiesData.IsLoading = true;

                            citiesDetailsGridView.ItemsSource =
                                await GlobalizationRepo.GetCitiesByCountryNameAsync(txtSearchCountries.Text);

                            txtSearchCountries.ItemsSource = null;
                            //Don't implement it because it causes problems
                            //globalizationMasterDetails.SelectedItem = null;

                            loadingCitiesData.IsLoading = false;
                        }
                        else
                        {
                            AutoSuggestBox txtSearchCountries =
                                 (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchCountries");

                            FontIcon networkIssueIcon =
                               (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");

                            TextBlock txtNetworkIssue =
                               (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                            txtSearchCountries.Text = "";
                            txtSearchCountries.ItemsSource = null;

                            networkIssueIcon.Visibility = Visibility.Visible;
                            txtNetworkIssue.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        AutoSuggestBox txtSearchCountries =
                            (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchCountries");

                        txtSearchCountries.Text = "";
                    }
                });
            }
        }
    }
}
