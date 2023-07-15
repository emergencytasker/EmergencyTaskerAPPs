﻿using Android.App;
using Android.OS;
using Plugin.Notification;
using System.Collections.Generic;

namespace Plugin.Droid.Notification
{
    [Activity(Theme = "@style/Theme.Splash")]
    public class ActivityNotification : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var bundle = Intent.Extras;
            var keyvalue = bundle.GetString("keyvalue", "");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(keyvalue))
            {
                var keyvalues = keyvalue.Split(",");
                if(keyvalues.Length > 0)
                {
                    for (int i = 0; i < keyvalues.Length; i += 2)
                    {
                        var key = keyvalues[i];
                        var value = keyvalues[i + 1];
                        parameters.Add(key, value);
                    }   
                }
            }
            LocalNotification.OnOpen(parameters);
            Finish();
        }
    }
}