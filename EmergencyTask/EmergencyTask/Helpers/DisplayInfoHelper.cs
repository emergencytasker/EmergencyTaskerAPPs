using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EmergencyTask.Helpers
{
    public class DisplayInfoHelper
    {
        public DisplayInfo Info { get; private set; }

        public DisplayInfoHelper()
        {
            Info =  DeviceDisplay.MainDisplayInfo;
            // Subscribe to changes of screen metrics
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            // Process changes
            Info = e.DisplayInfo;
        }
    }
}
