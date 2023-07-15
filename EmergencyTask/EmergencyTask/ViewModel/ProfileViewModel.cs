using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Helpers;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.View;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Views.Rating;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Stripe;
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
    public class ProfileViewModel : ViewModelBase
    {

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
        private ObservableCollection<EvidenceModel> evidences;
        public ObservableCollection<EvidenceModel> Evidences
        {
            get { return evidences; }
            set { evidences = value; OnPropertyChanged(); }
        }

        public List<EvidenceModel> Source { get; set; }
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

        #region BindableProperty BtnEditDocuments
        /// <summary>
        /// BtnEditDocuments de la propiedad bindable
        /// </summary>
        private Command btneditdocuments;

        public Command BtnEditDocuments
        {
            get { return btneditdocuments; }
            set { btneditdocuments = value; OnPropertyChanged(); }
        }
        #endregion

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
        private Color galeriainfocolor = Color.Black;
        public Color GaleriaInfoColor
        {
            get { return galeriainfocolor; }
            set { galeriainfocolor = value; OnPropertyChanged(); }
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
        private ObservableCollection<CartaModel> cartas;
        public ObservableCollection<CartaModel> Cartas
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

        #region BindableProperty BtnEditStudy
        /// <summary>
        /// BtnEdit de la propiedad bindable
        /// </summary>
        private Command btneditstudy;
        public Command BtnEditStudy
        {
            get { return btneditstudy; }
            set { btneditstudy = value; OnPropertyChanged(); }
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

        #region BindableProperty BtnEditWork
        /// <summary>
        /// BtnEditWork de la propiedad bindable
        /// </summary>
        private Command btneditwork;
        public Command BtnEditWork
        {
            get { return btneditwork; }
            set { btneditwork = value; OnPropertyChanged(); }
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
        public List<WorkModel> WorksSource { get; set; }
        /// <summary>
        /// Works de la propiedad bindable
        /// </summary>
        private ObservableCollection<WorkModel> works;
        public ObservableCollection<WorkModel> Works
        {
            get { return works; }
            set { works = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty IsBtnAgregarServiceVisible
        /// <summary>
        /// IsBtnAgregarServiceVisible de la propiedad bindable
        /// </summary>
        private bool isbtnagregarservicevisible;
        public bool IsBtnAgregarServiceVisible
        {
            get { return isbtnagregarservicevisible; }
            set { isbtnagregarservicevisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnEditProfile
        /// <summary>
        /// BtnEditProfile
        /// </summary>
        private UserCommand btneditprofile;
        public UserCommand BtnEditProfile
        {
            get { return btneditprofile; }
            set { btneditprofile = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnAgregarService
        /// <summary>
        /// BtnAgregarService de la propiedad bindable
        /// </summary>
        private UserCommand btnagregarservice;
        public UserCommand BtnAgregarService
        {
            get { return btnagregarservice; }
            set { btnagregarservice = value; OnPropertyChanged(); }
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

        #region BindableProperty BtnGuardarDescription
        /// <summary>
        /// BtnGuardarDescription de la propiedad bindable
        /// </summary>
        private ExtendCommand btnguardardescription;
        public ExtendCommand BtnGuardarDescription
        {
            get { return btnguardardescription; }
            set { btnguardardescription = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapLabelDescription
        /// <summary>
        /// TapLabelDescription de la propiedad bindable
        /// </summary>
        private Command taplabeldescription;
        public Command TapLabelDescription
        {
            get { return taplabeldescription; }
            set { taplabeldescription = value; OnPropertyChanged(); }
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

        #region BindableProperty BtnTelefono
        /// <summary>
        /// BtnTelefono de la propiedad bindable
        /// </summary>
        private Command btntelefono;
        public Command BtnTelefono
        {
            get { return btntelefono; }
            set { btntelefono = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnCorreo
        /// <summary>
        /// BtnCorreo de la propiedad bindable
        /// </summary>
        private Command btncorreo;
        public Command BtnCorreo
        {
            get { return btncorreo; }
            set { btncorreo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnAgregarFoto
        /// <summary>
        /// BtnAgregarFoto
        /// </summary>
        private ICommand btnagregarfoto;
        public ICommand BtnAgregarFoto
        {
            get { return btnagregarfoto; }
            set { btnagregarfoto = value; OnPropertyChanged(); }
        }
        #endregion

        #region API
        public IEnumerable<Category> Categorias { get; set; }
        public IEnumerable<Subcategory> Subcategorias { get; set; }
        private List<Service> Services { get; set; }
        public long CompletedServices { get; private set; }
        public double Review { get; set; }
        public Study Study { get; set; }
        public IEnumerable<Work> CV { get; set; }
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
        private Command gotoreviews;
        public Command GoToReviews
        {
            get { return gotoreviews; }
            set { gotoreviews = value; OnPropertyChanged(); }
        }
        #endregion

        public StarsReview Stars { get; set; }

        public ProfileViewModel()
        {

        }

        private async Task<bool> EnsureApiResponses()
        {
            var lang = Usuario.GetUserLogin()?.lenguaje ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var services = await Client.GetServices(lang);

            if (services == null)
                services = new List<API.Response.Service>();

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

            WorksSource = new List<WorkModel>();

            var me = Usuario.GetUserLogin();
            if (me != null)
            {
                if (Services == null)
                {
                    Services = (await Client.Service.Where(new Service
                    {
                        idusuario = me.id
                    })).ToList();
                }

                // cantidad de servicios completados
                CompletedServices = await Client.GetCompletedServices(me.id, me.idperfil);

                // obtener estadisticas del usuario
                Review = await Client.GetReview(me.id);

                // obtener ultimos datos de estudy
                Study = (await Client.Study.Where(new Study
                {
                    idusuario = me.id
                })).FirstOrDefault();
                // obter cv

                CV = (await Client.Work.Where(new Work
                {
                    idusuario = me.id
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

            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;

            Evidences = new ObservableCollection<EvidenceModel>();

            IdState = usuario.identificacionvalidada == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            NSSState = usuario.segurosocialvalidado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            PhoneState = usuario.telefonoverificado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            CorreoState = usuario.activado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];

            PersonalInfoColor = (Color)App.Resources["Accent"];

            BtnEditDocuments = new Command(BtnEditDocuments_Clicked);
            BtnGuardarDescription = new ExtendCommand(BtnGuardarDescription_Clicked, new UserValidator(), new InternetValidator());
            TapLabelDescription = new Command(TapLabelDescription_Clicked);
            BtnTelefono = new Command(BtnTelefono_Clicked);
            BtnCorreo = new Command(BtnCorreo_Clicked);

            Cartas = new ObservableCollection<CartaModel>();
            Works = new ObservableCollection<WorkModel>();

            FotoAsistente = Client.GetPath(usuario.imagen);
            NombreAsistente = usuario.nombre;
            UserDescription = usuario.descripcion;

            IsPersonalInfoVisible = true;
            IsNotStudyInfoVisible = false;
            IsStudyInfoVisible = true;
            IsNotWorkInfoVisible = false;
            IsWorkInfoVisible = true;
            IsGaleriaVisible = false;

            BtnEditWork = new Command(BtnEditWork_Clicked);
            BtnAgregarService = new UserCommand(BtnAgregarService_Clicked);
            BtnEditProfile = new UserCommand(BtnEditProfile_Clicked);
            TapPersonalInfo = new Command(TapPersonalInfo_Clicked);
            TapServiceInfo = new Command(TapServiceInfo_Clicked);
            TapCVInfo = new Command(TapCVInfo_Clicked);
            TapGaleriaInfo = new Command(TapGaleriaInfo_Clicked);
            BtnEditStudy = new Command(BtnEditStudy_Clicked);
            GoToReviews = new Command(GoToReviews_Clicked);

            if (await EnsureApiResponses())
            {
                foreach (var service in Services)
                {
                    var carta = GetServiceModel(service);
                    if (carta == null) continue;
                    Cartas.Add(carta);
                }

                // stats
                StadisticReview = $"{CompletedServices} {AppResource.Tareas}";
                Description = usuario.descripcion;

                IsVisibleDescription = !string.IsNullOrEmpty(Description);

                // last study info
                Grado = Study?.grado ?? "";
                Titulo = Study?.titulo ?? "";
                Institucion = Study?.institucion ?? "";

                var worklist = new List<WorkModel>();
                foreach (var cv in CV)
                {
                    var model = GetWork(cv);
                    if (model == null) continue;
                    worklist.Add(model);
                }

                Works = new ObservableCollection<WorkModel>(works.OrderByDescending(w => w.FechaInicio).ThenBy(w => w.FechaFin));

                if (Stars != null)
                {
                    Stars.Value = Review;
                    IsStarsVisible = true;
                }

                IsHideSingle = Works.Count > 1;
            }

            var galery = await Client.Galery.Where(new Galery
            {
                idusuario = usuario.id
            });

            Source = new List<EvidenceModel>();
            foreach (var item in galery)
            {
                Source.Add(GetModel(item));
            }

            Evidences = new ObservableCollection<EvidenceModel>(Source);

            BtnAgregarFoto = new ExtendCommand(AgregarFoto_Clicked, new InternetValidator());

            MessagingCenter.Instance.Subscribe<App, Study>(App, "Change", StudyChanged);
            MessagingCenter.Instance.Subscribe<App, Work>(App, "Change", WorkChanged);
        }

        private EvidenceModel GetModel(Galery galery)
        {
            return new EvidenceModel()
            {
                Id = galery.id,
                Image = Client.GetPath(galery.path),
                Descripcion = galery.descripcion,
                Options = new ExtendCommand(TapOptions_Clicked, new InternetValidator(), new UserValidator()),
                TieneDescripcion = true
            };
        }

        private async void AgregarFoto_Clicked(object obj, IExecuteValidator[] validators)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;

            var option = await ActionSheet(AppResource.Info, AppResource.Cancelar, AppResource.TomarFoto, AppResource.FotoGaleria);
            MediaFile photo = null;
            if(option == AppResource.TomarFoto)
            {
                photo = await TakePhoto();
            }
            else if(option == AppResource.FotoGaleria)
            {
                photo = await PickPhoto();
            }
            
            if (photo == null) return;

            IsBusy = true;

            var stream = photo.GetStream();
            var upload = await Client.Upload(stream);

            if (!upload.status)
            {
                Toast(AppResource.ErrorImagen);
                IsBusy = false;
                return;
            }

            var description = await Promt<string>(AppResource.Descripcion, AppResource.Cancelar, AppResource.Aceptar, Acr.UserDialogs.InputType.Default) ?? "";

            var path = upload.path;

            var galery = await Client.Galery.Add(new Galery
            {
                path = path,
                descripcion = description,
                idusuario = me.id
            });

            if (galery == null || galery.id <= 0)
            {
                Toast(AppResource.ErrorImagen);
                IsBusy = false;
                return;
            }

            Toast(AppResource.SeSubioImagen);
            // Add To Evidence
            AddToEvidence(galery);
            IsBusy = false;
        }

        private void AddToEvidence(Galery galery)
        {
            if (galery == null) return;
            var model = GetModel(galery);
            if (Source == null) Source = new List<EvidenceModel>();
            Source.Add(model);
            Evidences = new ObservableCollection<EvidenceModel>(Source);
        }

        /// <summary>
        /// Programar Logica
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void TapOptions_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if(!(arg1 is EvidenceModel model)) return;
            var option = await ActionSheet(AppResource.Info, AppResource.Cancelar, AppResource.Editar, AppResource.Eliminar);
            if(option == AppResource.Editar)
            {
                EditPhoto(model);
            }
            else if(option == AppResource.Eliminar)
            {
                DeletePhoto(model);
            }
        }

        private async void EditPhoto(EvidenceModel model)
        {
            var description = await Promt<string>(AppResource.Descripcion, AppResource.Cancelar, AppResource.Aceptar, Acr.UserDialogs.InputType.Default) ?? "";
            if (string.IsNullOrEmpty(description)) return;
            var updated = await Client.Galery.Update(model.Id, new Dictionary<string, string>
            {
                { nameof(Galery.descripcion), description }
            });
            if(updated == null || updated.descripcion != description)
            {
                Toast(AppResource.ErrorServer);
                return;
            }
            model.Descripcion = description;
            Toast(AppResource.DescripcionActualizada);
        }

        private async void DeletePhoto(EvidenceModel model)
        {
            if(!await Confirm(AppResource.SeguroDeEliminarLaImagen)) return;
            var delete = await Client.Galery.Delete(model.Id);
            if (!delete.HasBeenDeleted(model.Id))
            {
                Toast(AppResource.ErrorServer);
                return;
            }
            RemoveFromEvidences(model);
        }

        private void RemoveFromEvidences(EvidenceModel model)
        {
            if (Source == null) return;
            Source.Remove(model);
            Evidences = new ObservableCollection<EvidenceModel>(Source);
        }

        private async void GoToReviews_Clicked(object obj)
        {
            await Navigation.PushAsync(new ReviewListPage
            {
                BindingContext = new ReviewListViewModel()
            });
        }

        public void SetStars(StarsReview stars)
        {
            Stars = stars;
            Stars.Value = Review;
            IsStarsVisible = true;
        }

        #region Study
        private async void BtnEditStudy_Clicked(object obj)
        {
            await Navigation.PushPopupAsync(new StudyPopUp());
        }

        private void StudyChanged(App app, Study study)
        {
            if (study == null) return;
            Grado = study.grado;
            Titulo = study.titulo;
            Institucion = study.institucion;
        }
        #endregion

        #region Trabajos
        private void WorkChanged(App app, Work work)
        {
            IsBusy = true;
            var workinlist = WorksSource.FirstOrDefault(w => w.Id == work.id);
            if (workinlist != null)
            {
                workinlist.Descripcion = work.descripcion;
                workinlist.Empresa = work.empresa;
                workinlist.Fin = work.fin.FromMySqlDateTimeFormat();
                workinlist.Inicio = work.inicio.FromMySqlDateTimeFormat();
                workinlist.Puesto = work.puesto;
            }
            else
            {
                var model = GetWork(work);
                if (model != null)
                    WorksSource?.Add(model);
                    Works = new ObservableCollection<WorkModel>(WorksSource);
            }
            IsBusy = false;
        }

        /// <summary>
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
                Puesto = work.puesto,
                TapOpciones = new Command(Tap_Opciones)
            };
        }

        private async void Tap_Opciones(object obj)
        {
            if (!(obj is WorkModel model)) return;
            var eliminar = AppResource.Eliminar;
            var editar = AppResource.Editar;
            var option = await ActionSheet(AppResource.Opiniones, AppResource.Cancelar, editar, eliminar);
            if(option == editar)
            {
                EditarTrabajo(model);
            }
            else if(option == eliminar)
            {
                EliminarTrabajo(model);
            }
        }

        private async void EliminarTrabajo(WorkModel model)
        {
            if (!await Confirm(AppResource.EliminarTrabajo + model.Puesto + " • " + model.Empresa)) return;
            IsBusy = true;
            var delete = await Client.Work.Delete(model.Id);
            if(!delete.HasBeenDeleted(model.Id))
            {
                Toast(AppResource.NoPodemosEliminarTrabajo);
                IsBusy = false;
                return;
            }
            Toast(AppResource.Trabajoeliminado);
            Works?.Remove(model);
            IsBusy = false;
        }

        private async void EditarTrabajo(WorkModel model)
        {
            await Navigation.PushPopupAsync(new WorkPopUp
            {
                BindingContext = new WorkViewModel
                {
                    Work = model
                }
            });
        }

        private async void BtnEditWork_Clicked(object obj)
        {
            if (IsBusy) return;
            IsBusy = true;
            var me = Usuario.GetUserLogin();
            if (me == null)
            {
                IsBusy = false;
                return;
            }
            var trabajos = await Client.Work.Where(new Work
            {
                idusuario = me.id
            });
            if (trabajos.Count() < 3)
            {
                await Navigation.PushPopupAsync(new WorkPopUp());
            }
            else
            {
                Toast(AppResource.TrabajosMaximos);
            }
            IsBusy = false;
        }
        #endregion

        #region Info Actions
        private async void BtnTelefono_Clicked(object obj)
        {
            await Navigation.PushAsync(new VerifyPhonePage());
        }

        private void BtnCorreo_Clicked(object obj)
        {

        }

        private async void BtnEditProfile_Clicked(object obj, Usuario usuario)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;
            IsBusy = true;
            var takephoto = AppResource.TomarFoto;
            var galery = AppResource.FotoGaleria;
            var option = await ActionSheet(AppResource.Info, AppResource.Cancelar, galery, takephoto);
            MediaFile media = null;
            if (option == takephoto)
            {
                media = await TakePhoto();
            }
            else if (option == galery)
            {
                media = await PickPhoto();
            }

            if (media == null)
            {
                IsBusy = false;
                return;
            }

            var stream = media.GetStream();
            var upload = await Client.Upload(stream);
            if (upload == null || !upload.status)
            {
                Toast(AppResource.ErrorImagen);
                IsBusy = false;
                return;
            }

            var path = upload.path;

            var update = await Client.User.Update(me.id, new Dictionary<string, string>
            {
                { nameof(User.imagen), path }
            });

            if (update == null || update.imagen != path)
            {
                Toast(AppResource.ErrorImagen);
                IsBusy = false;
                return;
            }

            me.imagen = path;
            Usuario.SetUserLogin(me);
            FotoAsistente = Client.GetPath(path);

            IsBusy = false;
        }

        private async void BtnEditDocuments_Clicked(object obj)
        {
            await Navigation.PushAsync(new DocumentPage
            {
                BindingContext = new DocumentViewModel
                {
                    FromProfile = true
                }
            });
        }

        private async void BtnGuardarDescription_Clicked(object obj, IExecuteValidator[] validators)
        {
            IsBusy = true;
            if (!validators.TryGetComparator(out Usuario me)) return;
            var update = await Client.User.Update(me.id, new Dictionary<string,string>
            {
                { nameof(User.descripcion), UserDescription }
            });
            if(update ==  null || update.descripcion != UserDescription)
            {
                Toast(AppResource.DescripcionNoActualizada);
                UserDescription = "";
            }
            else
            {
                Toast(AppResource.DescripcionActualizada);
                me.descripcion = UserDescription;
                Usuario.SetUserLogin(me);
            }
            IsDescriptionEditable = false;
            IsVisibleDescription = true;
            IsBusy = false;
        }

        private void TapLabelDescription_Clicked(object obj)
        {
            IsDescriptionEditable = true;
            IsVisibleDescription = false;
        }

        private async void BtnAgregarService_Clicked(object obj, Usuario usuario)
        {
            if (IsBusy) return;
            IsBusy = true;
            if (await EnsureApiResponses())
            {
                var subcategories = Subcategorias.Where(sub => !Services.Any(s => s.idsubcategoria == sub.id)).Select(s => new CommonModel
                {
                    Id = s.id,
                    Name = s.nombre,
                    Categoria = s.idcategoria,
                    Costo = s.costo
                });

                var categories = Categorias.Select(s => new CommonModel
                {
                    Id = s.id,
                    Name = s.nombre
                });

                var viewmodel = new NewCategoryViewModel(categories.Where(c => subcategories.Select(s => s.Categoria).Contains(c.Id)), subcategories)
                {
                    IdUsuario = usuario.id
                };

                viewmodel.Commit += Viewmodel_Commit;

                await Navigation.PushPopupAsync(new NewCategoryPopUp
                {
                    BindingContext = viewmodel
                });
            }
            IsBusy = false;
        }

        private async void Viewmodel_Commit(object sender, OfferServiceModel e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Navigation.PopPopupAsync();
            var carta = Cartas.FirstOrDefault(c => c.Id == e.Id);
            Service service = null;
            if (e.Id > 0)
            {
                // update
                service = await Client.Service.Update(e.Id, new Dictionary<string, string>
                {
                    { nameof(Service.idcategoria), e.Categoria.ToString() },
                    { nameof(Service.idsubcategoria), e.Subcategoria.ToString() }
                });

                if (service == null || service.idcategoria != e.Categoria || service.idsubcategoria != e.Subcategoria)
                {
                    Toast(AppResource.NoFuePosibleRegistrarElServicio);
                    IsBusy = false;
                    return;
                }
                
                Toast(AppResource.SeHaActualizadoElServicio);
            }
            else
            {
                // insert
                service = await Client.Service.Add(new Service
                {
                    idcategoria = e.Categoria,
                    idsubcategoria = e.Subcategoria,
                    idusuario = e.IdUsuario,
                    eliminado = 0
                });

                if (service == null || service.id <= 0)
                {
                    Toast(AppResource.NoFuePosibleRegistrarElServicio);
                    IsBusy = false;
                    return;
                }
                
                Toast(AppResource.SeHaRegistradoElServicio);
            }

            UpdateServiceList(carta, service);

            IsBusy = false;
        }

        private void UpdateServiceList(CartaModel carta, Service service)
        {
            if (service == null) return;
            if (Categorias == null) return;
            if (Subcategorias == null) return;
            if (Services == null) Services = new List<Service>();
            Services.Add(service);
            var categoria = Categorias.FirstOrDefault(c => c.id == service.idcategoria);
            var subcategoria = Subcategorias.FirstOrDefault(c => c.id == service.idsubcategoria);
            if (carta != null)
            {
                // update card
                carta.Precio = service.costo;
                carta.Costo = service.costo.ToString();
                carta.Id = service.id;
                carta.Image = Client.GetPath(subcategoria.imagen);
                carta.Title = categoria.nombre;
                carta.Subtitle = subcategoria.nombre;
            }
            else
            {
                carta = GetServiceModel(service);
                if (carta != null)
                {
                    Cartas.Add(carta);
                }
            }
        }

        private CartaModel GetServiceModel(Service service)
        {
            if (service == null) return null;
            if (Categorias == null) return null;
            if (Subcategorias == null) return null;
            var categoria = Categorias.FirstOrDefault(c => c.id == service.idcategoria);
            var subcategoria = Subcategorias.FirstOrDefault(c => c.id == service.idsubcategoria);
            if (categoria == null || subcategoria == null) return null;
            return new CartaModel
            {
                Precio = service.costo,
                Costo = service.costo.ToString(),
                Id = service.id,
                Image = Client.GetPath(subcategoria.imagen),
                Title = categoria.nombre,
                Subtitle = subcategoria.nombre,
                TapMenu = new Command(TapMenuService)
            };
        }

        private async void TapMenuService(object obj)
        {
            if (!(obj is CartaModel model)) return;
            if(!await Confirm(AppResource.SeguroDeEliminarServicio)) return;
            IsBusy = true;
            var delete = await Client.Service.Update(model.Id, new Dictionary<string, string>
            {
                { nameof(Service.eliminado), "1" }
            });
            if(delete == null || delete.eliminado != 1)
            {
                Toast(AppResource.NoPodemosEliminarElServicio);
                return;
            }
            Toast(AppResource.SeHaEliminadoElServicio);
            Cartas.Remove(model);
            IsBusy = false;
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
            IsBtnAgregarServiceVisible = false;
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
            IsBtnAgregarServiceVisible = false;
            IsGaleriaVisible = true;
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
            IsBtnAgregarServiceVisible = true;
            IsGaleriaVisible = false;
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
            IsBtnAgregarServiceVisible = false;
            IsGaleriaVisible = false;
        }
        #endregion
        
    }
}
