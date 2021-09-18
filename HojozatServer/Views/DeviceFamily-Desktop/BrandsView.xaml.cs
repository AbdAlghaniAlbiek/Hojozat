using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HojozatServer.Models;
using HojozatServer.Repositories.RemoteRepo;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Connectivity;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrandsView : Page
    {

        public BrandsView()
        {
            this.InitializeComponent();

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
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
            if (BrandsRepo.IsFirstConnection)
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        loadingBrandsData.IsLoading = true;
                        
                        brandsMasterDetails.ItemsSource = await BrandsRepo.GetBrandsAsync();

                        loadingBrandsData.IsLoading = false;

                        BrandsRepo.IsFirstConnection = false;
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
                    loadingBrandsData.IsLoading = true;
                 
                    brandsMasterDetails.ItemsSource = await BrandsRepo.GetBrandsAsync();

                    loadingBrandsData.IsLoading = false;
                });
            }
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (BrandsRepo.IsFirstConnection)
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

                        loadingBrandsData.IsLoading = true;

                        brandsMasterDetails.ItemsSource = await BrandsRepo.GetBrandsAsync();

                        loadingBrandsData.IsLoading = false;

                        UsersRepo.IsFirstConnection = false;
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
                        StackPanel mainBrandDetails =
                                (StackPanel)FindChildControl<StackPanel>(this, "mainBrandDetails");

                        if (brandsMasterDetails.SelectedItem != null && mainBrandDetails.DataContext == null)
                        {
                            
                            Loading loadingBrandDetailsData =
                                (Loading)FindChildControl<Loading>(this, "loadingBrandDetailsData");
                            Loading networkingIssueConnDetails =
                                (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                            networkingIssueConnDetails.IsLoading = false;

                            loadingBrandDetailsData.IsLoading = true;

                            mainBrandDetails.DataContext =
                                await BrandsRepo.GetBrandDetailsAsync(((Brand)brandsMasterDetails.SelectedItem).Id);

                            loadingBrandDetailsData.IsLoading = false;
                        }
                    });
                }
            }
        }

        private async void brandsMasterDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);

            if (HasInternetAccess)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    if (brandsMasterDetails.SelectedItem != null)
                    {
                        StackPanel mainBrandDetails =
                               (StackPanel)FindChildControl<StackPanel>(this, "mainBrandDetails");
                        Loading loadingBrandDetailsData =
                            (Loading)FindChildControl<Loading>(this, "loadingBrandDetailsData");
                        Loading networkingIssueConnDetails =
                            (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                        networkingIssueConnDetails.IsLoading = false;

                        loadingBrandDetailsData.IsLoading = true;

                        mainBrandDetails.DataContext =
                            await BrandsRepo.GetBrandDetailsAsync(((Brand)brandsMasterDetails.SelectedItem).Id);

                        loadingBrandDetailsData.IsLoading = false;
                    }
                });
            }
            else
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    StackPanel mainBrandDetails =
                               (StackPanel)FindChildControl<StackPanel>(this, "mainBrandDetails");

                    Loading networkingIssueConnDetails =
                          (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                    networkingIssueConnDetails.IsLoading = true;

                    mainBrandDetails.DataContext = null;
                });
            }
        }

        private async void txtSearchBrands_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
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
                            StackPanel mainBrandDetails =
                              (StackPanel)FindChildControl<StackPanel>(this, "mainBrandDetails");
                            Loading loadingBrandDetailsData =
                                (Loading)FindChildControl<Loading>(this, "loadingBrandDetailsData");
                            Loading networkingIssueConnDetails =
                                (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                            networkingIssueConnDetails.IsLoading = false;

                            loadingBrandDetailsData.IsLoading = true;

                            int brandId =
                              await BrandsRepo.GetBrandIdByNameAsync(sender.Text);

                            mainBrandDetails.DataContext =
                                await BrandsRepo.GetBrandDetailsAsync(brandId);

                            sender.ItemsSource = null;
                            //Don't implement it because it causes problems
                            //usersMasterDetails.SelectedItem = null;

                            loadingBrandDetailsData.IsLoading = false;
                        }
                        else
                        {
                            FontIcon networkIssueIcon =
                               (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");

                            TextBlock txtNetworkIssue =
                               (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                            sender.Text = "";
                            sender.ItemsSource = null;

                            networkIssueIcon.Visibility = Visibility.Visible;
                            txtNetworkIssue.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        sender.Text = "";
                    }
                });
            }
        }

        private async void txtSearchBrands_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Down && e.Key != Windows.System.VirtualKey.Up)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                    {
                        AutoSuggestBox txtSearchBrands =
                             (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchBrands");
                        FontIcon networkIssueIcon =
                             (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");
                        TextBlock txtNetworkIssue =
                             (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                        if (!string.IsNullOrEmpty(txtSearchBrands.Text))
                        {
                            networkIssueIcon.Visibility = Visibility.Collapsed;
                            txtNetworkIssue.Visibility = Visibility.Collapsed;

                            txtSearchBrands.ItemsSource =
                                await BrandsRepo.GetBrandsSearchNamesAsync(txtSearchBrands.Text);
                        }
                        else
                        {
                            txtSearchBrands.ItemsSource = null;
                        }
                    }
                    else
                    {
                        AutoSuggestBox txtSearchBrands =
                                 (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchBrands");
                        FontIcon networkIssueIcon =
                                (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");
                        TextBlock txtNetworkIssue =
                           (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                        txtSearchBrands.ItemsSource = null;

                        networkIssueIcon.Visibility = Visibility.Visible;
                        txtNetworkIssue.Visibility = Visibility.Visible;
                    }
                });

            }
        }
    }
}

    
