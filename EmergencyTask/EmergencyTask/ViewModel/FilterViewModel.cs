using EmergencyTask.Model;
using System;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class FilterViewModel : ViewModelBase
    {

        #region BindableProperty BtnAplicar
        /// <summary>
        /// BtnAplicar de la propiedad bindable
        /// </summary>
        private Command btnaplicar;
        public Command BtnAplicar
        {
            get { return btnaplicar; }
            set { btnaplicar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Distancia
        /// <summary>
        /// Distancia
        /// </summary>
        private int distancia;
        public int Distancia
        {
            get { return distancia; }
            set { distancia = value; OnPropertyChanged(); if (Filter != null) { Filter.Distancia = value; } }
        }
        #endregion

        private Action<FilterModel> FilterResult { get; set; }

        #region Notified Property Filter
        /// <summary>
        /// Filter
        /// </summary>
        private FilterModel filter;
        public FilterModel Filter
        {
            get { return filter; }
            set { filter = value; OnPropertyChanged(); }
        }
        #endregion

        public FilterViewModel(Action<FilterModel> filterresult)
        {
            BtnAplicar = new Command(BtnAplicar_Clicked);
            FilterResult = filterresult;
            Filter = new FilterModel();
        }
        private void BtnAplicar_Clicked(object obj)
        {
            FilterResult?.Invoke(Filter);
        }
    }
}