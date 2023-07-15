using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.UI.Xaml.Calendar
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DayView : Frame
	{

        public DateTime Date { get; set; }
        public CalendarDay Day { get; set; }

        private IEnumerable<IEvent> _events;
        public IEnumerable<IEvent> Events
        {
            get
            {
                return _events;
            }

            set
            {
                _events = value;
                if (_events is ObservableCollection<IEvent> obscollection)
                {
                    obscollection.CollectionChanged += Obscollection_CollectionChanged;
                }

                SetEvents();
            }
        }

        /// <summary>
        /// La lista de eventos a cambiado para el dia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Obscollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetEvents();
        }

        private void SetEvents()
        {
            if (_events != null)
            {
                if (_events.Count() > 0)
                {
                    for (int i = 0; i < _events.Count(); i++)
                    {
                        if (i < EventsColor.Count)
                        {
                            var _event = _events.ElementAtOrDefault(i);
                            if (_event != null)
                            {
                                if (_event.Color != null)
                                {
                                    EventsColor[i].Color = _event.Color;
                                }

                                EventsColor[i].IsVisible = true;
                            }
                            else
                            {
                                EventsColor[i].IsVisible = false;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var _event in EventsColor)
                    {
                        _event.IsVisible = false;
                    }
                    
                }
            }
            else
            {
                foreach (var _event in EventsColor)
                {
                    _event.IsVisible = false;
                }
            }

            if(Day != null)
            {
                Day.Events = _events;
            }
        }

        public List<BoxView> EventsColor { get; set; }
        public Color TextColor { get; set; }
        public Color SelectColor { get; set; }
        public bool IsRestricted { get; set; }

        public DayView ()
		{
			InitializeComponent ();

            IsVisible = false;

            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(DaySelected)
            });

            EventsColor = new List<BoxView>
            {
                EventOne, EventTwo, EventThree, EventFour
            };
        }

        public event EventHandler<DayView> Tapped;

        private void DaySelected(object obj)
        {
            if (IsEnabled)
            {
                Tapped?.Invoke(this, this);
            }
        }

        public void SetDay(DateTime date, bool iscurrentmonth, DateTime? setdate = null, bool printday = true, bool isrestricted = false)
        {
            IsRestricted = isrestricted;
            LabelDay.Text = "";
            Date = date;
            LabelDay.Text = Date.Day.ToString();
            IsEnabled = true;

            if (!iscurrentmonth)
            {
                IsVisible = false;
                LabelDay.TextColor = Color.LightCyan;
                LabelDay.FontAttributes = FontAttributes.Italic;    
            }
            else
            {
                IsVisible = true;
                LabelDay.TextColor = TextColor != null ? TextColor : Color.Black;
                LabelDay.FontAttributes = FontAttributes.Bold;
            }

            if (setdate.HasValue)
            {
                if (printday)
                {
                    if (setdate.Value.Year == date.Year &&
                        setdate.Value.Month == date.Month &&
                        setdate.Value.Day == date.Day)
                    {
                        LabelDay.TextColor = SelectColor != null ? SelectColor : Color.Accent;
                    }
                    else
                    {
                        LabelDay.TextColor = TextColor != null ? TextColor : Color.Black;
                    }
                }
                else
                {
                    LabelDay.TextColor = TextColor != null ? TextColor : Color.Black;
                }
            }
            else
            {
                LabelDay.TextColor = TextColor != null ? TextColor : Color.Black;
            }

            Day = new CalendarDay
            {
                Date = date,
                Events = _events
            };

            if (isrestricted)
            {
                var now = DateTime.Now;
                if (Date.Day < now.Day && date.Month == now.Month && date.Year == now.Year)
                {
                    IsEnabled = false;
                    LabelDay.TextColor = Color.FromHex("#eeeeee");
                }
            }
        }

        public void Select(Color color)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                if (IsRestricted)
                {
                    var now = DateTime.Now;
                    if (Date.Day < now.Day && Date.Month == now.Month && Date.Year == now.Year)
                    {
                        IsEnabled = false;
                        LabelDay.TextColor = Color.FromHex("#eeeeee");
                    }
                    else
                    {
                        LabelDay.TextColor = color != null ? color : Color.Accent;
                    }
                }
                else
                {
                    LabelDay.TextColor = color != null ? color : Color.Accent;
                }
            });
        }

        public void UnSelect(Color color)
        {
            if (IsRestricted)
            {
                var now = DateTime.Now;
                if (Date.Day < now.Day && Date.Month == now.Month && Date.Year == now.Year)
                {
                    IsEnabled = false;
                    LabelDay.TextColor = Color.FromHex("#eeeeee");
                }
                else
                {
                    LabelDay.TextColor = color != null ? color : Color.Black;
                }
            }
            else
            {
                LabelDay.TextColor = color != null ? color : Color.Black;
            }
        }
    }
}