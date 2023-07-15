using FFImageLoading.Forms;
using System.Reflection;
using Xamarin.Forms;

namespace EmergencyTask.Views.Rating.Images
{
    public class ImageAsset : Behavior<CachedImage>
    {

        public static readonly BindableProperty AssetProperty = BindableProperty.Create("Asset", typeof(string), typeof(ImageAsset), "starfill.png", propertyChanged: OnAssetPropertyChanged);

        private static void OnAssetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ImageAsset imageasset)
            {
                if(newValue != null)
                {
                    imageasset.Asset = newValue.ToString();
                }
            }
        }

        private string asset;
        public string Asset
        {
            get { return asset; }
            set { asset = value; OnPropertyChanged(); ChangeSource(); }
        }

        public const string StandarName = "EmergencyTask.View.Rating.Images";

        public CachedImage Current { get; set; }

        public ImageAsset()
        {
            BindingContext = this;
        }

        protected override void OnAttachedTo(CachedImage bindable)
        {
            base.OnAttachedTo(bindable);
            Current = bindable;
            BindingContext = Current.BindingContext;
            ChangeSource();
        }

        private void ChangeSource()
        {
            if (!string.IsNullOrEmpty(Asset) && Current != null)
            {
                var imageSource = ImageSource.FromResource(StandarName + "." + Asset, typeof(ImageAsset).GetTypeInfo().Assembly);
                Current.Source = imageSource;
            }
        }

        protected override void OnDetachingFrom(CachedImage bindable)
        {
            base.OnDetachingFrom(bindable);
        }

    }
}