using EmergencyTask.API;
using EmergencyTask.API.Enum;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ReviewListViewModel : ViewModelBase
    {

        #region BindableProperty FiveCount
        /// <summary>
        /// FiveCount de la propiedad bindable
        /// </summary>
        private int fivecount;
        public int FiveCount
        {
            get { return fivecount; }
            set { fivecount = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty FourCount
        /// <summary>
        /// FourCount de la propiedad bindable
        /// </summary>
        private int fourcount;
        public int FourCount
        {
            get { return fourcount; }
            set { fourcount = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty ThreeCount
        /// <summary>
        /// ThreeCount de la propiedad bindable
        /// </summary>
        private int threecount;
        public int ThreeCount
        {
            get { return threecount; }
            set { threecount = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty TwoCount
        /// <summary>
        /// TwoCount de la propiedad bindable
        /// </summary>
        private int twocount;
        public int TwoCount
        {
            get { return twocount; }
            set { twocount = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty OneCount
        /// <summary>
        /// OneCount de la propiedad bindable
        /// </summary>
        private int onecount;
        public int OneCount
        {
            get { return onecount; }
            set { onecount = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Review
        /// <summary>
        /// Review de la propiedad bindable
        /// </summary>
        private ReviewModel review;
        public ReviewModel Review
        {
            get { return review; }
            set { review = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Reviews
        /// <summary>
        /// Reviews de la propiedad bindable
        /// </summary>
        private ObservableCollection<ReviewModel> reviews;
        public ObservableCollection<ReviewModel> Reviews
        {
            get { return reviews; }
            set { reviews = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property OnePercent
        /// <summary>
        /// OnePercent
        /// </summary>
        private double onepercent;
        public double OnePercent
        {
            get { return onepercent; }
            set { onepercent = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property TwoPercent
        /// <summary>
        /// TwoPercent
        /// </summary>
        private double twopercent;
        public double TwoPercent
        {
            get { return twopercent; }
            set { twopercent = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ThreePercent
        /// <summary>
        /// ThreePercent
        /// </summary>
        private double threepercent;
        public double ThreePercent
        {
            get { return threepercent; }
            set { threepercent = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FourPercent
        /// <summary>
        /// FourPercent
        /// </summary>
        private double fourpercent;
        public double FourPercent
        {
            get { return fourpercent; }
            set { fourpercent = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property FivePercent
        /// <summary>
        /// FivePercent
        /// </summary>
        private double fivepercent;
        public double FivePercent
        {
            get { return fivepercent; }
            set { fivepercent = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Percent
        /// <summary>
        /// Percent
        /// </summary>
        private double percent;
        public double Percent
        {
            get { return percent; }
            set { percent = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdUsuario { get; set; }

        public ReviewListViewModel(int idusuario = 0)
        {
            IdUsuario = idusuario;
        }

        public override async void OnAppearing(Page page)
        {
            var me = Usuario.GetUserLogin();
            if (IdUsuario > 0)
            {
                var user = await Client.User.Get(IdUsuario);
                try
                {
                    me = JsonConvert.DeserializeObject<Usuario>(JsonConvert.SerializeObject(user));
                }
                catch { }
            }

            if (me == null) return;

            var expression = (Expression<Func<Requestservice, object>>)(s => s.trabajador);


            if (IdUsuario > 0)
            {
                expression = App.Perfil == Perfil.Client ? (Expression<Func<Requestservice, object>>)(s => s.trabajador) : (s => s.cliente);
            }
            else
            {
                expression = App.Perfil == Perfil.Client ? (Expression<Func<Requestservice, object>>)(s => s.cliente) : (s => s.trabajador);
            }

            var servicesquery = await Client.Requestservice.Where(expression, s => s.eliminado).In(new object[]
            {
                me.id
            }, new object[]
            {
                0
            }).Execute();

            if (!servicesquery.HasExecute)
            {
                Toast(AppResource.NoPodemosContinuar);
                return;
            }

            var services = (servicesquery.Result ?? new List<Requestservice>(0)).Where
            (
                s => s.idestadoservicio == (int) EstadoServicio.TrabajoTerminado || s.idestadoservicio == (int) EstadoServicio.Finalizado
                || s.idestadoservicio == (int) EstadoServicio.Calificado
            );

            if(services.Count() <= 0) return;

            var reviewsquery = await Client.Review.Where(r => r.idsolicitudservicio).In(services.Where(s => s.eliminado == 0).Select(s => (object) s.id)).Execute();

            if (!reviewsquery.HasExecute)
            {
                Toast(AppResource.NoPodemosContinuar);
                return;
            }

            var reviewsdata = reviewsquery.Result ?? new List<Review>(0);

            var reviews = reviewsdata.Where(r => r.idperfil != me.idperfil && r.eliminado == 0 && r.idusuario != me.id).GroupBy(r => r.idsolicitudservicio);

            FiveCount = reviews.Count(r => r.Any(c => c.calificacion == 5));
            FourCount = reviews.Count(r => r.Any(c => c.calificacion == 4));
            ThreeCount = reviews.Count(r => r.Any(c => c.calificacion == 3));
            TwoCount = reviews.Count(r => r.Any(c => c.calificacion == 2));
            OneCount = reviews.Count(r => r.Any(c => c.calificacion == 1));

            FivePercent = ((double)FiveCount) / reviews.Count();
            FourPercent = ((double)FourCount) / reviews.Count();
            ThreePercent = ((double)ThreeCount) / reviews.Count();
            TwoPercent = ((double)TwoCount) / reviews.Count();
            OnePercent = ((double)OneCount) / reviews.Count();

            try
            {
                Percent = Math.Round(reviews.Where(r => r.Count() > 0).Average(r =>
                {
                    var review = r.FirstOrDefault();
                    if (review == null) return 0;
                    return review.calificacion;
                }), 2);
            }
            catch { }

            var usuarios = reviewsdata.Select(s => (object) s.idusuario);
            var users = (await Client.User.Where(u => u.id).In(usuarios).Execute())?.Result;

            Reviews = new ObservableCollection<ReviewModel>(reviews.Where(r => r.Count() > 0).Select(r => {
                var user = users?.FirstOrDefault(u => u.id == r.First().idusuario);
                return new ReviewModel
                {
                    Estado = GetState((int)r.First().calificacion),
                    Valor = new Views.Rating.StarsReviewModel
                    {
                        CanChange = false,
                        StarHeight = 15,
                        StarWidth = 15,
                        Value = r.First().calificacion
                    },
                    Comentario = string.IsNullOrEmpty(r.First().comentario) ? "----" : r.First().comentario,
                    Imagen = Client.Path(user.imagen),
                    Nombre = user?.nombre ?? "Unknown"
                };
            }));
        }

        public string GetState(int calification)
        {
            if (calification == 5)
                return "Excelente";
            else if (calification == 4)
                return "Buena";
            else if (calification == 3)
                return "Regular";
            else if (calification == 2)
                return "Mala";
            else if (calification == 1)
                return "Pesima";
            else
                return "N/A";
        }
    }
}
