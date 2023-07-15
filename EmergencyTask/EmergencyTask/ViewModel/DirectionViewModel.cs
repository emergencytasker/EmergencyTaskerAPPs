using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using GooglePlacesApi;
using GooglePlacesApi.Loggers;
using GooglePlacesApi.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Twilio.Rest.Api.V2010.Account;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Internals;

namespace EmergencyTask.ViewModel
{
    public class DirectionViewModel : ViewModelBase
    {

        #region BindableProperty Detail
        /// <summary>
        /// Detail de la propiedad bindable
        /// </summary>
        private string detail;
        public string Detail
        {
            get { return detail; }
            set { detail = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsMapChangeEnabled
        /// <summary>
        /// IsMapChangeEnabled de la propiedad bindable
        /// </summary>
        private bool ismapchangeenabled;
        public bool IsMapChangeEnabled
        {
            get { return ismapchangeenabled; }
            set { ismapchangeenabled = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty MapChangeCommand
        /// <summary>
        /// MapChangeCommand de la propiedad bindable
        /// </summary>
        private Command mapchangecommand;
        public Command MapChangeCommand
        {
            get { return mapchangecommand; }
            set { mapchangecommand = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Direccion
        /// <summary>
        /// Direccion de la propiedad bindable
        /// </summary>
        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnConfirmar
        /// <summary>
        /// BtnConfirmar de la propiedad bindable
        /// </summary>
        private Command btnconfirmar;
        public Command BtnConfirmar
        {
            get { return btnconfirmar; }
            set { btnconfirmar = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty GpsChangeCommand
        /// <summary>
        /// GpsChangeCommand de la propiedad bindable
        /// </summary>
        private Command gpschangecommand;
        public Command GpsChangeCommand
        {
            get { return gpschangecommand; }
            set { gpschangecommand = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsGpsEnabled
        /// <summary>
        /// IsGpsEnabled de la propiedad bindable
        /// </summary>
        private bool isgpsenabled;
        public bool IsGpsEnabled
        {
            get { return isgpsenabled; }
            set { isgpsenabled = value; OnPropertyChanged(); }
        }

        public ServiceModel Service { get; internal set; }
        #endregion

        public Map Mapa { get; set; }
        public double LatitudeService { get; set; }
        public double LongitudeService { get; set; }

        #region SearchLogic

        #region Notified Property SearchText
        /// <summary>
        /// SearchText
        /// </summary>
        private string searchtext;
        public string SearchText
        {
            get { return searchtext; }
            set { searchtext = value; OnPropertyChanged(); ListDirections(value); }
        }
        #endregion

        #region Notified Property SearchCommand
        /// <summary>
        /// SearchCommand
        /// </summary>
        private Command searchcommand;
        public Command SearchCommand
        {
            get { return searchcommand; }
            set { searchcommand = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsLocationsVisible
        /// <summary>
        /// IsLocationsVisible
        /// </summary>
        private bool islocationsvisible;
        public bool IsLocationsVisible
        {
            get { return islocationsvisible; }
            set { islocationsvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Locations
        /// <summary>
        /// Locations
        /// </summary>
        private ObservableCollection<LocationModel> locations;
        public ObservableCollection<LocationModel> Locations
        {
            get { return locations; }
            set { locations = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Location
        /// <summary>
        /// Location
        /// </summary>
        private LocationModel location;
        public LocationModel Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged(); if (Location != null) { LocationSelected(Location); } }
        }
        #endregion

        #endregion

        public string GoogleMapsKey { get; set; }
        public string GoogleLanguage { get; set; }
        public string GoogleCountry { get; set; }

        public DirectionViewModel ()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            Locations = new ObservableCollection<LocationModel>();
            BtnConfirmar = new Command(BtnConfirmar_Clicked);
            SearchCommand = new Command(SearchCommand_Clicked);
            MapChangeCommand = new Command(MapChangeCommand_Clicked);
            GpsChangeCommand = new Command(GpsChangeCommand_Clicked);
            IsMapChangeEnabled = false;
            IsGpsEnabled = true;
            GoogleMapsKey = await GetVar<string>("googlemapskey");
            GoogleLanguage = await GetVar<string>("googlelanguage");
            GoogleCountry = await GetVar<string>("googlecountry");
            LatitudeService = (double)App.Resources["Latitud"];
            LongitudeService = (double)App.Resources["Longitud"];
            SetMapPosition();
            GpsChangeCommand_Clicked(null);
            IsBusy = false;
        }

        public GooglePlacesApiService GooglePlacesApi { get; private set; }

        private async void LocationSelected(LocationModel location)
        {
            IsBusy = true;
            IsLocationsVisible = false;
            IsSearching = true;
            SearchText = location.Title;
            IsSearching = false;
            var details = await GooglePlacesApi.GetDetailsAsync(location.PlaceId).ConfigureAwait(false);
            if (details != null && details.Place != null && details.Place.Geometry != null && details.Place.Geometry.Location != null)
            {
                var direccion = location.Title;
                LatitudeService = details.Place.Geometry.Location.Latitude;
                LongitudeService = details.Place.Geometry.Location.Longitude;
                SetMapPosition(direccion);
            }
            else
            {
                Toast(AppResource.NoObtieneLugar);
            }
            IsBusy = false;
        }

        private void SearchCommand_Clicked(object obj)
        {
            ListDirections(SearchText);
        }

        public bool IsSearching { get; set; }
        private async void ListDirections(string searchtext)
        {
            if (IsSearching) return;
            try
            {
                IsSearching = true;
                if (searchtext != null) searchtext = searchtext.Trim().TrimStart().TrimEnd();
                if (string.IsNullOrEmpty(searchtext)) Locations = new ObservableCollection<LocationModel>();
                else
                {
                    var parts = searchtext.Split(' ');
                    if (searchtext.Length <= 3 && parts.Length == 0) Locations = new ObservableCollection<LocationModel>();
                    else
                    {
                        if (string.IsNullOrEmpty(GoogleMapsKey) || string.IsNullOrEmpty(GoogleLanguage) || string.IsNullOrEmpty(GoogleCountry))
                        {
                            IsSearching = false;
                            IsLocationsVisible = false;
                            return;
                        }

                        GooglePlacesApi = new GooglePlacesApiService(GoogleApiSettings.Builder.WithApiKey(GoogleMapsKey)
                            .WithLanguage(GoogleLanguage)
                            .WithType(PlaceTypes.Address)
                            .WithLogger(new ConsoleLogger())
                            .AddCountry(GoogleCountry)
                            .Build());

                        var result = await GooglePlacesApi.GetPredictionsAsync(searchtext).ConfigureAwait(false);
                        if (result == null || result.Status != "OK") Locations = new ObservableCollection<LocationModel>();
                        else
                        {
                            if (result.Items == null || result.Items.Count <= 0) Locations = new ObservableCollection<LocationModel>();
                            else
                            {
                                Locations = new ObservableCollection<LocationModel>(result.Items.Select(p => new LocationModel
                                {
                                    Title = p.Description,
                                    PlaceId = p.PlaceId
                                }));
                            }
                        }
                    }
                }
                IsLocationsVisible = Locations != null && Locations.Count() > 0;
                IsSearching = false;
            }
            catch
            {

            }
            finally
            {
                IsSearching = false;
            }
        }

        private async void SetMapPosition(string direction = "")
        {
            var pin = Mapa.Pins.FirstOrDefault();
            if (pin != null)
            {
                if (string.IsNullOrEmpty(direction))
                    direction = await GetAddressFromPosition(LatitudeService, LongitudeService);
                Direccion = direction;
                if (pin.Position.Latitude == LatitudeService && pin.Position.Longitude == LongitudeService)
                    return;
            }
            IsBusy = true;
            if (string.IsNullOrEmpty(direction))
                direction = await GetAddressFromPosition(LatitudeService, LongitudeService);
            Direccion = direction;
            Device.BeginInvokeOnMainThread(async () =>
            {
                Mapa.Pins.Clear();
                var position = new Position(LatitudeService, LongitudeService);
                var newpin = new Pin
                {
                    Address = IsGpsEnabled ? AppResource.MiUbicacion : (Direccion ?? "---"),
                    Position = position,
                    Label = "This pin is draggable",
                    IsDraggable = true,
                    Type = PinType.SearchResult
                };
                Mapa.Pins.Add(newpin);
                if (Mapa.Pins.Count > 0)
                {
                    var bounds = Bounds.FromPositions(Mapa.Pins.Select(s => s.Position));
                    Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.3)));
                }
            });
            IsBusy = false;
        }

        private async Task<string> GetAddressFromPosition(double latitudeService, double longitudeService)
        {
            var response = await Client.GetAddressByPoint(GoogleMapsKey, latitudeService, longitudeService);
            if (response == null) return string.Empty;
            return response.FirstOrDefault();
        }

        public void SetMapa(Map mapa)
        {
            IsBusy = true;
            Mapa = mapa;
            Mapa.PinDragStart += Mapa_PinDragStart;
            Mapa.PinDragEnd += Mapa_PinDragEnd;
            Mapa.PinDragging += Mapa_PinDragging;
            SetMapPosition();
            IsBusy = false;
        }

        private void Mapa_PinDragging(object sender, PinDragEventArgs e)
        {

        }

        private async void Mapa_PinDragEnd(object sender, PinDragEventArgs e)
        {
            LatitudeService = e.Pin.Position.Latitude;
            LongitudeService = e.Pin.Position.Longitude;
            Direccion = await GetAddressFromPosition(LatitudeService, LongitudeService);
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(LatitudeService, LongitudeService), Distance.FromKilometers(0.3)));
        }

        private void Mapa_PinDragStart(object sender, PinDragEventArgs e)
        {
            
        }

        private async void GpsChangeCommand_Clicked(object obj)
        {
            IsBusy = true;
            var gpsisenabled = false;
            if (await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location) == PermissionStatus.Granted)
                gpsisenabled = true;
            else
            {
                var response = (await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location));
                if (response.ContainsKey(Permission.Location))
                    gpsisenabled = response[Permission.Location] == PermissionStatus.Granted;
            }
            if (!gpsisenabled)
            {
                Toast(AppResource.PermisoGPS);
                return;
            }
            await GetLocation();
            LatitudeService = Latitud;
            LongitudeService = Longitud;
            SetMapPosition();
            IsGpsEnabled = true;
            IsMapChangeEnabled = false;
            IsBusy = false;
        }

        private void MapChangeCommand_Clicked(object obj)
        {
            IsMapChangeEnabled = true;
            IsGpsEnabled = false;
        }

        private async void BtnConfirmar_Clicked(object obj)
        {
            if(LatitudeService == 0.0D && LongitudeService == 0.0D)
            {
                Toast(AppResource.UbicacionNoValida);
                return;
            }
            Service.Direccion = Direccion;
            Service.Detalles = Detail;
            Service.Latitud = LatitudeService;
            Service.Longitud = LongitudeService;
            await Navigation.PushAsync(new DescriptionServicePage
            {
                BindingContext = new DescriptionServiceViewModel
                {
                    Service = Service
                }
            });
        }
    }
}