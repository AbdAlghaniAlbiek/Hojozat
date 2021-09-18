using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.UI.Notifications;
using HojozatServer.Background;
using HojozatServer.Models;

namespace HojozatServer.Background
{
    public sealed class DashboardBackground:IBackgroundTask
    {
        BackgroundTaskDeferral _deferral; // Note: defined at class scope so that we can mark it complete inside the OnCancel() callback if we choose to support cancellation
       
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //if (NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            //{
            //    if (Repositories.RemoteRepo.DashboardRepo.Brand == null ||
            //        Repositories.RemoteRepo.DashboardRepo.User == null)
            //    {
            //        //We use this _deferral to make async code run (if we don't have it may make exception)
            //        _deferral = taskInstance.GetDeferral();

            //        Repositories.RemoteRepo.DashboardRepo.Brand =
            //            await Repositories.RemoteRepo.DashboardRepo.GetDashboardLatestBrandAsync();

            //        Repositories.RemoteRepo.DashboardRepo.User =
            //            await Repositories.RemoteRepo.DashboardRepo.GetDashboardLatestUserAsync();

            //        _deferral.Complete();
            //    }
            //    else
            //    {
            //        _deferral = taskInstance.GetDeferral();

            //        var latestBrand =
            //            await Repositories.RemoteRepo.DashboardRepo.GetDashboardLatestBrandAsync();

            //        if (latestBrand.Id != Repositories.RemoteRepo.DashboardRepo.Brand.Id)
            //        {
            //            Repositories.RemoteRepo.DashboardRepo.Brand = latestBrand;
            //            NewBrandRegisteredNotification();
            //        }

            //        var latestUser =
            //            await Repositories.RemoteRepo.DashboardRepo.GetDashboardLatestUserAsync();

            //        if (latestUser.Id != Repositories.RemoteRepo.DashboardRepo.User.Id)
            //        {
            //            Repositories.RemoteRepo.DashboardRepo.User = latestUser;
            //            NewUserRegisteredNotification();
            //        }

            //        _deferral.Complete();
            //    }
            //}
            
        }

        private void NewBrandRegisteredNotification()
        {
            //ToastContent newBrandToast = new ToastContent()
            //{
            //    Visual = new ToastVisual()
            //    {
            //        BindingGeneric = new ToastBindingGeneric()
            //        {
            //            Children =
            //            {
            //                new AdaptiveText()
            //                {
            //                    Text = Repositories.RemoteRepo.DashboardRepo.Brand.Name,
            //                    HintMaxLines = 1
            //                },
            //                new AdaptiveText()
            //                {
            //                    Text = "A New Member in Hojozat system"
            //                },
            //            },
            //            AppLogoOverride =
            //            {
            //                Source = Repositories.RemoteRepo.DashboardRepo.Brand.ImagePath,
            //                HintCrop = ToastGenericAppLogoCrop.Circle
            //            }
            //        }
            //    },
            //    DisplayTimestamp = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
            //};

            //var toast = new ToastNotification(newBrandToast.GetXml());
            //toast.ExpirationTime = DateTime.Now.AddDays(2);

            //Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void NewUserRegisteredNotification()
        {
            //    ToastContent newBrandToast = new ToastContent()
            //    {
            //        Visual = new ToastVisual()
            //        {
            //            BindingGeneric = new ToastBindingGeneric()
            //            {
            //                Children =
            //                {
            //                    new AdaptiveText()
            //                    {
            //                        Text = Repositories.RemoteRepo.DashboardRepo.User.Name,
            //                        HintMaxLines = 1
            //                    },
            //                    new AdaptiveText()
            //                    {
            //                        Text = "A New Member in Hojozat system"
            //                    },
            //                },
            //                AppLogoOverride =
            //                {
            //                    Source = Repositories.RemoteRepo.DashboardRepo.Brand.ImagePath,
            //                    HintCrop = ToastGenericAppLogoCrop.Circle
            //                }
            //            }
            //        },
            //        DisplayTimestamp = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
            //    };

            //    var toast = new ToastNotification(newBrandToast.GetXml());
            //    toast.ExpirationTime = DateTime.Now.AddDays(2);

            //    Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
