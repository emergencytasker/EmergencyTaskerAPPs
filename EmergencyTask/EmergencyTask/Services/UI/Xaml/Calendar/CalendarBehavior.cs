using Plugin.UI.Xaml.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Plugin.UI.Xaml.Calendar
{
    public class CalendarBehavior : Behavior<CalendarView>, ICalendarBehavior
    {

        #region BindableProperty Source
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty EventsProperty = BindableProperty.Create("Events", typeof(ObservableCollection<IEvent>), typeof(CalendarBehavior), null, propertyChanged: OnSourcePropertyChanged);
        private static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarBehavior view && newValue != null && newValue is ObservableCollection<IEvent> source)
            {
                view.Events = source;
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private ObservableCollection<IEvent> events;
        public ObservableCollection<IEvent> Events
        {
            get { return events; }
            set { events = value; OnPropertyChanged(); }
        }
        #endregion
        
        #region BindableProperty SelectedItem
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create("SelectedDate", typeof(Command<DateTime>), typeof(CalendarBehavior), null, propertyChanged: OnSelectedItemPropertyChanged);
        private static void OnSelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarBehavior view && newValue != null && newValue is Command<DateTime> command)
            {
                view.SelectedDate = command;
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Command<DateTime> selecteddate;
        public Command<DateTime> SelectedDate
        {
            get { return selecteddate; }
            set { selecteddate = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SelectedEvent
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty SelectedEventProperty = BindableProperty.Create("SelectedEvent", typeof(Command<IEvent>), typeof(CalendarBehavior), null, propertyChanged: OnSelectedEventPropertyChanged);
        private static void OnSelectedEventPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarBehavior view && newValue != null && newValue is Command<IEvent> ievent)
            {
                view.SelectedEvent = ievent;
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Command<IEvent> selectedevent;
        public Command<IEvent> SelectedEvent
        {
            get { return selectedevent; }
            set { selectedevent = value; OnPropertyChanged(); }
        }
        #endregion


        protected override void OnAttachedTo(CalendarView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Events = Events;
            bindable.LoadingMonth += Bindable_LoadingMonth;
            bindable.LoadedMonth += Bindable_LoadedMonth;
            bindable.ItemSelected += Bindable_ItemSelected;
            bindable.EventSelected += Bindable_EventSelected;
        }

        private void Bindable_EventSelected(object sender, IEvent e)
        {
            SelectedEvent?.Execute(e);
        }

        private void Bindable_ItemSelected(object sender, CalendarDay e)
        {
            SelectedDate?.Execute(e.Date);
        }

        private void Bindable_LoadedMonth(object sender, int e)
        {

        }

        private void Bindable_LoadingMonth(object sender, int e)
        {

        }

        protected override void OnDetachingFrom(CalendarView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Events = null;
            bindable.LoadingMonth -= Bindable_LoadingMonth;
            bindable.LoadedMonth -= Bindable_LoadedMonth;
            bindable.ItemSelected -= Bindable_ItemSelected;
            bindable.EventSelected -= Bindable_EventSelected;
        }

    }
}