using System;
using System.Collections.Generic;
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
using Windows.Networking.Connectivity;
using System.Collections.ObjectModel;
using HojozatServer.Repositories.RemoteRepo;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardView : Page
    {
        public ObservableCollection<Models.BrandProfile> NewBrands { get; set; }
        public ObservableCollection<Models.UserProfile> NewUsers { get; set; }

        public DashboardView()
        {
            this.InitializeComponent();

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            if (DashboardRepo.IsFirstConnection)
            {
                if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        loadingDashboardData.IsLoading = true;

                        mainDashboardStackPanel.DataContext =
                              await DashboardRepo.GetDashboardAsync();
                        DashboardRepo.Brand =
                              await DashboardRepo.GetDashboardLatestBrandAsync();
                        DashboardRepo.User =
                              await DashboardRepo.GetDashboardLatestUserAsync();

                        loadingDashboardData.IsLoading = false;

                        DashboardRepo.IsFirstConnection = false;
                    });

                }
                else
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        networkingIssueConnecting.IsLoading = true;
                    });
                }
            }
            else
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
               {
                   loadingDashboardData.IsLoading = true;

                   mainDashboardStackPanel.DataContext =
                         await DashboardRepo.GetDashboardAsync();

                   var latestBrand =
                         await DashboardRepo.GetDashboardLatestBrandAsync();

                   if (latestBrand.Id > DashboardRepo.Brand.Id)
                   {
                       NewBrands =
                          await DashboardRepo.GetDashboardLatestBrandsProfilesAsync();

                       DashboardRepo.Brand = latestBrand;

                       notifyNewBrand.Visibility = Visibility.Visible;
                   }

                   var latesUser =
                         await DashboardRepo.GetDashboardLatestUserAsync();

                   if (latesUser.Id > DashboardRepo.User.Id)
                   {
                       NewUsers =
                          await DashboardRepo.GetDashboardLatestUsersProfilesAsync();

                       DashboardRepo.User = latesUser;

                       notifyNewUser.Visibility = Visibility.Visible;
                   }

                   loadingDashboardData.IsLoading = false;
               });
            }
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (DashboardRepo.IsFirstConnection)
            {
                if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                    {
                        networkingIssueConnecting.IsLoading = false;

                        loadingDashboardData.IsLoading = true;

                        mainDashboardStackPanel.DataContext =
                             await DashboardRepo.GetDashboardAsync();
                        DashboardRepo.Brand =
                             await DashboardRepo.GetDashboardLatestBrandAsync();
                        DashboardRepo.User =
                             await DashboardRepo.GetDashboardLatestUserAsync();

                        loadingDashboardData.IsLoading = false;

                        UsersRepo.IsFirstConnection = false;
                    });
                }
                else
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        networkingIssueConnecting.IsLoading = true;
                    });
                }
            }
        }
      
        private async void newBrandsRefButt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    var animNewBrandsFontIcon = new Storyboard();
                    var doubleAnimation = new DoubleAnimation();
                    doubleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
                    doubleAnimation.EnableDependentAnimation = true;
                    doubleAnimation.To = 360;
                    Storyboard.SetTargetProperty(doubleAnimation, "Angle");
                    Storyboard.SetTarget(doubleAnimation, newBrandsRefreshFontIconAnim);
                    animNewBrandsFontIcon.Children.Add(doubleAnimation);
                    animNewBrandsFontIcon.Begin();


                    var latestBrand =
                          await DashboardRepo.GetDashboardLatestBrandAsync();

                    if (latestBrand.Id > DashboardRepo.Brand.Id)
                    {
                        NewBrands =
                           await DashboardRepo.GetDashboardLatestBrandsProfilesAsync();

                        DashboardRepo.Brand = latestBrand;

                        notifyNewBrand.Visibility = Visibility.Visible;

                        animNewBrandsFontIcon.Stop();
                    }
                    else
                    {
                        animNewBrandsFontIcon.Stop();

                        noNewBrandsInfoMessage.Show(5000);
                    }
                });
            }
            else
            {
                newMembersMessageNetworkIssue.Show(5000);
            }
        }

        private async void newUsersRefButt_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    var animNewUsersFontIcon = new Storyboard();
                    var doubleAnimation = new DoubleAnimation();
                    doubleAnimation.Duration = TimeSpan.FromMilliseconds(1000);
                    doubleAnimation.EnableDependentAnimation = true;
                    doubleAnimation.To = 360;
                    Storyboard.SetTargetProperty(doubleAnimation, "Angle");
                    Storyboard.SetTarget(doubleAnimation, newUsersRefreshFontIconAnim);
                    animNewUsersFontIcon.Children.Add(doubleAnimation);
                    animNewUsersFontIcon.Begin();

                    var latestUser =
                          await DashboardRepo.GetDashboardLatestUserAsync();

                    if (latestUser.Id > DashboardRepo.User.Id)
                    {
                        NewUsers =
                           await DashboardRepo.GetDashboardLatestUsersProfilesAsync();

                        DashboardRepo.User = latestUser;

                        notifyNewUser.Visibility = Visibility.Visible;

                        animNewUsersFontIcon.Stop();
                    }
                    else
                    {
                        animNewUsersFontIcon.Stop();

                        noNewUsersInfoMessage.Show(5000);
                    }
                });
            }
            else
            {
                newMembersMessageNetworkIssue.Show(5000);
            }
        }

        private void newBrandsUserPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (newBrandsUserPivot.SelectedIndex == 0)
            {
                notifyNewBrand.Visibility = Visibility.Collapsed;
            }
            else
            {
                notifyNewUser.Visibility = Visibility.Collapsed;
            }
        }
    }
}
