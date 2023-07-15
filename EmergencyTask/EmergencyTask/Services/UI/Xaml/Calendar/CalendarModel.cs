using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Plugin.UI.Xaml.Calendar
{
    public class CalendarModel : INotifyPropertyChanged
    {

        #region BindableProperty DateSelected
        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private DateTime dateselected;
        public DateTime DateSelected
        {
            get { return dateselected; }
            set { dateselected = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Culture
        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private CultureInfo culture;
        public CultureInfo Culture
        {
            get { return culture; }
            set { culture = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CanSelect
        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private bool canselect;
        public bool CanSelect
        {
            get { return canselect; }
            set { canselect = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Events
        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private IEnumerable<IEvent> events;
        public IEnumerable<IEvent> Events
        {
            get { return events; }
            set
            {
                events = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BindableProperty SelectedItem
        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private CalendarDay selecteditem;
        public CalendarDay SelectedItem
        {
            get
            {
                return selecteditem;
            }

            set
            {
                selecteditem = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}