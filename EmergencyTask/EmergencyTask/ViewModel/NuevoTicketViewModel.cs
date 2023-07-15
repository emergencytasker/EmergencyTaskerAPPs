using System.Collections.Generic;
using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class NuevoTicketViewModel : ViewModelBase
    {

        #region BindableProperty Image
        /// <summary>
        /// Image de la propiedad bindable
        /// </summary>
        public ImageSource Image
        {
            get { return Model.Image; }
            set { Model.Image = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TapImage
        /// <summary>
        /// TapImage de la propiedad bindable
        /// </summary>
        private ExtendCommand tapimage;
        public ExtendCommand TapImage
        {
            get { return tapimage; }
            set { tapimage = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Descripcion
        /// <summary>
        /// Descripcion de la propiedad bindable
        /// </summary>
        public string Descripcion
        {
            get { return Model.Description; }
            set { Model.Description = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnGuardar
        /// <summary>
        /// BtnGuardar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnguardar;
        public ExtendCommand BtnGuardar
        {
            get { return btnguardar; }
            set { btnguardar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Id
        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get { return Model.Id; }
            set { Model.Id = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Model
        /// <summary>
        /// Model
        /// </summary>
        private TicketListModel model = new TicketListModel();
        public TicketListModel Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdSolicitudServicio { get; set; }
        public string Path { get; set; }

        public NuevoTicketViewModel(int idsolicitudservicio, TicketListModel model = null)
        {
            Model = model ?? new TicketListModel();
            IdSolicitudServicio = idsolicitudservicio;
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            BtnGuardar = new ExtendCommand(BtnGuardar_Clicked, new InternetValidator(), new UserValidator());
            TapImage = new ExtendCommand(TapImage_Tapped, new InternetValidator());
        }

        private async void TapImage_Tapped(object arg1, IExecuteValidator[] arg2)
        {
            IsBusy = true;
            var takephoto = AppResource.TomarFoto;
            var pickphoto = AppResource.FotoGaleria;
            var source = await ActionSheet(AppResource.SubirTicket, AppResource.Cancelar, takephoto, pickphoto);

            MediaFile image = null;
            if (source == takephoto)
            {
                image = await TakePhoto();
            }
            else if (source == pickphoto)
            {
                image = await PickPhoto();
            }

            if (image == null)
            {
                IsBusy = false;
                return;
            }

            var upload = await Client.Upload(image.GetStream());
            if (!upload.status)
            {
                Toast(AppResource.NoSubirImagen);
                IsBusy = false;
                return;
            }

            Path = upload.path;
            Image = Client.Path(Path);
            IsBusy = false;
        }

        private async void BtnGuardar_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            IsBusy = true;
            if (Model.Id > 0)
            {
                var ticket = await Client.Ticket.Update(Model.Id, new Dictionary<string, string>
                {
                    { nameof(Ticket.imagen), Path },
                    { nameof(Ticket.detalle), Descripcion }
                });

                if (ticket == null || ticket.id <= 0)
                {
                    Toast(AppResource.NoGuardoTicket);
                    IsBusy = false;
                    return;
                }
            }
            else
            {
                var ticket = await Client.Ticket.Add(new Ticket
                {
                    detalle = Descripcion,
                    imagen = Path,
                    idsolicitudservicio = IdSolicitudServicio
                });

                if (ticket == null || ticket.id <= 0)
                {
                    Toast(AppResource.NoGuardoTicket);
                    IsBusy = false;
                    return;
                }

                Model.Id = ticket.id;
            }

            if (PopupNavigation.Instance.PopupStack.Count > 0) await PopupNavigation.Instance.PopAsync();

            MessagingCenter.Instance.Send(App, "Refresh", Model);
            IsBusy = false;
        }
    }
}
