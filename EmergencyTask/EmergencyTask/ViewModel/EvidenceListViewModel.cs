using System.Collections.Generic;
using System.Collections.ObjectModel;
using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class EvidenceListViewModel : ViewModelBase
    {

        #region Notified Property Source
        /// <summary>
        /// Source
        /// </summary>
        private ObservableCollection<EvidenceModel> source;
        public ObservableCollection<EvidenceModel> Source
        {
            get { return source; }
            set { source = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property AddEvidence
        /// <summary>
        /// AddEvidence
        /// </summary>
        private ExtendCommand addevidence;
        public ExtendCommand AddEvidence
        {
            get { return addevidence; }
            set { addevidence = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdCalificacion { get; set; }
        public int IdSolicitudServicio { get; set; }

        public EvidenceListViewModel(int idsolicitudservicio, int idcalificacion)
        {
            IdSolicitudServicio = idsolicitudservicio;
            IdCalificacion = idcalificacion;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            IsBusy = true;

            AddEvidence = new ExtendCommand(AddEvidence_Clicked, new InternetValidator(), new UserValidator());
            var evidence = await Client.Evidence.Where(new Evidence
            {
                idsolicitudservicio = IdSolicitudServicio,
                idcalificacion = IdCalificacion
            });

            foreach (var item in evidence)
            {
                InsertEvidence(GetModel(item));
            }

            IsBusy = false;
        }

        private async void AddEvidence_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            IsBusy = true;
            var me = Usuario.GetUserLogin();
            if (me == null)
            {
                IsBusy = false;
                return;
            }

            var takephoto = AppResource.TomarFoto;
            var galery = AppResource.GaleriaImagenes;
            var option = await ActionSheet(AppResource.SubirEvidencia, AppResource.Cancelar, galery, takephoto);
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

            string text;
            var loop = true;
            do
            {
                text = await Promt<string>(AppResource.InputEvidenceDescription, AppResource.Cancelar, AppResource.Accept, Acr.UserDialogs.InputType.Default);
                if (string.IsNullOrEmpty(text))
                    loop = !await Confirm(AppResource.ConfirmUploadEvidenceWithoutDescription);
                else
                    loop = false;
            } while (loop);

            var stream = media.GetStream();

            var upload = await Client.Upload(stream);
            if (upload == null || !upload.status)
            {
                Toast(upload?.message ?? AppResource.ErrorImagen);
                IsBusy = false;
                return;
            }

            var path = upload.path;
            var evidencia = await Client.Evidence.Add(new Evidence
            {
                idcalificacion = IdCalificacion,
                path = path,
                idsolicitudservicio = IdSolicitudServicio,
                idusuario = me.id,
                comentario = text
            });

            if(evidencia == null || evidencia.id <= 0)
            {
                Toast(AppResource.ErrorImagen);
                IsBusy = false;
                return;
            }

            Toast(AppResource.EvidenciaAgregada);
            var item = GetModel(evidencia);
            if (item == null) return;
            InsertEvidence(item);
            IsBusy = false;
        }

        private void InsertEvidence(EvidenceModel evidencemodel)
        {
            if (Source == null) Source = new ObservableCollection<EvidenceModel>();
            Source.Add(evidencemodel);
        }

        private EvidenceModel GetModel(Evidence evidencia)
        {
            if (evidencia == null) return null;
            return new EvidenceModel
            {
                Id = evidencia.id,
                Image = Client.GetPath(evidencia.path),
                Descripcion = evidencia.comentario,
                TieneDescripcion = string.IsNullOrEmpty(evidencia.comentario),
                TapDelete = new ExtendCommand(Delete_Tapped, new InternetValidator())
            };
        }

        private async void Delete_Tapped(object arg1, IExecuteValidator[] arg2)
        {
            if (!(arg1 is EvidenceModel model)) return;
            IsBusy = true;
            var update = await Client.Evidence.Update(model.Id, new Dictionary<string, string>
            {
                { nameof(Evidence.eliminado), "1" }
            });

            if(update == null || update.eliminado != 1)
            {
                Toast(AppResource.SinEliminarEvidencia);
                IsBusy = false;
                return;
            }
            Device.BeginInvokeOnMainThread(() => Source?.Remove(model));
            IsBusy = false;
        }
    }
}
