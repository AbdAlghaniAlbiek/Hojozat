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
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using HojozatServer.Repositories.RemoteRepo;
using Windows.Networking.Connectivity;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Connectivity;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UsersView : Page
    {
        public UsersView()
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
            if (UsersRepo.IsFirstConnection)
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        loadingUsersData.IsLoading = true;

                        usersMasterDetails.ItemsSource = await UsersRepo.GetUsersAsync();

                        loadingUsersData.IsLoading = false;

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
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    loadingUsersData.IsLoading = true;

                    usersMasterDetails.ItemsSource = await UsersRepo.GetUsersAsync();

                    loadingUsersData.IsLoading = false;
                });
            }
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (UsersRepo.IsFirstConnection)
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

                        loadingUsersData.IsLoading = true;

                        usersMasterDetails.ItemsSource = await UsersRepo.GetUsersAsync();

                        loadingUsersData.IsLoading = false;

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
                        StackPanel mainUserDetails =
                                (StackPanel)FindChildControl<StackPanel>(this, "mainUserDetails");

                        if (usersMasterDetails.SelectedItem != null && mainUserDetails.DataContext == null)
                        {
                            
                            Loading loadingUserDetailsData =
                                (Loading)FindChildControl<Loading>(this, "loadingUserDetailsData");
                            Loading networkingIssueConnDetails =
                                (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                            networkingIssueConnDetails.IsLoading = false;

                            loadingUserDetailsData.IsLoading = true;

                            mainUserDetails.DataContext =
                                await UsersRepo.GetUserDetailsAsync(((User)usersMasterDetails.SelectedItem).Id);

                            loadingUserDetailsData.IsLoading = false;
                        }
                    });
                }
            }
        }

        private async void  usersMasterDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);

            if (HasInternetAccess)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    if (usersMasterDetails.SelectedItem != null)
                    {
                        StackPanel mainUserDetails =
                             (StackPanel)FindChildControl<StackPanel>(this, "mainUserDetails");
                        Loading loadingUserDetailsData =
                            (Loading)FindChildControl<Loading>(this, "loadingUserDetailsData");
                        Loading networkingIssueConnDetails =
                            (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                        networkingIssueConnDetails.IsLoading = false;

                        loadingUserDetailsData.IsLoading = true;

                        mainUserDetails.DataContext =
                            await UsersRepo.GetUserDetailsAsync(((User)usersMasterDetails.SelectedItem).Id);

                        loadingUserDetailsData.IsLoading = false;
                    }
                });
            }
            else
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    StackPanel mainUserDetails =
                                (StackPanel)FindChildControl<StackPanel>(this, "mainUserDetails");

                    Loading networkingIssueConnDetails =
                          (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                    networkingIssueConnDetails.IsLoading = true;
                    mainUserDetails.DataContext = null;
                });
            }
        }

        private async void txtSearchUsers_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
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
                            StackPanel mainUserDetails =
                              (StackPanel)FindChildControl<StackPanel>(this, "mainUserDetails");
                            Loading loadingUserDetailsData =
                                (Loading)FindChildControl<Loading>(this, "loadingUserDetailsData");
                            Loading networkingIssueConnDetails =
                                (Loading)FindChildControl<Loading>(this, "networkingIssueConnDetails");

                            networkingIssueConnDetails.IsLoading = false;

                            loadingUserDetailsData.IsLoading = true;

                            int userId =
                              await UsersRepo.GetUserIdByNameAsync(sender.Text);

                            mainUserDetails.DataContext =
                                await UsersRepo.GetUserDetailsAsync(userId);

                            sender.ItemsSource = null;
                            //Don't implement it because it causes problems
                            ///usersMasterDetails.SelectedItem = null;

                            loadingUserDetailsData.IsLoading = false;
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

        private async void txtSearchUsers_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Down && e.Key != Windows.System.VirtualKey.Up)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                    {
                        AutoSuggestBox txtSearchUsers =
                             (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchUsers");
                        FontIcon networkIssueIcon =
                             (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");
                        TextBlock txtNetworkIssue =
                             (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                        if (!string.IsNullOrEmpty(txtSearchUsers.Text))
                        {
                            networkIssueIcon.Visibility = Visibility.Collapsed;
                            txtNetworkIssue.Visibility = Visibility.Collapsed;

                            txtSearchUsers.ItemsSource =
                                await UsersRepo.GetUsersSearchNamesAsync(txtSearchUsers.Text);
                        }
                        else
                        {
                            txtSearchUsers.ItemsSource = null;
                        }
                    }
                    else
                    {
                        AutoSuggestBox txtSearchUsers =
                                 (AutoSuggestBox)FindChildControl<AutoSuggestBox>(this, "txtSearchUsers");
                        FontIcon networkIssueIcon =
                                (FontIcon)FindChildControl<FontIcon>(this, "networkIssueIcon");
                        TextBlock txtNetworkIssue =
                           (TextBlock)FindChildControl<TextBlock>(this, "txtNetworkIssue");

                        txtSearchUsers.ItemsSource = null;

                        networkIssueIcon.Visibility = Visibility.Visible;
                        txtNetworkIssue.Visibility = Visibility.Visible;
                    }
                });

            }
        }
    }
}

