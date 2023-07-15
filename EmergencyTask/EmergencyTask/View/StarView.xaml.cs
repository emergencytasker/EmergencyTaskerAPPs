using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Simsa.Views.Rating
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StarView : CachedImage
    {
        public static readonly BindableProperty AssetProperty = BindableProperty.Create("Asset", typeof(string), typeof(StarView), "starfill.png", propertyChanged: OnAssetProperty);

        private static void OnAssetProperty(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is StarView view)
            {
                if(newValue != null)
                {
                    view.Asset = newValue.ToString();
                }
            }
        }

        private string asset;
        public string Asset
        {
            get { return asset; }
            set { asset = value; OnPropertyChanged(); }
        }

        public static readonly BindableProperty StarHeightProperty = BindableProperty.Create("StarHeight", typeof(double), typeof(StarView), 30.0D, propertyChanged: OnStarHeightChanged);

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

        public static readonly BindableProperty StarWidthProperty = BindableProperty.Create("StarWidth", typeof(double), typeof(StarView), 30.0D, propertyChanged: OnStarWidthChanged);

        private static void OnStarWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarsReview view)
            {
                if (newValue != null)
                {
                    if (double.TryParse(newValue.ToString(), out double size))
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

        #region BindableProperty Value
        /// <summary>
        /// Propiedad bindable
        /// </summary>
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(int), typeof(StarView), 0, propertyChanged: OnValuePropertyChanged);
        private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarView view)
            {
                if (newValue != null)
                {
                    view.Value = (int) newValue;
                }
            }
        }

        /// <summary>
        /// Nombre de la propiedad bindable
        /// </summary>
        private int myvalue;
        public int Value
        {
            get { return myvalue; }
            set { myvalue = value; OnPropertyChanged(); }
        }
        #endregion

        public StarView ()
		{
			InitializeComponent ();
            BindingContext = this;
		}
	}
}