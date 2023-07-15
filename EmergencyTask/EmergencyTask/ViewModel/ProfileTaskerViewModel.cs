using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.Views.Rating;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ProfileTaskerViewModel : ViewModelBase
    {

        #region BindableProperty CVInfoColor
        /// <summary>
        /// CVInfoColor de la propiedad bindable
        /// </summary>
        private Color cvinfocolor = Color.Black;
        public Color CVInfoColor
        {
            get { return cvinfocolor; }
            set { cvinfocolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsHideSingle
        /// <summary>
        /// IsHideSingle
        /// </summary>
        private bool ishidesingle;
        public bool IsHideSingle
        {
            get { return ishidesingle; }
            set { ishidesingle = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty PersonalInfoColor
        /// <summary>
        /// PersonalInfoColor de la propiedad bindable
        /// </summary>
        private Color personalinfocolor = Color.Black;
        public Color PersonalInfoColor
        {
            get { return personalinfocolor; }
            set { personalinfocolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty SeviceInfoColor
        /// <summary>
        /// SeviceInfoColor de la propiedad bindable
        /// </summary>
        private Color serviceinfocolor = Color.Black;
        public Color ServiceInfoColor
        {
            get { return serviceinfocolor; }
            set { serviceinfocolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty GaleriaInfoColor
        /// <summary>
        /// GaleriaInfoColor de la propiedad bindable
        /// </summary>
        private Color galeriainfocolor;
        public Color GaleriaInfoColor
        {
            get { return galeriainfocolor; }
            set { galeriainfocolor = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Evidence
        /// <summary>
        /// Evidence de la propiedad bindable
        /// </summary>
        private EvidenceModel evidence;
        public EvidenceModel Evidence
        {
            get { return evidence; }
            set { evidence = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Evidences
        /// <summary>
        /// Evidences de la propiedad bindable
        /// </summary>
        private IList<EvidenceModel> evidences;
        public IList<EvidenceModel> Evidences
        {
            get { return evidences; }
            set { evidences = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty FotoAsistente
        /// <summary>
        /// FotoAsistente de la propiedad bindable
        /// </summary>
        private ImageSource fotoasistente;
        public ImageSource FotoAsistente
        {
            get { return fotoasistente; }
            set { fotoasistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NombreAsistente
        /// <summary>
        /// NombreAsistente de la propiedad bindable
        /// </summary>
        private string nombreasistente;
        public string NombreAsistente
        {
            get { return nombreasistente; }
            set { nombreasistente = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapPersonalInfo       
        /// <summary>
        /// TapPersonalInfo de la propiedad bindable
        /// </summary>
        private Command tappersonalinfo;
        public Command TapPersonalInfo
        {
            get { return tappersonalinfo; }
            set { tappersonalinfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapServiceInfo
        /// <summary>
        /// TapServiceInfo de la propiedad bindable
        /// </summary>
        private Command tapserviceinfo;
        public Command TapServiceInfo
        {
            get { return tapserviceinfo; }
            set { tapserviceinfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapCVInfo
        /// <summary>
        /// TapCVInfo de la propiedad bindable
        /// </summary>
        private Command tapcvinfo;
        public Command TapCVInfo
        {
            get { return tapcvinfo; }
            set { tapcvinfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapGaleriaInfo
        /// <summary>
        /// TapGaleriaInfo de la propiedad bindable
        /// </summary>
        private Command tapgaleriainfo;
        public Command TapGaleriaInfo
        {
            get { return tapgaleriainfo; }
            set { tapgaleriainfo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsPersonalInfoVisible
        /// <summary>
        /// IsPersonalInfoVisible de la propiedad bindable
        /// </summary>
        private bool ispersonalinfovisible;
        public bool IsPersonalInfoVisible
        {
            get { return ispersonalinfovisible; }
            set { ispersonalinfovisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty StadisticReview
        /// <summary>
        /// StadisticReview de la propiedad bindable
        /// </summary>
        private string stadisticreview;
        public string StadisticReview
        {
            get { return stadisticreview; }
            set { stadisticreview = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Description
        /// <summary>
        /// Description de la propiedad bindable
        /// </summary>
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Ubication
        /// <summary>
        /// Ubication de la propiedad bindable
        /// </summary>
        private string ubication;
        public string Ubication
        {
            get { return ubication; }
            set { ubication = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsServiceInfoVisible
        /// <summary>
        /// IsServiceInfoVisible de la propiedad bindable
        /// </summary>
        private bool isvisibleinfoservice;
        public bool IsServiceInfoVisible
        {
            get { return isvisibleinfoservice; }
            set { isvisibleinfoservice = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Carta
        /// <summary>
        /// Carta de la propiedad bindable
        /// </summary>
        private CartaModel carta;
        public CartaModel Carta
        {
            get { return carta; }
            set { carta = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Cartas
        /// <summary>
        /// Cartas de la propiedad bindable
        /// </summary>
        private IList<CartaModel> cartas;
        public IList<CartaModel> Cartas
        {
            get { return cartas; }
            set { cartas = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsCVInfoVisible
        /// <summary>
        /// IsCVInfoVisible de la propiedad bindable
        /// </summary>
        private bool iscvinfovisible;
        public bool IsCVInfoVisible
        {
            get { return iscvinfovisible; }
            set { iscvinfovisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsNotStudyInfoVisible
        /// <summary>
        /// IsNotStudyInfoVisible de la propiedad bindable
        /// </summary>
        private bool isnotstudyinfovisible;
        public bool IsNotStudyInfoVisible
        {
            get { return isnotstudyinfovisible; }
            set { isnotstudyinfovisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsStudyInfoVisible
        /// <summary>
        /// IsStudyInfoVisible de la propiedad bindable
        /// </summary>
        private bool isstudyinfovisible;
        public bool IsStudyInfoVisible
        {
            get { return isstudyinfovisible; }
            set { isstudyinfovisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Grado
        /// <summary>
        /// Grado de la propiedad bindable
        /// </summary>
        private string grado;
        public string Grado
        {
            get { return grado; }
            set { grado = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Institucion
        /// <summary>
        /// Institucion de la propiedad bindable
        /// </summary>
        private string institucion;
        public string Institucion
        {
            get { return institucion; }
            set { institucion = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Titulo
        /// <summary>
        /// Titulo de la propiedad bindable
        /// </summary>
        private string titulo;
        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsNotWorkInfoVisible
        /// <summary>
        /// IsNotWorkInfoVisible de la propiedad bindable
        /// </summary>
        private bool isnotworkinfovisible;
        public bool IsNotWorkInfoVisible
        {
            get { return isnotworkinfovisible; }
            set { isnotworkinfovisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsWorkInfoVisible
        /// <summary>
        /// IsWorkInfoVisible de la propiedad bindable
        /// </summary>
        private bool isworkinfovisible;
        public bool IsWorkInfoVisible
        {
            get { return isworkinfovisible; }
            set { isworkinfovisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Work
        /// <summary>
        /// Work de la propiedad bindable
        /// </summary>
        private WorkModel work;
        public WorkModel Work
        {
            get { return work; }
            set { work = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Works
        /// <summary>
        /// Works de la propiedad bindable
        /// </summary>
        private IList<WorkModel> works;
        public IList<WorkModel> Works
        {
            get { return works; }
            set { works = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty LabelDescription
        /// <summary>
        /// LabelDescription de la propiedad bindable
        /// </summary>
        private string userdescription;
        public string UserDescription
        {
            get { return userdescription; }
            set { userdescription = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsVisibleDescription
        /// <summary>
        /// IsVisibleDescription de la propiedad bindable
        /// </summary>
        private bool isvisibledescription;
        public bool IsVisibleDescription
        {
            get { return isvisibledescription; }
            set { isvisibledescription = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsVisibleEditor
        /// <summary>
        /// IsVisibleEditor de la propiedad bindable
        /// </summary>
        private bool isdescriptioneditable;
        public bool IsDescriptionEditable
        {
            get { return isdescriptioneditable; }
            set { isdescriptioneditable = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IdState
        /// <summary>
        /// IdState de la propiedad bindable
        /// </summary>
        private string idstate;
        public string IdState
        {
            get { return idstate; }
            set { idstate = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NSSState
        /// <summary>
        /// NSSState de la propiedad bindable
        /// </summary>
        private string nssstate;
        public string NSSState
        {
            get { return nssstate; }
            set { nssstate = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty CorreoState
        /// <summary>
        /// CorreoState de la propiedad bindable
        /// </summary>
        private string correostate;
        public string CorreoState
        {
            get { return correostate; }
            set { correostate = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty PhoneState
        /// <summary>
        /// PhoneState de la propiedad bindable
        /// </summary>
        private string phonestate;
        public string PhoneState
        {
            get { return phonestate; }
            set { phonestate = value; OnPropertyChanged(); }
        }
        #endregion
        
        #region API
        public IEnumerable<Category> Categorias { get; set; }
        public IEnumerable<Subcategory> Subcategorias { get; set; }
        private IEnumerable<Service> Services { get; set; }
        public long CompletedServices { get; private set; }
        public double Review { get; set; }
        public Study Study { get; set; }
        public IEnumerable<Work> CV { get; set; }
        #endregion

        #region Notified Property IsStarsVisible
        /// <summary>
        /// IsStarsVisible
        /// </summary>
        private bool isstarsvisible;
        public bool IsStarsVisible
        {
            get { return isstarsvisible; }
            set { isstarsvisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property GoToReviews
        /// <summary>
        /// GoToReviews
        /// </summary>
        private ICommand gotoreviews;
        public ICommand GoToReviews
        {
            get { return gotoreviews; }
            set { gotoreviews = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsGaleriaVisible
        /// <summary>
        /// IsGaleriaVisible de la propiedad bindable
        /// </summary>
        private bool isgaleriavisible;
        public bool IsGaleriaVisible
        {
            get { return isgaleriavisible; }
            set { isgaleriavisible = value; OnPropertyChanged(); }
        }
        #endregion

        public StarsReview Stars { get; set; }
        public int IdUsuario { get; set; }
        public User CurrentUser { get; set; }

        public ProfileTaskerViewModel(int idusuario)
        {
            IdUsuario = idusuario;
        }

        private async Task<bool> EnsureApiResponses()
        {
            var lang = Usuario.GetUserLogin()?.lenguaje ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var services = await Client.GetServices(lang);

            Categorias = services.Select(s => new Category
            {
                eliminado = 0,
                id = s.idcategoria,
                imagen = s.imagencategoria,
                nombre = s.categoria
            });

            Subcategorias = services.Select(s => new Subcategory
            {
                id = s.idsubcategoria,
                eliminado = 0,
                nombre = s.subcategoria,
                imagen = s.imagensubcategoria,
                idcategoria = s.idcategoria
            });

            if (CurrentUser != null)
            {
                if (Services == null)
                {
                    Services = await Client.Service.Where(new Service
                    {
                        idusuario = CurrentUser.id
                    });
                }

                // cantidad de servicios completados
                CompletedServices = await Client.GetCompletedServices(CurrentUser.id, CurrentUser.idperfil ?? 2);

                // obtener estadisticas del usuario
                Review = await Client.GetReview(CurrentUser.id);

                // obtener ultimos datos de estudy
                Study = (await Client.Study.Where(new Study
                {
                    idusuario = CurrentUser.id
                })).FirstOrDefault();

                // obter cv

                CV = (await Client.Work.Where(new Work
                {
                    idusuario = CurrentUser.id
                }));
            }

            if (Categorias == null || Subcategorias == null || Services == null || CV == null)
            {
                Toast(AppResource.SinInternet);
                return false;
            }

            return true;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            Cartas = new List<CartaModel>();
            Works = new List<WorkModel>();

            var usuario = CurrentUser = await Client.User.Get(IdUsuario);
            if (usuario == null) return;

            IdState = usuario.identificacionvalidada == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            NSSState = usuario.segurosocialvalidado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            PhoneState = usuario.telefonoverificado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            CorreoState = usuario.activado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];

            PersonalInfoColor = (Color)App.Resources["Accent"];

            FotoAsistente = Client.GetPath(usuario.imagen);
            NombreAsistente = usuario.nombre;
            UserDescription = usuario.descripcion;

            IsPersonalInfoVisible = true;
            IsNotStudyInfoVisible = false;
            IsStudyInfoVisible = true;
            IsNotWorkInfoVisible = false;
            IsWorkInfoVisible = true;
            IsGaleriaVisible = false;

            TapPersonalInfo = new Command(TapPersonalInfo_Clicked);
            TapServiceInfo = new Command(TapServiceInfo_Clicked);
            TapCVInfo = new Command(TapCVInfo_Clicked);
            TapGaleriaInfo = new Command(TapGaleriaInfo_Clicked);
            GoToReviews = new Command(GoToReviews_Command);

            if (await EnsureApiResponses())
            {
                var services = new List<CartaModel>();
                foreach (var service in Services)
                {
                    var carta = GetServiceModel(service);
                    if (carta == null) continue;
                    services.Add(carta);
                }

                // Cartas
                Cartas = new List<CartaModel>(services);

                // stats
                StadisticReview = $"{CompletedServices} {AppResource.Tareas}";
                Description = usuario.descripcion;

                // last study info
                Grado = Study?.grado ?? "";
                Titulo = Study?.titulo ?? "";
                Institucion = Study?.institucion ?? "";

                var works = new List<WorkModel>();
                foreach (var cv in CV)
                {
                    var model = GetWork(cv);
                    if (model == null) continue;
                    works.Add(model);
                }

                Works = new List<WorkModel>(works.OrderByDescending(w => w.FechaInicio).ThenBy(w => w.FechaFin));
                IsHideSingle = Works.Count > 1;

                if (Stars != null)
                {
                    Stars.Value = Review;
                    IsStarsVisible = true;
                }
            }

            var galery = await Client.Galery.Where(new Galery
            {
                idusuario = usuario.id
            });

            var source = new List<EvidenceModel>();
            foreach (var item in galery)
            {
                source.Add(new EvidenceModel
                {
                    Image = Client.GetPath(item.path),
                    Descripcion = item.descripcion,
                    TieneDescripcion = true,
                    Options = new Command(Options_Clicked)
                });
            }
            Evidences = new List<EvidenceModel>(source);
        }

        private async void Options_Clicked(object obj)
        {
            if (!(obj is EvidenceModel model)) return;
            var grid = new Grid
            {
                Margin = 60
            };
            grid.Children.Add(new Image
            {
                Source = model.Image,
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });
            grid.Children.Add(new Label
            {
                MaxLines = 3,
                Text = model.Descripcion,
                TextColor = Color.White,
                Padding = 12,
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            });
            var popup = new PopupPage
            {
                Content = grid
            };
            popup.BackgroundClicked += Popup_BackgroundClicked;
            await Navigation.PushPopupAsync(popup);
        }

        private async void Popup_BackgroundClicked(object sender, EventArgs e)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0) await PopupNavigation.Instance.PopAsync();
        }

        private async void GoToReviews_Command(object obj)
        {
            await Navigation.PushAsync(new ReviewListPage
            {
                BindingContext = new ReviewListViewModel(CurrentUser.id)
            });
        }

        public void SetStars(StarsReview stars)
        {
            Stars = stars;
            Stars.Value = Review;
            IsStarsVisible = true;
        }

        #region Trabajos
        /// <summary>4
        /// Devuelve un trabajo
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        private WorkModel GetWork(Work work)
        {
            var date = DateTime.Now.AddYears(-20);
            var enddate = work.fin.FromMySqlDateFormat();
            var startdate = work.inicio.FromMySqlDateFormat();
            if (enddate < date)
                enddate = new DateTime(date.Year, enddate.Month, enddate.Day);
            if (startdate < date)
                startdate = new DateTime(date.Year, startdate.Month, startdate.Day);
            return new WorkModel
            {
                Id = work.id,
                Descripcion = work.descripcion,
                Empresa = work.empresa,
                Fin = enddate,
                Inicio = startdate,
                Puesto = work.puesto
            };
        }

        private CartaModel GetServiceModel(Service service)
        {
            if (service == null) return null;
            if (Categorias == null) return null;
            if (Subcategorias == null) return null;
            var categoria = Categorias.FirstOrDefault(c => c.id == service.idcategoria);
            var subcategoria = Subcategorias.FirstOrDefault(c => c.id == service.idsubcategoria);

            if (categoria == null) return null;
            if (subcategoria == null) return null;

            return new CartaModel
            {
                Precio = service.costo,
                Costo = service.costo.ToString(),
                Id = service.id,
                Image = Client.GetPath(subcategoria.imagen),
                Title = categoria.nombre,
                Subtitle = subcategoria.nombre
            };
        }
        #endregion

        #region Menu [Info, Service, CV]
        private void TapCVInfo_Clicked(object obj)
        {
            CVInfoColor = (Color)App.Resources["Accent"];
            PersonalInfoColor = Color.Black;
            ServiceInfoColor = Color.Black;
            GaleriaInfoColor = Color.Black;
            IsCVInfoVisible = true;
            IsPersonalInfoVisible = false;
            IsServiceInfoVisible = false;
            IsGaleriaVisible = false;
        }

        private void TapServiceInfo_Clicked(object obj)
        {
            CVInfoColor = Color.Black;
            PersonalInfoColor = Color.Black;
            ServiceInfoColor = (Color)App.Resources["Accent"];
            GaleriaInfoColor = Color.Black;
            IsCVInfoVisible = false;
            IsPersonalInfoVisible = false;
            IsServiceInfoVisible = true;
            IsGaleriaVisible = false;
        }


        private void TapGaleriaInfo_Clicked(object obj)
        {
            CVInfoColor = Color.Black;
            PersonalInfoColor = Color.Black;
            ServiceInfoColor = Color.Black;
            GaleriaInfoColor = (Color)App.Resources["Accent"];
            IsCVInfoVisible = false;
            IsPersonalInfoVisible = false;
            IsServiceInfoVisible = false;
            IsGaleriaVisible = true;
        }

        private void TapPersonalInfo_Clicked(object obj)
        {
            CVInfoColor = Color.Black;
            PersonalInfoColor = (Color)App.Resources["Accent"];
            ServiceInfoColor = Color.Black;
            GaleriaInfoColor = Color.Black;
            IsCVInfoVisible = false;
            IsPersonalInfoVisible = true;
            IsServiceInfoVisible = false;
            IsGaleriaVisible = false;
        }
        #endregion

    }
}
