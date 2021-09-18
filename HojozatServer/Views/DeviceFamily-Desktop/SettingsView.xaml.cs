using HojozatServer.Repositories.RemoteRepo;
using HojozatServer.Services;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HojozatServer.Views.DeviceFamily_Desktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();
        }

        private async void Page_Loading(FrameworkElement sender, object args)
        {
            loadingServerUsersData.IsLoading = true;

            //For loading profile server user data
            this.DataContext = await SettingsRepo.GetServerUserAsync();

            //Loading theme that server user choose it
            if (Settings.Theme == "Light")
            {
                radbtnLightTheme.IsChecked = true;
            }
            else if (Settings.Theme == "Dark")
            {
                radbtnDarkTheme.IsChecked = true;
            }
            else
            {
                radbtnSystemTheme.IsChecked = true;
            }

            //Loading Sound and special audio state for all controls
            if (Settings.Sound)
            {
                togSwitchSound.IsOn = true;
                checkboxSpacialAudio.IsEnabled = true;
                ElementSoundPlayer.State = ElementSoundPlayerState.On;

                if (Settings.SpatialAudio)
                {
                    checkboxSpacialAudio.IsChecked = true;
                }
            }
            else
            {
                togSwitchSound.IsOn = false;
                checkboxSpacialAudio.IsEnabled = false;
                checkboxSpacialAudio.IsChecked = false;

                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }

            //Languages detects
            if (Windows.Globalization.Language.CurrentInputMethodLanguageTag == "en-US")
            {
                radbtnEnglish.IsChecked = true;
            }
            else if (Windows.Globalization.Language.CurrentInputMethodLanguageTag == "ar-SY")
            {
                radbtnArabic.IsChecked = true;
            }

            loadingServerUsersData.IsLoading = false;

        }

        private void togbtnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (togbtnShowPassword.IsChecked == true)
            {
                serverUserPassword.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                serverUserPassword.PasswordRevealMode = PasswordRevealMode.Peek;
            }
        }

        private void radbtnSystemTheme_Click(object sender, RoutedEventArgs e)
        {
            string radbtnContent = ((RadioButton)sender).Content.ToString();

            if (radbtnContent == "Use system theme")
            {
                Settings.Theme = "System";
            }
            else if (radbtnContent == "Light")
            {
                Settings.Theme = "Light";
            }
            else
            {
                Settings.Theme = "Dark";
            }
        }

        private void togSwitchSound_Toggled(object sender, RoutedEventArgs e)
        {
            if (togSwitchSound.IsOn == true)
            {
                checkboxSpacialAudio.IsEnabled = true;
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
                Settings.Sound = true;
            }
            else
            {
                checkboxSpacialAudio.IsEnabled = false;
                checkboxSpacialAudio.IsChecked = false;

                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
                Settings.Sound = false;
            }
        }

        private void checkboxSpacialAudio_Click(object sender, RoutedEventArgs e)
        {
            if (checkboxSpacialAudio.IsChecked == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
                Settings.SpatialAudio = true;
            }
            else
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
                Settings.SpatialAudio = false;
            }
        }
    }
}
