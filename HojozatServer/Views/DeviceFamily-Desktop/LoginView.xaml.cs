using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Extensions;
using HojozatServer.Services.Security;
using HojozatServer.Repositories;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using System.Threading.Tasks;
using HojozatServer.Services;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {

            if (Settings.HasAnAccount)
            {
                //If serverUser have an account let's begin from login pivot
                MainPivot.SelectedIndex = 0;

                //To check there is connection to WiFi or not
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (!HasInternetAccess)
                {
                    loginErrorMessageNetworkIssue.Show(6000);
                }
            }
            else
            {
                //If serverUser doesn't have any account let's begin from register pivot
                MainPivot.SelectedIndex = 1;

                //To check there is connection to WiFi or not
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                bool HasInternetAccess = (connectionProfile != null &&
                                     connectionProfile.GetNetworkConnectivityLevel() ==
                                     NetworkConnectivityLevel.InternetAccess);

                if (!HasInternetAccess)
                {
                    registerErrorMessageNetworkIssue.Show(6000);
                }
            }
            
        }

        #region Login Process

        private void txtEmailLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginErrorIconEmail.Visibility = Visibility.Collapsed;
            loginErrorTextEmail.Visibility = Visibility.Collapsed;
        }
        public bool LoginCheckConstrains()
        {
            bool canPass = true;

            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);

            if (HasInternetAccess)
            {
                if (string.IsNullOrEmpty(txtEmailLogin.Text) || string.IsNullOrEmpty(txtPasswordLogin.Password))
                {
                    loginErrorMessageTextBoxesEmpty.Show(6000);
                    canPass = false;
                }

                if (!txtEmailLogin.Text.IsEmail())
                {
                    loginErrorIconEmail.Visibility = Visibility.Visible;
                    loginErrorTextEmail.Visibility = Visibility.Visible;
                    canPass = false;
                }
                else
                {
                    loginErrorIconEmail.Visibility = Visibility.Collapsed;
                    loginErrorTextEmail.Visibility = Visibility.Collapsed;
                }

                return canPass;
            }
            else
            {
                loginErrorMessageNetworkIssue.Show(6000);
                canPass = false;
                return canPass;
            }
        }
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            progLoginLoading.IsIndeterminate = true;

            if (LoginCheckConstrains())
            {
                bool result =
                    await Repositories.RemoteRepo.LoginRepo.LoginServerUserAsync(
                        new Models.ServerUser() 
                        {
                            Email = AESCryptography.Encrypt(txtEmailLogin.Text),
                            Password = AESCryptography.Encrypt(txtPasswordLogin.Password)
                        });

                //the process is successful (user has email and password in database)
                if (result)
                {
                    this.Frame.Navigate(typeof(Views.DeviceFamily_Desktop.MainView), null, new DrillInNavigationTransitionInfo());
                }
                //server user doesn't have an account in database
                else
                {
                    loginErrorMessage.Show(6000);
                }
            }

            progLoginLoading.IsIndeterminate = false;
        }


        #endregion


        #region Register process

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerErrorIconName.Visibility = Visibility.Collapsed;
            registerErrorTextName.Visibility = Visibility.Collapsed;
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerErrorIconEmail.Visibility = Visibility.Collapsed;
            registerErrorTextEmail.Visibility = Visibility.Collapsed;
        }
        private void txtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            registerErrorIconPhoneNumber.Visibility = Visibility.Collapsed;
            registerErrorTextPhoneNumber.Visibility = Visibility.Collapsed;
        }
        private bool RegisterCheckConstrains()
        {
            bool canPass = true;

            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);

            //Check if the user connect to internet
            if (HasInternetAccess)
            {

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) ||
                    string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtPhoneNumber.Text))
                {
                    registerErrorMessageTextBoxesEmpty.Show(6000);
                    canPass = false;
                }

                //I make this way beacause IsCharacterString() method if there is spaces ot tabs between words in 
                // txtName.Text then it return false state to me
                //to solve that I made this 'for' to split the text of txtName to subStrings and check each of them once by once
                string[] subTxtName = txtName.Text.Split(" ");

                for (int i = 0; i < subTxtName.Length; i++)
                {
                    if (subTxtName[i] != "")
                    {
                        //Check if there is Unvalid Name format
                        if (!subTxtName[i].IsCharacterString())
                        {
                            registerErrorIconName.Visibility = Visibility.Visible;
                            registerErrorTextName.Visibility = Visibility.Visible;
                            canPass = false;
                            break;
                        }
                    }
                    
                }

                //Check if there is Unvalid Email format
                if (!txtEmail.Text.IsEmail())
                {
                    registerErrorIconEmail.Visibility = Visibility.Visible;
                    registerErrorTextEmail.Visibility = Visibility.Visible;
                    canPass = false;
                }

                //Check if there is Unvalid Phone number format
                if (!txtPhoneNumber.Text.IsPhoneNumber())
                {
                    registerErrorIconPhoneNumber.Visibility = Visibility.Visible;
                    registerErrorTextPhoneNumber.Visibility = Visibility.Visible;
                    canPass = false;
                }

                return canPass;
            }
            else
            {
                registerErrorMessageNetworkIssue.Show(6000);
                canPass = false;
                return canPass;
            }
        }
        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            progRegisterLoading.IsIndeterminate = true;

            if (RegisterCheckConstrains())
            {

                var serverUser = new Models.ServerUser()
                {
                    Name = txtName.Text,
                    Email = AESCryptography.Encrypt(txtEmail.Text),
                    Password =  AESCryptography.Encrypt(txtPassword.Password),
                    PhoneNumber = txtPhoneNumber.Text,
                };

                bool result =
                   await Repositories.RemoteRepo.LoginRepo.RegisterServerUserAsync(serverUser);

                if (result)
                {
                    Settings.HasAnAccount = true;
                    MainPivot.SelectedIndex = 2;
                }
                else
                {
                    registerErrorMessageSomeOneHasSameEmail.Show(6000);
                }
            }

            progRegisterLoading.IsIndeterminate = false;

        }

        private bool VerifyrCheckConstrains()
        {

            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            bool HasInternetAccess = (connectionProfile != null &&
                                 connectionProfile.GetNetworkConnectivityLevel() ==
                                 NetworkConnectivityLevel.InternetAccess);

            //Check if the user connect to internet
            if (HasInternetAccess)
            {

                //Check if any textbox is empty and tell user to fill all of them
                if (string.IsNullOrEmpty(txtCode.Text))
                {
                    verifyErrorMessageTextBoxesEmpty.Show(6000);
                    return false;
                }

                return true;
            }
            else
            {
                verifyErrorMessageNetworkIssue.Show(6000);
                return false;
            }
        }
        private async void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            progVerifyLoading.IsIndeterminate = true;

            if (VerifyrCheckConstrains())
            {
                bool result =
                    await Repositories.RemoteRepo.LoginRepo.VerifyAccountAsync(txtCode.Text);

                //the account verified
                if (result)
                {
                    MainPivot.SelectedIndex = 3;
                }
                //the verify process failed so the code that user enter it isn't correct
                else
                {
                    verifyErrorMessageCodeIncorrect.Show(6000);
                }
            }

            progVerifyLoading.IsIndeterminate = false;

        }

        private void BtnRegSuccess_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.DeviceFamily_Desktop.MainView), null, new DrillInNavigationTransitionInfo());
        }

        #endregion 
    }
}
