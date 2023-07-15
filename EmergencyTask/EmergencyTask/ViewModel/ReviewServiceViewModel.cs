using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using EmergencyTask.Views.Rating;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ReviewServiceViewModel : ViewModelBase
    {
        #region BindableProperty Servicio
        /// <summary>
        /// Servicio de la propiedad bindable
        /// </summary>
        private string servicio;
        public string Servicio
        {
            get { return servicio; }
            set { servicio = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Tarea
        /// <summary>
        /// Tarea de la propiedad bindable
        /// </summary>
        private string tarea;
        public string Tarea
        {
            get { return tarea; }
            set { tarea = value; OnPropertyChanged(); }
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

        #region BindableProperty BtnAceptar
        /// <summary>
        /// BtnAceptar de la propiedad bindable
        /// </summary>
        private ExtendCommand btnaceptar;
        public ExtendCommand BtnAceptar
        {
            get { return btnaceptar; }
            set { btnaceptar = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Opinion
        /// <summary>
        /// Opinion
        /// </summary>
        private string opinion;
        public string Opinion
        {
            get { return opinion; }
            set { opinion = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsSaveVisible
        /// <summary>
        /// IsSaveVisible
        /// </summary>
        private bool issavevisible = true;
        public bool IsSaveVisible
        {
            get { return issavevisible; }
            set { issavevisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnEvidence
        /// <summary>
        /// BtnEvidence
        /// </summary>
        private Command btnevidence;
        public Command BtnEvidence
        {
            get { return btnevidence; }
            set { btnevidence = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsEvidenceVisible
        /// <summary>
        /// IsEvidenceVisible
        /// </summary>
        private bool isevidencevisible;
        public bool IsEvidenceVisible
        {
            get { return isevidencevisible; }
            set { isevidencevisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BoxDescription
        /// <summary>
        /// BoxDescription
        /// </summary>
        private string boxDescription;
        public string BoxDescription
        {
            get { return boxDescription; }
            set { boxDescription = value; OnPropertyChanged(); }
        }
        #endregion

        public Review CurrentReview { get; private set; }
        public Requestservice CurrentService { get; set; }
        public StarsReview StarsControl { get; set; }

        public ReviewServiceViewModel(Requestservice service)
        {
            CurrentService = service;
        }

        public void SetStartControl(StarsReview starReview)
        {
            StarsControl = starReview;
            StarsControl.Value = 5;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);

            if (CurrentService == null) return;

            var me = Usuario.GetUserLogin();
            if (me == null) return;

            IsBusy = true;

            var idpersona = App.Perfil == Perfil.Client ? CurrentService.trabajador : CurrentService.cliente;

            var persona = await Client.User.Get(idpersona);

            if (persona == null) return;

            BoxDescription = me.Perfil == Perfil.Client ? AppResource.OpinionDelServicio : AppResource.OpinionDelCliente;

            Servicio = $"{AppResource.Servicio}: {CurrentService.categoria}";
            Tarea = $"{AppResource.Tarea}: {CurrentService.subcategoria}";
            FotoAsistente = Client.GetPath(persona.imagen);
            NombreAsistente = $"{AppResource.Con} {persona.nombre}";

            CurrentReview = (await Client.Review.Where(new Review
            {
                idperfil = (int) me.Perfil,
                idsolicitudservicio = CurrentService.id,
                idusuario = me.id
            })).FirstOrDefault();

            var current = 0.0D;
            if (CurrentReview == null)
            {
                BtnAceptar = new ExtendCommand(BtnAceptar_Clicked, new UserValidator(), new InternetValidator());
                IsSaveVisible = true;
            }
            else
            {
                current = CurrentReview.calificacion;
                IsSaveVisible = false;
                BtnEvidence = new Command(BtnEvidence_Clicked);
                IsEvidenceVisible = me.idperfil == (int)Perfil.Tasker;
            }
        }

        private async void BtnEvidence_Clicked(object obj)
        {
            if (CurrentService == null) return;
            if (CurrentReview == null) return;
            await GoToEvidence(CurrentService.id, CurrentReview.id);
        }

        private async void BtnAceptar_Clicked(object obj, IExecuteValidator[] args)
        {
            if (CurrentService == null) return;
            if (!args.TryGetComparator(out Usuario me)) return;

            if(!await Confirm(AppResource.EnviarCalificacion)) return;

            IsBusy = true;

            if ((StarsControl?.Value ?? 0) == 0)
            {
                Toast(AppResource.CalificacionInvalida);
                IsBusy = false;
                return;
            }

            var reviews = (await Client.Review.Where(new Review
            {
                idsolicitudservicio = CurrentService.id
            })).ToList();

            var review = reviews.FirstOrDefault(r => r.idperfil == me.idperfil && r.idusuario == me.id);

            if (review == null)
            {
                review = await Client.Review.Add(new Review
                {
                    calificacion = StarsControl.Value,
                    idperfil = (int)me.Perfil,
                    idsolicitudservicio = CurrentService.id,
                    idusuario = me.id,
                    comentario = string.IsNullOrEmpty(Opinion) ? " " : Opinion
                });
            }
            else
            {
                review = await Client.Review.Update(review.id, new Dictionary<string, string>
                {
                    { "calificacion", StarsControl.Value.ToString() },
                    { "idperfil", ((int)me.Perfil).ToString() },
                    { "idsolicitudservicio", CurrentService.id.ToString() },
                    { "idusuario", me.id.ToString() },
                    { "comentario", Opinion }
                });
            }

            if(review == null || review.id <= 0)
            {
                DisplayAlert(AppResource.ErrorCalificacion, AppResource.Aceptar);
                IsBusy = false;
                return;
            }

            if (reviews != null)
            {
                reviews.Add(review);
                if (reviews.Count() == 2)
                {
                    await Client.ChangeServiceStatus(CurrentService.id, EstadoServicio.Calificado, me.id, Latitud, Longitud, Latitud != 0 && Longitud != 0 ? 1 : 0);
                }
            }

            Toast(AppResource.CalificacionGuardada);

            if (App.Perfil == Perfil.Tasker)
            {
                await GoToEvidence(CurrentService.id, review.id);
            }
            else
            {
                GoToProfileHome();
            }

            IsBusy = false;
        }

    }
}
