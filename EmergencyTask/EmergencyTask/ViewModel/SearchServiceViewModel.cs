using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class SearchServiceViewModel : ViewModelBase
    {

        #region Notified Property Subcategorias
        /// <summary>
        /// Subcategorias
        /// </summary>
        private ObservableCollection<ServiceListModel> services;
        public ObservableCollection<ServiceListModel> Services
        {
            get { return services; }
            set { services = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Search
        /// <summary>
        /// Search
        /// </summary>
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged(); OnSearchChanged(value); }
        }
        #endregion

        #region Notified Property Service
        /// <summary>
        /// Service
        /// </summary>
        private CartaModel service;
        public CartaModel Service
        {
            get { return service; }
            set { service = value; OnPropertyChanged(); if (value != null) { OnServiceSelected(value); } }
        }
        #endregion

        #region Notified Property Refresh
        /// <summary>
        /// Refresh
        /// </summary>
        private Command refresh;
        public Command Refresh
        {
            get { return refresh; }
            set { refresh = value; OnPropertyChanged(); }
        }

        public DateTime CurrentDate { get; private set; }
        #endregion

        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<Subcategory> Subcategories { get; private set; }
        public IEnumerable<API.Response.Service> ServicesInDB { get; set; }

        public SearchServiceViewModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            await Load();
            Refresh = new Command(async () => await Refresh_Command());
        }

        private async Task Load()
        {
            IsBusy = true;

            var date = await Client.GetDate();
            if(date == null)
            {
                IsBusy = false;
                Toast(AppResource.ErrorBuscar);
                return;
            }

            CurrentDate = date.Value;

            var lenguaje = Usuario.GetUserLogin()?.lenguaje ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            ServicesInDB = await Client.GetServices(lenguaje);

            if (ServicesInDB == null || ServicesInDB.Count() == 0)
            {
                IsBusy = false;
                return;
            }

            Categories = ServicesInDB.GroupBy(s => s.idcategoria).Select(s => new Category
            {
                eliminado = 0,
                id = s.Key,
                imagen = s.ToList().FirstOrDefault()?.imagencategoria ?? "",
                nombre = s.ToList().FirstOrDefault()?.categoria ?? ""
            });

            Subcategories = ServicesInDB.Select(s => new Subcategory
            {
                eliminado = 0,
                id = s.idsubcategoria,
                imagen = s.imagensubcategoria,
                nombre = s.subcategoria,
                idcategoria = s.idcategoria,
                horarios = s.horarios ?? new List<API.ER.Schedule>(0)
            });

            SetSource(Filter(""));

            IsBusy = false;
        }

        private async Task Refresh_Command()
        {
            await Load();
        }

        private async void OnServiceSelected(CartaModel value)
        {
            var category = Categories.FirstOrDefault(c => c.id == value.IdCategory);
            if (category == null)
            {
                return;
            }
            await Navigation.PushAsync(new DirectionPage
            {
                BindingContext = new DirectionViewModel
                {
                    Service = new ServiceModel
                    {
                        IdCategoria = value.IdCategory,
                        Category = category.nombre,
                        IdSubcategory = value.Id,
                        SubCategory = value.Title
                    }
                }
            });
        }

        private void SetSource(List<ServiceListModel> items)
        {
            if (items == null) return;
            if (Services == null)
            {
                Services = new ObservableCollection<ServiceListModel>(items);
            }
            else
            {
                if (!Services.SequenceEqual(items, new ServiceListModelEqualityComparer()))
                {
                    Services = new ObservableCollection<ServiceListModel>(items);
                }
            }
        }

        private void OnSearchChanged(string value)
        {
            SetSource(Filter(value));
        }

        private List<ServiceListModel> Filter(string texttosearch)
        {
            Debug.WriteLine($"[Filter] {texttosearch}");
            if (Categories == null || Subcategories == null) return new List<ServiceListModel>();
            var items = new List<ServiceListModel>();
            foreach (var category in Categories.OrderBy(c => c.nombre))
            {
                var title = category.nombre;
                if (string.IsNullOrEmpty(category.nombre)) title = AppResource.Otros;
                var list = new ServiceListModel(title, title.FirstOrDefault().ToString(), Client.GetPath(category.imagen));
                IEnumerable<Subcategory> source = new List<Subcategory>();
                if (string.IsNullOrEmpty(texttosearch))
                {
                    source = Subcategories.Where(s => s.idcategoria == category.id);
                }
                else
                {
                    source = Subcategories.Where(s => s.idcategoria == category.id && s.nombre.ToLower().Contains(texttosearch.ToLower()));
                }
                
                foreach (var item in source.OrderBy(s => s.nombre))
                {
                    var tarifa1 = item.horarios.FirstOrDefault(h => h.tipo == ScheduleType.Day.ToString())?.costo ?? 100;
                    var tarifa2 = item.horarios.FirstOrDefault(h => h.tipo == ScheduleType.Night.ToString())?.costo ?? 200;
                    CostCalculator costcalculator = new CostCalculator();
                    var costo = costcalculator.Calculate(CurrentDate, CurrentDate.AddHours(8), tarifa1, tarifa2).Max(c => c.Cost);

                    var trabajadores = ServicesInDB.FirstOrDefault(s => s.idsubcategoria == item.id)?.taskers ?? 0;

                    if (trabajadores == 0) continue;

                    var taskers = $"{trabajadores} {AppResource.Taskers}";

                    list.Add(new CartaModel
                    {
                        Title = item.nombre,
                        Image = Client.GetPath(item.imagen),
                        Precio = item.costo,
                        Costo = item.costo.ToString(),
                        Id = item.id,
                        IdCategory = category.id,
                        Taskers = taskers
                    });
                }

                if (list.Count > 0)
                {
                    items.Add(list);
                }
            }
            return items;
        }
    }
}