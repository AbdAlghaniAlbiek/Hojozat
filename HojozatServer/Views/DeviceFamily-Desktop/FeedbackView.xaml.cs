using HojozatServer.Repositories.RemoteRepo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedbackView : Page
    {

        public ObservableCollection<Models.Feedback> UsersFeedback { get; set; }

        public ObservableCollection<Models.Feedback> BrandsFeedback { get; set; }

        public FeedbackView()
        {
            this.InitializeComponent();
        }


        private async void Page_Loading(FrameworkElement sender, object args)
        {
            loadingFeedbackData.IsLoading = true;

            UsersFeedback = await FeedbackRepo.GetUsersFeedbackAsync();
            BrandsFeedback = await FeedbackRepo.GetBrandsFeedbackAsync();

            loadingFeedbackData.IsLoading = false;

            usersFeedbackListView.ItemsSource = UsersFeedback;
            brandsFeedbackListView.ItemsSource = BrandsFeedback;

        }
    }
}
