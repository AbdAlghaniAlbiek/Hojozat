using HojozatServer.Models;
using HojozatServer.Repositories.RemoteRepo;
using System;
using System.Collections.Generic;
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

namespace HojozatServer.Views.DeviceFamily_Desktop.HotelsViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HotelDetailsView : Page
    {
        public int HotelId { get; set; }
        public HotelDetails HotelDetails { get; set; }

        public HotelDetailsView()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                HotelId = (int)e.Parameter;
                await PageLoading();
            }
        }

        private async Task PageLoading()
        {
            loadingHotelDetailsData.IsLoading = true;

            HotelDetails =
                await HotelsRepo.GetHotelDetailsAsync(HotelId);

            this.DataContext = HotelDetails;

            hotelRoomsTypes.SelectedIndex = 0;

            loadingHotelDetailsData.IsLoading = false;
        }

        private void hotelPhotos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hotelPhotoRepresenter.Source = (string)hotelPhotos.SelectedItem;
        }

        private async void hotelRoomsTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string roomType = (string)hotelRoomsTypes.SelectedItem;

            RoomsTypeDetails.DataContext = await HotelsRepo.GetHotelRoomsTypeDetailsAsync(HotelDetails.HotelProfile.Id, roomType);
        }

        private void btnHotelDetailsBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
