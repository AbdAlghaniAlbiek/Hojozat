using HojozatServer.Repositories.RemoteRepo;
using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Helpers;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop.HotelsViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelsView : Page
    {

        public string HostType { get; set; }

        public HotelsView()
        {
            this.InitializeComponent();
        }

        private void hotelsTypesContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void mainHotelsHostTypesNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }

            string navItemTag = ((NavigationViewItem)sender.SelectedItem).Tag.ToString();

            if (navItemTag == "NormalHotels")
            {
                HostType = "Normal";
                hotelsTypesContentFrame.NavigateToType(typeof(HotelsNormalView), HostType, navOptions);
            }
            else if (navItemTag == "MediumHotels")
            {
                HostType = "Medium";
                hotelsTypesContentFrame.NavigateToType(typeof(HotelsMediumView), HostType, navOptions);
            }
            else if (navItemTag == "SuperHotels")
            {
                HostType = "Super";
                hotelsTypesContentFrame.NavigateToType(typeof(HotelsSuperView), HostType, navOptions);
            }
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            HostType = "Normal";
            hotelsTypesContentFrame.Navigate(typeof(HotelsNormalView), "Normal",new EntranceNavigationTransitionInfo());
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
                            FrameNavigationOptions navOptions = new FrameNavigationOptions();
                            navOptions.TransitionInfoOverride = new DrillInNavigationTransitionInfo();
                            if (mainHotelsHostTypesNavigation.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
                            {
                                navOptions.IsNavigationStackEnabled = false;
                            }
                
                            int hotelId =
                              await HotelsRepo.GetHotelIdByNameAsync(sender.Text);

                            hotelsTypesContentFrame.NavigateToType(typeof(HotelDetailsView), hotelId, navOptions);
                        }
                        else
                        {
                            sender.Text = "";
                            sender.ItemsSource = null;
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
                        if (!string.IsNullOrEmpty(txtSearchHotels.Text))
                        { 
                            txtSearchHotels.ItemsSource =
                                await HotelsRepo.GetHotelsSearchNamesAsync(HostType, txtSearchHotels.Text);
                        }
                        else
                        {
                            txtSearchHotels.ItemsSource = null;
                        }
                    }
                    else
                    {
                        txtSearchHotels.ItemsSource = null;
                    }
                });
            }
        }
    }
}
