using System;
using System.Collections.Generic;
using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Views.Rating;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ProfileClientViewModel : ViewModelBase
    {

        #region BindableProperty ImageUser
        /// <summary>
        /// ImageUser de la propiedad bindable
        /// </summary>
        private ImageSource imageuser;
        public ImageSource ImageUser
        {
            get { return imageuser; }
            set { imageuser = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty NameUser
        /// <summary>
        /// NameUser de la propiedad bindable
        /// </summary>
        private string nameuser;
        public string NameUser
        {
            get { return nameuser; }
            set { nameuser = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnEditProfile
        /// <summary>
        /// BtnEditProfile de la propiedad bindable
        /// </summary>
        private ExtendCommand btneditprofile;
        public ExtendCommand BtnEditProfile
        {
            get { return btneditprofile; }
            set { btneditprofile = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty UserDescription
        /// <summary>
        /// UserDescription de la propiedad bindable
        /// </summary>
        private string userdecription;
        public string UserDescription
        {
            get { return userdecription; }
            set { userdecription = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapLabelDescription
        /// <summary>
        /// TapLabelDescripcion de la propiedad bindable
        /// </summary>
        private Command taplabeldescription;
        public Command TapLabelDescription
        {
            get { return taplabeldescription; }
            set { taplabeldescription = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnGuardarDescription
        /// <summary>
        /// BtnGuardarDescription de la propiedad bindable
        /// </summary>
        private Command btnguardardescription;
        public Command BtnGuardarDescription
        {
            get { return btnguardardescription; }
            set { btnguardardescription = value; OnPropertyChanged(); }
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

        #region BindableProperty IsDescriptionEditable
        /// <summary>
        /// IsDescriptionEditable de la propiedad bindable
        /// </summary>
        private bool isdescriptioneditable;
        public bool IsDescriptionEditable
        {
            get { return isdescriptioneditable; }
            set { isdescriptioneditable = value; OnPropertyChanged(); }
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

        #region Notified Property StadisticReview
        /// <summary>
        /// StadisticReview
        /// </summary>
        private string stadisticreview;
        public string StadisticReview
        {
            get { return stadisticreview; }
            set { stadisticreview = value; OnPropertyChanged(); }
        }
        #endregion

        public StarsReview Stars { get; set; }

        public ProfileClientViewModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;

            IdState = usuario.identificacionvalidada == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            NSSState = usuario.segurosocialvalidado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            PhoneState = usuario.telefonoverificado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];
            CorreoState = usuario.activado == 1 ? (string)App.Resources["Checkboxtrue"] : (string)App.Resources["Checkboxfalse"];

            ImageUser = Client.GetPath(usuario.imagen);
            NameUser = usuario.nombre;

            // cantidad de servicios completados
            var completedservices = await Client.GetCompletedServices(usuario.id, usuario.idperfil);
            StadisticReview = $"{completedservices} {AppResource.Tareas}";

            IsVisibleDescription = true;
            IsDescriptionEditable = false;

            if(Stars != null)
                Stars.Value = await Client.GetReview(usuario.id);

            GoToReviews = new Command(GoToReviews_Clicked);

            BtnEditProfile = new ExtendCommand(BtnEditProfile_Command, new InternetValidator(), new UserValidator());
            BtnEditDocuments = new Command(BtnEditDocuments_Clicked);
            BtnTelefono = new Command(BtnTelefono_Clicked);
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
        }

        private async void BtnEditProfile_Command(object obj, IExecuteValidator[] args)
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
                Toast(upload?.message ?? AppResource.ErrorImagen);
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
            ImageUser = Client.GetPath(path);

            IsBusy = false;
        }

        private async void BtnTelefono_Clicked(object obj)
        {
            await Navigation.PushAsync(new VerifyPhonePage());
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
    }
}
