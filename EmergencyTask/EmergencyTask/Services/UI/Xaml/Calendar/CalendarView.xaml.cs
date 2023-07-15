using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace Plugin.UI.Xaml.Calendar
{
	public partial class CalendarView : ContentView
	{

        #region BindableProperty DateSelected
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty DateSelectedProperty = BindableProperty.Create("DateSelected", typeof(DateTime), typeof(CalendarView), DateTime.Now, propertyChanged: OnDateSelectedPropertyChanged);
        private static void OnDateSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view && newValue != null && newValue is DateTime time)
            {
                view.DateSelected = time;
                view.SetDate(time, view.Culture);
            }
        }

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
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty CultureProperty = BindableProperty.Create("Culture", typeof(CultureInfo), typeof(CalendarView), CultureInfo.CurrentCulture, propertyChanged: OnCulturePropertyChanged);
        private static void OnCulturePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view)
            {
                if (newValue != null)
                {
                    view.Culture = (CultureInfo) newValue;
                }
            }
        }

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
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty CanSelectProperty = BindableProperty.Create("CanSelect", typeof(bool), typeof(CalendarView), false, propertyChanged: OnCanSelectPropertyChanged);
        private static void OnCanSelectPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view)
            {
                if (newValue != null)
                {
                    if(bool.TryParse(newValue.ToString(), out bool selected))
                    {
                        view.CanSelect = selected;
                    }
                }
            }
        }

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

        #region BindableProperty IsRestricted
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty IsRestrictedProperty = BindableProperty.Create("IsRestricted", typeof(bool), typeof(CalendarView), false, propertyChanged: OnIsRestrictedPropertyChanged);
        private static void OnIsRestrictedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view && newValue != null && newValue is bool value)
            {
                view.IsRestricted = value;
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private bool isrestricted;
        public bool IsRestricted
        {
            get { return isrestricted; }
            set { isrestricted = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Events
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty EventsProperty = BindableProperty.Create("Events", typeof(IEnumerable<IEvent>), typeof(CalendarView), new List<IEvent>(), propertyChanged: OnEventsPropertyChanged);
        private static void OnEventsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view && newValue != null && newValue is IEnumerable<IEvent> list)
            {
                view.Events = list;
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private IEnumerable<IEvent> events;
        public IEnumerable<IEvent> Events
        {
            get { return events; }
            set
            {
                if (value is ObservableCollection<IEvent> obscollection)
                {
                    obscollection.CollectionChanged += Obscollection_CollectionChanged;
                }
                events = value;
                OnPropertyChanged();
                SetDate(DateSelected, Culture, true);
            }
        }

        private void Obscollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetDate(DateSelected, Culture, true, true);
        }
        #endregion

        #region BindableProperty HeaderTextColor
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create("HeaderTextColor", typeof(Color), typeof(CalendarView), Color.Black, propertyChanged: OnHeaderTextColorPropertyChanged);
        private static void OnHeaderTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view)
            {
                if (newValue != null)
                {
                    view.HeaderTextColor = (Color) newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Color headertextcolor;
        public Color HeaderTextColor
        {
            get { return headertextcolor; }
            set { headertextcolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty DayTextColor
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty DayTextColorProperty = BindableProperty.Create("DayTextColor", typeof(Color), typeof(CalendarView), Color.Black, propertyChanged: OnDayTextColorPropertyChanged);
        private static void OnDayTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view)
            {
                if (newValue != null)
                {
                    view.DayTextColor = (Color) newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Color daytextcolor;
        public Color DayTextColor
        {
            get { return daytextcolor; }
            set { daytextcolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SelectedDayTextColor
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty SelectedDayTextColorProperty = BindableProperty.Create("SelectedDayTextColor", typeof(Color), typeof(CalendarView), Color.Accent, propertyChanged: OnSelectedDayTextColorPropertyChanged);
        private static void OnSelectedDayTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view)
            {
                if (newValue != null)
                {
                    view.SelectedDayTextColor = (Color) newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Color selecteddaytextcolor;
        public Color SelectedDayTextColor
        {
            get { return selecteddaytextcolor; }
            set { selecteddaytextcolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Sunday
        /// <summary>
        /// Sunday de la propiedad bindable
        /// </summary>
        private string sunday = "Sun";
        public string Sunday
        {
            get { return sunday; }
            set { sunday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Monday
        /// <summary>
        /// Monday de la propiedad bindable
        /// </summary>
        private string monday = "Mon";
        public string Monday
        {
            get { return monday; }
            set { monday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Tuesday
        /// <summary>
        /// Tuesday de la propiedad bindable
        /// </summary>
        private string tuesday = "Tue";
        public string Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Wednesday
        /// <summary>
        /// Wednesday de la propiedad bindable
        /// </summary>
        private string wednesday = "Wed";
        public string Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Thursday
        /// <summary>
        /// Thursday de la propiedad bindable
        /// </summary>
        private string thursday = "Thu";
        public string Thursday
        {
            get { return thursday; }
            set { thursday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Friday
        /// <summary>
        /// Friday de la propiedad bindable
        /// </summary>
        private string friday = "Fri";
        public string Friday
        {
            get { return friday; }
            set { friday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Saturday
        /// <summary>
        /// Saturday de la propiedad bindable
        /// </summary>
        private string saturday = "Sat";
        public string Saturday
        {
            get { return saturday; }
            set { saturday = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BgColor
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty BgColorProperty = BindableProperty.Create("BgColor", typeof(Color), typeof(CalendarView), Color.White, propertyChanged: OnBgColorPropertyChanged);
        private static void OnBgColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarView view)
            {
                if (newValue != null)
                {
                    view.BgColor = (Color) newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Color bgcolor = Color.White;
        public Color BgColor
        {
            get { return bgcolor; }
            set { bgcolor = value; OnPropertyChanged(); }
        }
        #endregion

        private List<DayView> DayView { get; set; }
        public Command Up { get; set; }
        public Command Down { get; set; }
        public Dictionary<int, ListView> ListViews { get; set; }

        public event EventHandler<CalendarDay> ItemSelected;
        public event EventHandler<IEvent> EventSelected;
        public event EventHandler<int> LoadingMonth;
        public event EventHandler<int> LoadedMonth;

        public CalendarView ()
		{
			InitializeComponent ();
            ListViews = new Dictionary<int, ListView>
            {
                { 1, ListView1 },
                { 3, ListView3 },
                { 5, ListView5 },
                { 7, ListView7 },
                { 9, ListView9 },
                { 11, ListView11 },
            };
            DayView = new List<DayView>
            {
                _1,_2,_3,_4,_5,_6,_7,_8,_9,_10,_11,_12,_13,_14,_15,_16,_17,_18,_19,_20,_21,_22,_23,_24,_25,_26,_27,_28,_29,_30,_31,_32,_33,_34,_35,_36,_37,_38,_39,_40,_41,_42
            };
            foreach (var dayview in DayView)
            {
                dayview.Tapped += Dayview_Tapped;
            }
            DateSelected = DateTime.Now;
            Up = new Command(Up_Command);
            Down = new Command(Down_Command);
            Culture = Thread.CurrentThread.CurrentUICulture;
            SetDate(DateSelected, Culture);
            BindingContext = this;
        }

        private void Down_Command(object obj)
        {
            var now = DateTime.Now;
            var date = DateSelected.AddMonths(-1);
            if (IsRestricted)
            {
                if (now.Month <= date.Month)
                {
                    SetDate(date, Culture, date.Year == DateSelected.Year && date.Month == DateSelected.Month && date.Day == DateSelected.Day);
                }
            }
            else
            {
                SetDate(date, Culture, date.Year == DateSelected.Year && date.Month == DateSelected.Month && date.Day == DateSelected.Day);
            }
        }

        private void Up_Command(object obj)
        {
            var date = DateSelected.AddMonths(1);
            SetDate(date, Culture, date.Year == DateSelected.Year && date.Month == DateSelected.Month && date.Day == DateSelected.Day);
        }

        public void SetDate(DateTime time, CultureInfo info, bool printday = true, bool listvisible = false)
        {
            LoadingMonth?.Invoke(this, time.Month);

            foreach (var listview in ListViews)
            {
                listview.Value.ItemsSource = new List<IEvent>();
                listview.Value.ItemSelected -= Listview_ItemSelected;
                listview.Value.ItemTapped -= Listview_ItemTapped;
                listview.Value.IsVisible = listview.Value.IsVisible ? listvisible : false;
            }

            DateSelected = time;

            DateLabel.Text = time.ToString("MMM/yyyy", info);

            DateTime initdate = new DateTime(time.Year, time.Month, 1);
            var daysinmonth = DateTime.DaysInMonth(time.Year, time.Month);
            DateTime enddate = new DateTime(time.Year, time.Month, daysinmonth);

            var daysleft = GetDaysLeft(initdate);

            var index = 0;
            var lefttime = initdate.AddDays(daysleft * -1);
            for (var loopdate = lefttime; loopdate < initdate; loopdate = loopdate.AddDays(1))
            {
                SetDayView(index, loopdate);
                index++;
            }

            for (var loopdate = initdate; loopdate <= enddate; loopdate = loopdate.AddDays(1))
            {
                SetDayView(index, loopdate, true, time, printday);
                index++;

            }

            var daysinview = daysleft + daysinmonth;
            var lastdays = 42 - daysinview;

            var lastdayinview = enddate.AddDays(lastdays + 1);

            for (var loopdate = enddate.AddDays(1); loopdate < lastdayinview; loopdate = loopdate.AddDays(1))
            {
                SetDayView(index, loopdate);
                index++;
            }

            LoadedMonth?.Invoke(this, time.Month);
        }

        private int GetDaysLeft(DateTime initdate)
        {
            int daysleft = 0;
            switch (initdate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    daysleft = 0;
                    break;

                case DayOfWeek.Tuesday:
                    daysleft = 1;
                    break;

                case DayOfWeek.Wednesday:
                    daysleft = 2;
                    break;

                case DayOfWeek.Thursday:
                    daysleft = 3;
                    break;

                case DayOfWeek.Friday:
                    daysleft = 4;
                    break;

                case DayOfWeek.Saturday:
                    daysleft = 5;
                    break;

                case DayOfWeek.Sunday:
                    daysleft = 6;
                    break;
            }
            return daysleft;
        }

        private void SetDayView(int index, DateTime timeset, bool iscurrentmonth = false, DateTime? setdate = null, bool printday = true)
        {
            System.Diagnostics.Debug.WriteLine(timeset, "CalendarView");
            if (index >= DayView.Count) return;
            var dayview = DayView[index];
            dayview.TextColor = DayTextColor;
            dayview.SelectColor = SelectedDayTextColor;
            if (Events != null)
            {
                var eventsinday = Events.Where(e => e.StartDate.Year == timeset.Year && e.StartDate.Month == timeset.Month && e.StartDate.Day == timeset.Day);
                if (eventsinday.Count() > 0)
                {
                    dayview.Events = eventsinday.ToList();
                    if (dayview.Date == DateSelected)
                    {
                        var row = Grid.GetRow(dayview);
                        if (ListViews.ContainsKey(row))
                        {
                            ListViews[row].ItemsSource = dayview.Events;
                        }
                    }
                }
                else
                {
                    dayview.Events = new List<IEvent>();
                }
            }
            else
            {
                dayview.Events = new List<IEvent>();
            }

            dayview.SetDay(timeset, iscurrentmonth, setdate, printday, IsRestricted);
        }

        private void Dayview_Tapped(object sender, DayView e)
        {
            System.Diagnostics.Debug.WriteLine(e.Date, "CalendarView");
            DateSelected = e.Date;
            if (DayView != null)
            {
                foreach (var dayview in DayView)
                {
                    dayview.UnSelect(DayTextColor);
                }
                e.Select(SelectedDayTextColor);
            }
            ShowListView(sender as DayView);
            if (CanSelect) ItemSelected?.Invoke(this, e.Day);
        }

        private DayView Last;
        private void ShowListView(DayView day)
        {
            if (day == null) return;
            var row = Grid.GetRow(day);
            foreach (var item in ListViews)
            {
                if (item.Key == row)
                {
                    item.Value.ItemSelected -= Listview_ItemSelected;
                    item.Value.ItemTapped -= Listview_ItemTapped;
                    continue;
                }
                item.Value.IsVisible = false;
                item.Value.ItemsSource = new List<IEvent>();
                item.Value.ItemSelected -= Listview_ItemSelected;
                item.Value.ItemTapped -= Listview_ItemTapped;
            }

            if (ListViews.ContainsKey(row))
            {
                var listview = ListViews[row];
                listview.ItemsSource = day.Events;
                listview.ItemSelected += Listview_ItemSelected;
                listview.ItemTapped += Listview_ItemTapped;
                if (Last == null) listview.IsVisible = true;
                else if (Last.Id == day.Id) listview.IsVisible = !listview.IsVisible;
                else listview.IsVisible = true;
                Last = day;
            }
        }

        private void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var ievent = e.Item as IEvent;
            EventSelected?.Invoke(this, ievent);
        }

        private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}