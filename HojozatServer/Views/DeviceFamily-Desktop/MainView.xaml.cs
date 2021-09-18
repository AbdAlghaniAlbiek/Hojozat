using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.System.Profile;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page,INotifyPropertyChanged
    {

        //This property to give the btnNavMenu the right text when some click on navigation buttons
        private string _titlePageName = "Dashboard";
        public string TitlePageName
        {
            get { return this._titlePageName; }
            set
            {
                this._titlePageName = value;
                OnPropertyChanged();
            }
        }


        //To sense if someone clicks on navigation buttons and rewrite the text of  txtTitlePage
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string pageName = null)
        {
            PropertyChanged?.Invoke(PropertyChanged, new PropertyChangedEventArgs(pageName));
        }



        //Field contains the name of pages that I want ot navigate to it
        private readonly List<string> _pageName =
            new List<string>()
            {
                "Dashboard",
                "Globalization",
                "Brands",
                "Users",
                "Payments",
                "Analytics",
                "Settings",
                "Feedback",
                "Hotels"
            };


        public MainView()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Initializing the altLeft and goBack keyboardAccelerator
        /// call windows size changed event
        /// Make mainContentFrame navigate to Dashboard page
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //To determin what the back button that must be visible
            Window.Current.SizeChanged += WindowDimentionChangedEvent;

            //Go back button in mobile devices like phone and tablet
            KeyboardAccelerator goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += BackRequested;
            this.KeyboardAccelerators.Add(goBack);

            //when the user press on alt + left button to go back to previous page
            KeyboardAccelerator altLeft = new KeyboardAccelerator()
            {
                Key = Windows.System.VirtualKey.Left,
                Modifiers = Windows.System.VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackRequested;
            this.KeyboardAccelerators.Add(altLeft);

            //when this application get started doesn't hold any page
            //let's make it hold the dashboard page
            mainContentFramNavigation("Dashboard");
        }


        #region Display BackButton and control in the text of txtPageNav

        /// <summary>
        /// This event discuss when and which back button must be visible when mainContentFrame navigate from
        /// to new or previous page
        /// </summary>
        private void MainContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //first the mainContentFrame must have at least two pages to navigate 
            //to the previous page
            if (mainContentFrame.CanGoBack)
            {
                //In mobile mode
                if (Window.Current.Bounds.Width <= 640)
                {
                    btnBackMobileMode.Visibility = Visibility.Visible;
                    btnBack.Visibility = Visibility.Collapsed;
                }
                //In tablet and desktop mode
                else
                {
                    btnBackMobileMode.Visibility = Visibility.Collapsed;
                    btnBack.Visibility = Visibility.Visible;
                }
            }
        }


        /// <summary>
        /// This event discuss when and which back button must be visible when the window size change
        /// </summary>
        private void WindowDimentionChangedEvent(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            //first the mainContentFrame must have at least two pages to navigate 
            //to the previous page
            if (mainContentFrame.CanGoBack)
            {
                //In mobile mode
                if (e.Size.Width <= 640)
                {
                    btnBackMobileMode.Visibility = Visibility.Visible;
                    btnBack.Visibility = Visibility.Collapsed;
                }
                //In tablet and desktop mode
                else
                {
                    btnBackMobileMode.Visibility = Visibility.Collapsed;
                    btnBack.Visibility = Visibility.Visible;
                }
            }
        }

       
        /// <summary>
        /// This event for open the pane of splitView if it was close and vice versa
        /// And discuss the text of txtPageName states in desktop & tablet mode and in mobile mode 
        /// </summary>
        private void BtnNavMenu_Click(object sender, RoutedEventArgs e)
        {
            if (splitViewMainNav.IsPaneOpen)
            {
                splitViewMainNav.IsPaneOpen = false;
                if (Window.Current.Bounds.Width > 641)
                {
                    txtProjectName.Text = "H";
                    txtProjectName.Margin = new Thickness(2, 72, 0, 0);
                }

            }
            else
            {
                splitViewMainNav.IsPaneOpen = true;
                if (Window.Current.Bounds.Width > 641)
                {
                    txtProjectName.Text = "Hojozat";
                    txtProjectName.Margin = new Thickness(44, 20, 0, 0);
                }
                else
                {

                    txtProjectName.Text = "Hojozat";
                    txtProjectName.Margin = new Thickness(44, 20, 0, 0);
                }
            }
        }
        
       
        /// <summary>
        /// I created this event to solve the problem of text of txtPageName because in 
        /// tablet mode I can't close the pane by btnNavMenu because the pane is above it
        /// In this case the text in txtPageName will not back to right place so this event solve 
        /// this problem
        /// </summary>
        private void SplitViewMainNav_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            if (Window.Current.Bounds.Width > 641)
            {
                txtProjectName.Text = "H";
                txtProjectName.Margin = new Thickness(2, 72, 0, 0);
            }
        }

        #endregion


        #region Back navigation events

        /// <summary>
        /// when alt + left button or Go Back button are clicked
        /// </summary>
        private void BackRequested(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            OnBackRequested();
            args.Handled = true;
        }

        /// <summary>
        /// Back button click event in tablet and desktop mode
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            OnBackRequested();
        }

        /// <summary>
        /// Back button click event in mobile mode
        /// </summary>
        private void BtnBackMobileMode_Click(object sender, RoutedEventArgs e)
        {
            OnBackRequested();
        }

        /// <summary>
        /// Discuess when you can back to previous page  
        /// </summary>
        private void OnBackRequested()
        {
            //If it's in tablet or mobile mode and splite view was opened
            //close the split view and don't do any thing
            if (splitViewMainNav.IsPaneOpen && (splitViewMainNav.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                                                splitViewMainNav.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                splitViewMainNav.IsPaneOpen = false;
            }
            //if you are in table,mobile,desktop mode and split view wasn't opened
            //and you can go back -> go back now
            else
            {
                if (mainContentFrame.CanGoBack)
                {
                    mainContentFrame.GoBack();

                    if (!mainContentFrame.CanGoBack)
                    {
                        btnBack.Visibility = Visibility.Collapsed;
                        btnBackMobileMode.Visibility = Visibility.Collapsed;
                    }
                }
                
            }
        }

        #endregion


        #region Split View buttons navigation

        /// <summary>
        /// Make the frame navigate to the target page by send the page name to it
        /// </summary>
        /// <param name="pageName">The name of page that you want ot navigate to it</param>
        private void mainContentFramNavigation(string pageName)
        {

            //the frame contains a page or Dashboard page when the app is launching
            if (mainContentFrame.Content != null)
            {
                //If the you clicked on dashboard button and the frame doesn't display the dashboard page
                if (this._pageName[0] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.DashboardView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.DashboardView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }

                //If the you clicked on Hotels button and the frame doesn't display the Hotels page
                else if (this._pageName[1] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.GlobalizationView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.GlobalizationView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }

                //If the you clicked on Brands button and the frame doesn't display the Brands page
                else if (this._pageName[2] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.BrandsView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.BrandsView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }

                //If the you clicked on Users button and the frame doesn't display the Users page
                else if (this._pageName[3] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.UsersView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.UsersView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }

                //If the you clicked on Payments button and the frame doesn't display the payments page
                //else if (this._pageName[4] == pageName &&
                //    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.PaymentsView)))
                //{
                //    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.PaymentsView),
                //        null, new EntranceNavigationTransitionInfo());

                //    //In tablet and mobile mode and the pane of splitview is open -> close it
                //    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                //    {
                //        splitViewMainNav.IsPaneOpen = false;
                //    }
                //}

                ////If the you clicked on Analytics button and the frame doesn't display the Analytics page
                //else if (this._pageName[5] == pageName &&
                //    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.AnalyticsView)))
                //{
                //    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.AnalyticsView),
                //        null, new EntranceNavigationTransitionInfo());

                //    //In tablet and mobile mode and the pane of splitview is open -> close it
                //    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                //    {
                //        splitViewMainNav.IsPaneOpen = false;
                //    }
                //}

                //If the you clicked on Settings button and the frame doesn't display the Settings page
                else if (this._pageName[6] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.SettingsView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.SettingsView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }

                //If the you clicked on Feedback button and the frame doesn't display the Feedback page
                else if (this._pageName[7] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.FeedbackView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.FeedbackView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }

                //If the you clicked on Feedback button and the frame doesn't display the Feedback page
                else if (this._pageName[8] == pageName &&
                    !mainContentFrame.Content.GetType().Equals(typeof(Views.DeviceFamily_Desktop.HotelsViews.HotelsView)))
                {
                    mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.HotelsViews.HotelsView),
                        null, new EntranceNavigationTransitionInfo());

                    //In tablet and mobile mode and the pane of splitview is open -> close it
                    if (Window.Current.Bounds.Width <= 1007 && splitViewMainNav.IsPaneOpen)
                    {
                        splitViewMainNav.IsPaneOpen = false;
                    }
                }
            }

            //At launch the app doesn't have any page in the frame so it will hold dashboard page as 
            //a default page
            else
            {
                mainContentFrame.Navigate(typeof(Views.DeviceFamily_Desktop.DashboardView),
                    null, new EntranceNavigationTransitionInfo());
            }

        }

        /// <summary>
        /// Buttons that send page name to mainContentFrameNavigation method to navigate
        /// to the target page
        /// </summary>
        private void BtnNavDashboard_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Dashboard");
            txtTitlePage.Text = "Dashboard";
            //TitlePageName = "Dashboard";
        }

        private void BtnNavGlobalization_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Globalization");
            txtTitlePage.Text = "Globalization";
            //TitlePageName = "Globalization";
        }

        private void BtnNavBrands_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Brands");
            txtTitlePage.Text = "Brands";
            //TitlePageName = "Brands";
        }

        private void BtnNavUsers_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Users");
            txtTitlePage.Text = "Users";
            //TitlePageName = "Users";
        }

        private void BtnNavPayments_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Payments");
            txtTitlePage.Text = "Payments";
            //TitlePageName = "Payments";
        }

        private void BtnNavAnalytics_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Analytics");
            txtTitlePage.Text = "Analytics";
            //TitlePageName = "Analytics";
        }

        private void BtnNavSettings_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Settings");
            txtTitlePage.Text = "Settings";
            //TitlePageName = "Settings";
        }

        private void BtnNavFeedback_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Feedback");
            txtTitlePage.Text = "Feedback";
            //TitlePageName = "Feedback";
        }

        private void BtnNavHotels_Click(object sender, RoutedEventArgs e)
        {
            mainContentFramNavigation("Hotels");
            txtTitlePage.Text = "Hotels";
            //TitlePageName = "Hotels";
        }

        #endregion

        private async void btnNavExit_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await new ContentDialog()
            {
                Title = "Exit",
                Content = "Do you want to close Hojozat app",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                Background = (SolidColorBrush)this.Resources["DialogBackgroundSolidColorBrush"],
                BorderBrush = (SolidColorBrush)this.Resources["DialogBorderSolidColorBrush"],
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(4),
                PrimaryButtonStyle = (Style)App.Current.Resources["NotMainButtonDialogStyle"],
                CloseButtonStyle = (Style)App.Current.Resources["MainButtonDialogStyle"],
            }.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Windows.ApplicationModel.Core.CoreApplication.Exit();
            }
        }
    }
}
