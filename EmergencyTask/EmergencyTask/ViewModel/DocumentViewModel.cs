using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class DocumentViewModel : ViewModelBase
    {

        #region BindableProperty NroSeguro
        /// <summary>
        /// NroSeguro de la propiedad bindable
        /// </summary>
        private string nroseguro;
        public string NroSeguro
        {
            get { return nroseguro; }
            set { nroseguro = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty FrontIne
        /// <summary>
        /// FrontIne de la propiedad bindable
        /// </summary>
        private CartaModel frontine;
        public CartaModel FrontIne
        {
            get { return frontine; }
            set { frontine = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty FotoUser
        /// <summary>
        /// FotoUser de la propiedad bindable
        /// </summary>
        private ImageSource fotouser;
        public ImageSource FotoUser
        {
            get { return fotouser; }
            set { fotouser = value; OnPropertyChanged(); }
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

        #region BindableProperty BtnGuardar
        /// <summary>
        /// BtnGuardar de la propiedad bindable
        /// </summary>
        private UserCommand btnguardar;
        public UserCommand BtnGuardar
        {
            get { return btnguardar; }
            set { btnguardar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property UploadProfileImage
        /// <summary>
        /// UploadProfileImage
        /// </summary>
        private UserCommand uploadprofileimage;
        public UserCommand UploadProfileImage
        {
            get { return uploadprofileimage; }
            set { uploadprofileimage = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Identificacion
        /// <summary>
        /// Identificacion
        /// </summary>
        private string identificacion;
        public string Identificacion
        {
            get { return identificacion; }
            set { identificacion = value; OnPropertyChanged(); }
        }

        public bool FromProfile { get; internal set; }
        #endregion

        #region Notified Property IsClient
        /// <summary>
        /// IsClient
        /// </summary>
        private bool isclient;
        public bool IsClient
        {
            get { return isclient; }
            set { isclient = value; OnPropertyChanged(); }
        }
        #endregion

        public DocumentViewModel()
        {
            
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            var usuario = Usuario.GetUserLogin();

            if (usuario != null) 
            {
                IsClient = usuario.Perfil == API.Enum.Perfil.Client;
                NroSeguro = usuario.segurosocialvalidado == 1 ? usuario.segurosocial : string.Empty;
                FotoUser = string.IsNullOrEmpty(usuario.imagen) ? "icon.png" : Client.GetPath(usuario.imagen);
                NameUser = usuario.nombre;
                BtnGuardar = new UserCommand(BtnGuardar_Clicked);
                FrontIne = new CartaModel
                {
                    Title = AppResource.FotoId,
                    Image = "icon.png",
                    Action = new ExtendCommand(UploadId, new UserValidator(), new InternetValidator())
                };
                UploadProfileImage = new UserCommand(UploadProfileImage_Command);
            }
            else
            {
                Toast(AppResource.SesionCaducada);
                App.Restart();
            }
        }

        private async void UploadProfileImage_Command(object obj, Usuario usuario)
        {
            if (IsBusy) return;
            IsBusy = true;

            string takephoto = AppResource.TomarFoto;
            string pickphoto = AppResource.FotoGaleria;

            var option = await ActionSheet(AppResource.CambiarFoto, AppResource.Cancelar, pickphoto, takephoto);

            if (string.IsNullOrEmpty(option))
            {
                IsBusy = false;
                return;
            }

            if (option == AppResource.Cancelar)
            {
                IsBusy = false;
                return;
            }

            MediaFile photo = null;

            if (option == takephoto)
            { 
                photo = await TakePhoto();
            }
            else if(option == pickphoto)
            {
                photo = await PickPhoto();
            }

            if (photo == null)
            {
                IsBusy = false;
                return;
            }

            var upload = await Client.Upload(photo.GetStream());
            if (!upload.status)
            {
                Toast(AppResource.NoSubirImagen);
                IsBusy = false;
                return;
            }

            var imagepath = upload.path;

            var update = await Client.User.Update(usuario.id, new System.Collections.Generic.Dictionary<string, string>
            {
                { nameof(Usuario.imagen), imagepath }
            });

            if (update != null && update.imagen == imagepath)
            {
                var path = Client.GetPath(update.imagen);
                FotoUser = path;
                usuario.imagen = path;
                await usuario.SaveChanges();
                Toast(AppResource.SeCambioFoto);
            }
            else
            {
                Toast(AppResource.NoSubirImagen);
            }

            IsBusy = false;
        }

        private async void UploadId(object obj, IExecuteValidator[] validators)
        {
            if (IsBusy) return;
            IsBusy = true;

            var photo = await TakePhoto();
            if (photo == null)
            {
                IsBusy = false;
                return;
            }
            var upload = await Client.Upload(photo.GetStream());
            if (!upload.status)
            {
                Toast(AppResource.NoSubirImagen);
                IsBusy = false;
                return;
            }

            Identificacion = upload.path;

            var usuario = Usuario.GetUserLogin();
            if (usuario == null)
            {
                Toast(AppResource.NoSubirImagen);
                IsBusy = false;
                return;
            }

            var update = await Client.User.Update(usuario.id, new System.Collections.Generic.Dictionary<string, string>
            {
                { nameof(Usuario.identificacion), Identificacion },
                { nameof(Usuario.identificacionvalidada), "0" }
            });

            if (update != null && update.identificacion == Identificacion && update.identificacionvalidada == 0)
            {
                var path = Client.GetPath(update.identificacion);
                FrontIne.Image = path;
                usuario.identificacion = path;
                usuario.identificacionvalidada = 0;
                await usuario.SaveChanges();
                Toast(AppResource.SeSubioImagen);
            }
            else
            {
                Toast(AppResource.NoSubirImagen);
            }

            IsBusy = false;
        }

        private async void BtnGuardar_Clicked(object obj, Usuario usuario)
        {
            IsBusy = true;

            if (!string.IsNullOrEmpty(NroSeguro))
            {
                var update = await Client.User.Update(usuario.id, new System.Collections.Generic.Dictionary<string, string>
                {
                    { nameof(Usuario.segurosocial), NroSeguro },
                    { nameof(Usuario.segurosocialvalidado), "0" }
                });

                if(update != null && update.segurosocial == NroSeguro && update.segurosocialvalidado == 0)
                {
                    usuario.segurosocial = update.segurosocial;
                    usuario.segurosocialvalidado = 0;
                    await usuario.SaveChanges();
                    Toast(AppResource.NssGuardado);
                }
            }

            if (FromProfile)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await OnBasedProfileOpenApp();
            }

            IsBusy = false;
        }
    }
}