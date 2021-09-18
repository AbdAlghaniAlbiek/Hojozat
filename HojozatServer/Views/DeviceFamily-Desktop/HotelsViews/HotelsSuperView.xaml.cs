using HojozatServer.Models;
using HojozatServer.Repositories.RemoteRepo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop.HotelsViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsSuperView : Page
    {
        public ObservableCollection<Models.HotelsCollectedByBrand> HotelsCollected { get; set; }
        public bool FirstLoading { get; set; } = true;

        public string HostType { get; set; }

        public HotelsSuperView()
        {
            this.InitializeComponent();

            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                HostType = e.Parameter.ToString();

                await PageLoading();
            }
        }

        private async Task PageLoading()
        {
            try
            {
                if (HotelsRepo.IsFirstConnection)
                {
                    var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                    bool HasInternetAccess = (connectionProfile != null &&
                                         connectionProfile.GetNetworkConnectivityLevel() ==
                                         NetworkConnectivityLevel.InternetAccess);

                    if (HasInternetAccess)
                    {
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            loadingNormalHotelsData.IsLoading = true;

                            HotelsCollected = await HotelsRepo.GetHotelsCollectedByBrandAsync(HostType);

                            ((CollectionViewSource)this.Resources["HotelsCollection"]).Source = HotelsCollected;

                            loadingNormalHotelsData.IsLoading = false;

                            HotelsRepo.IsFirstConnection = false;
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
                        loadingNormalHotelsData.IsLoading = true;

                        HotelsCollected = await HotelsRepo.GetHotelsCollectedByBrandAsync(HostType);

                        loadingNormalHotelsData.IsLoading = false;

                    });
                }
            }
            catch (Exception)
            {

            }
        
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (HotelsRepo.IsFirstConnection)
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

                        loadingNormalHotelsData.IsLoading = true;

                        HotelsCollected = await HotelsRepo.GetHotelsCollectedByBrandAsync(HostType);

                        ((CollectionViewSource)this.Resources["HotelsCollection"]).Source = HotelsCollected;

                        loadingNormalHotelsData.IsLoading = false;

                        HotelsRepo.IsFirstConnection = false;
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
        }

        private async void normalHotelsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!FirstLoading)
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (HasInternetAccess)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        if (superHotelsGridView.SelectedItem != null)
                        {
                            this.Frame.Navigate(typeof(HotelsViews.HotelDetailsView),
                                ((Hotel)superHotelsGridView.SelectedItem).Id,
                                new DrillInNavigationTransitionInfo());
                        }
                    });
                }
                else
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        networkIssueToast.Show(6000);
                    });
                }
            }

            //Very very important don't remove it
            FirstLoading = false;

            superHotelsGridView.SelectedItem = null;

        }
    }
}
