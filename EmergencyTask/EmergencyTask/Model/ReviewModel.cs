using EmergencyTask.Views.Rating;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyTask.Model
{
    public class ReviewModel : ModelBase
    {

        #region BindableProperty Comentario
        /// <summary>
        /// Comentario de la propiedad bindable
        /// </summary>
        private string comentario;
        public string Comentario
        {
            get { return comentario; }
            set { comentario = value; OnPropertyChanged(); }
        }
        #endregion


        #region BindableProperty Estado
        /// <summary>
        /// Estado de la propiedad bindable
        /// </summary>
        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Valor
        /// <summary>
        /// Valor de la propiedad bindable
        /// </summary>
        private StarsReviewModel valor;
        public StarsReviewModel Valor
        {
            get { return valor; }
            set { valor = value; OnPropertyChanged(); }
        }

        public string Imagen { get; internal set; }
        public string Nombre { get; internal set; }
        #endregion


    }
}
