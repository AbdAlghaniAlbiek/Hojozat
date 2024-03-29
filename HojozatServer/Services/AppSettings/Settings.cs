﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HojozatServer.Services
{
    public class Settings
    {

        private static ApplicationDataContainer _settings = 
            ApplicationData.Current.LocalSettings;

        public static bool HasAnAccount
        {
            get
            {
                return _settings.Values.ContainsKey("HasAnAccount") ? (bool)_settings.Values["HasAnAccount"] : false;
            }
            set
            {
                _settings.Values["HasAnAccount"] = value;
            }
        }

        public static string Theme
        {
            get
            {
               return _settings.Values.ContainsKey("Theme") ? _settings.Values["Theme"] as string : "System";
            }

            set
            {
                _settings.Values["Theme"] = value;
            }
        }

        public static bool Sound
        {
            get
            {
                return _settings.Values.ContainsKey("Sound") ? (bool)_settings.Values["Sound"] : false;
            }

            set
            {
                _settings.Values["Sound"] = value;
            }
        }

        public static bool SpatialAudio
        {
            get
            {
                return _settings.Values.ContainsKey("SpatialAudio") ? (bool)_settings.Values["SpatialAudio"] : false;
            }

            set
            {
                _settings.Values["SpatialAudio"] = value;
            }
        }
    }
}
