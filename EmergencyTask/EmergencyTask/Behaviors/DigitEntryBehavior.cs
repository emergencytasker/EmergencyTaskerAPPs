using Xamarin.Forms;

namespace EmergencyTask.Behaviors
{
    public class DigitEntryBehavior : Behavior<Entry>
    {
        public Entry PrevDigit { get; set; }
        public Entry NextDigit { get; set; }

        #region BindableProperty Completed
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty CompletedProperty = BindableProperty.Create("Completed", typeof(Command), typeof(DigitEntryBehavior), null, propertyChanged: OnCompletedPropertyChanged);
        private static void OnCompletedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is DigitEntryBehavior behavior)
            {
                if (newValue != null && newValue is Command command)
                {
                    behavior.Completed = command;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private Command completed;
        public Command Completed
        {
            get { return completed; }
            set { completed = value; OnPropertyChanged(); }
        }
        #endregion

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length > 1)
            {
                entry.TextChanged -= OnEntryTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnEntryTextChanged;
                string newChar = e.NewTextValue.Substring(1);
                if (NextDigit != null)
                    NextDigit.Text = newChar;
            }
            else if (entry.Text.Length > 0)
            {
                NextDigit?.Focus();
                Completed?.Execute(this);
            }
            else
            {
                PrevDigit?.Focus();
            }
        }
    }
}