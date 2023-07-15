using EmergencyTask.Views.Rating.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Simsa.Views.Rating
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StarsReview : Grid
    {

        #region BindableProperty StarHeight
        public static readonly BindableProperty StarHeightProperty = BindableProperty.Create("StarHeight", typeof(double), typeof(StarsReview), 30.0D, propertyChanged: OnStarHeightChanged);

        private static void OnStarHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarsReview view)
            {
                if (newValue != null)
                {
                    if (double.TryParse(newValue.ToString(), out double size))
                    {
                        view.StarHeight = size;
                    }
                }
            }
        }

        private double starheight = 30.0D;
        public double StarHeight
        {
            get { return starheight; }
            set { starheight = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty StarWidth
        public static readonly BindableProperty StarWidthProperty = BindableProperty.Create("StarWidth", typeof(double), typeof(StarsReview), 30.0D, propertyChanged: OnStarWidthChanged);

        private static void OnStarWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is StarsReview view)
            {
                if(newValue != null)
                {
                    if(double.TryParse(newValue.ToString(), out double size))
                    {
                        view.StarWidth = size;
                    }
                }
            }
        }

        private double itemsize = 30.0D;
        public double StarWidth
        {
            get { return itemsize; }
            set { itemsize = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Value
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(double), typeof(StarsReview), 5.0D, propertyChanged: OnValuePropertyChanged);
        private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarsReview view)
            {
                if (newValue != null)
                {
                    view.Value = (double) newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private double myvalue;
        public double Value
        {
            get { return myvalue; }
            set { myvalue = value; OnPropertyChanged(); Toggle(Children.Cast<StarView>(), myvalue); }
        }
        #endregion

        #region BindableProperty CanChange
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty CanChangeProperty = BindableProperty.Create("CanChange", typeof(bool), typeof(StarsReview), false, propertyChanged: OnCanChangePropertyChanged);
        private static void OnCanChangePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarsReview view)
            {
                if (newValue != null)
                {
                    view.CanChange = (bool)newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private bool canchange;
        public bool CanChange
        {
            get { return canchange; }
            set { canchange = value; OnPropertyChanged(); }
        }
        #endregion

        public StarsReview ()
		{
			InitializeComponent ();
            SetSize(Children.Cast<StarView>());
            BindingContext = this;
        }

        private void SetSize(IEnumerable<StarView> enumerable)
        {
            if(enumerable == null)
            {
                foreach (var item in enumerable)
                {
                    if(StarWidth > 0 && StarHeight > 0)
                    {
                        item.WidthRequest = StarWidth;
                        item.HeightRequest = StarHeight;
                    }
                }
            }
        }

        public void Toggle(IEnumerable<StarView> views, double end)
        {
            if (end < 0 || end > 5) end = 0;
            if (views == null) return;
            for (int i = 0; i < (int) end; i++)
            {
                var star = views.ElementAt(i);
                var behavior = star.Behaviors.FirstOrDefault();
                if (behavior is ImageAsset imageasset)
                {
                    imageasset.Asset = "starfill.png";
                }
            }

            for (int i = (int) end; i < views.Count(); i++)
            {
                var star = views.ElementAt(i);
                var behavior = star.Behaviors.FirstOrDefault();
                if (behavior is ImageAsset imageasset)
                {
                    var resta = end - i;
                    if (resta <= 0)
                    {
                        imageasset.Asset = "starempty.png";
                    }
                    else
                    {
                        imageasset.Asset = "starhalfempty.png";
                    }
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!CanChange)
            {
                return;
            }
            if (sender is StarView star)
            {
                var position = star.Value;
                Value = position;
                if(BindingContext is StarsReviewModel model)
                {
                    model.Value = position;
                }
            }
        }
    }
}