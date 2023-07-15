using FFImageLoading.Forms;
using FFImageLoading.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EmergencyTask.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageLoading : Grid
    {


        #region BindableProperty Transformations
        public static readonly BindableProperty TransformationsProperty = BindableProperty.Create(nameof(Transformations), typeof(List<ITransformation>), typeof(CachedImage), new List<ITransformation>());
        public List<ITransformation> Transformations
        {
            get
            {
                return (List<ITransformation>)GetValue(TransformationsProperty);
            }
            set
            {
                SetValue(TransformationsProperty, value);
                if (value == null || FFImage == null) return;
                FFImage.Transformations = value;
            }
        }
        #endregion

        #region BindableProperty Source
        /// <summary>
        /// Source
        /// </summary>
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(Xamarin.Forms.ImageSource), typeof(ImageLoading), null, BindingMode.OneWay);

        /// <summary>
        /// Source
        /// </summary>
        public Xamarin.Forms.ImageSource Source
        {
            get
            {
                return (Xamarin.Forms.ImageSource)GetValue(SourceProperty);
            }

            set
            {
                SetValue(SourceProperty, value);
            }
        }
        #endregion

        #region BindableProperty Aspect
        /// <summary>
        /// Aspcet of image
        /// </summary>
        public static readonly BindableProperty AspectProperty = BindableProperty.Create(nameof(Aspect), typeof(Aspect), typeof(ImageLoading), Aspect.Fill, BindingMode.OneWay);

        /// <summary>
        /// Aspcet of image
        /// </summary>
        public Aspect Aspect
        {
            get
            {
                return (Aspect)GetValue(AspectProperty);
            }

            set
            {
                SetValue(AspectProperty, value);
            }
        }
        #endregion

        #region BindableProperty LoadingPlaceHolder
        /// <summary>
        /// Description of property
        /// </summary>
        public static readonly BindableProperty LoadingPlaceHolderProperty = BindableProperty.Create(nameof(LoadingPlaceHolder), typeof(string), typeof(ImageLoading), default(string), BindingMode.OneWay);

        /// <summary>
        /// Description of property
        /// </summary>
        public string LoadingPlaceHolder
        {
            get
            {
                return (string)GetValue(LoadingPlaceHolderProperty);
            }

            set
            {
                SetValue(LoadingPlaceHolderProperty, value);
            }
        }
        #endregion

        #region BindableProperty ErrorPlaceHolder
        /// <summary>
        /// Description of property
        /// </summary>
        public static readonly BindableProperty ErrorPlaceHolderProperty = BindableProperty.Create(nameof(ErrorPlaceHolder), typeof(Xamarin.Forms.ImageSource), typeof(ImageLoading), null, BindingMode.OneWay);

        /// <summary>
        /// Description of property
        /// </summary>
        public Xamarin.Forms.ImageSource ErrorPlaceHolder
        {
            get
            {
                return (Xamarin.Forms.ImageSource)GetValue(ErrorPlaceHolderProperty);
            }

            set
            {
                SetValue(ErrorPlaceHolderProperty, value);
            }
        }
        #endregion


        public ImageLoading()
        {
            InitializeComponent();
            Transformations = new List<ITransformation>();
            BindableProperties = new List<BindablePropertyChanged>
            {
                new BindablePropertyChanged(SourceProperty, () => FFImage.Source = Source),
                new BindablePropertyChanged(AspectProperty, () => FFImage.Aspect = Aspect),
                new BindablePropertyChanged(TransformationsProperty, () =>
                {
                    if(FFImage?.Source != null)
                    {
                        FFImage.Transformations = Transformations;
                        FFImage.ReloadImage();
                    }
                }),
                new BindablePropertyChanged(LoadingPlaceHolderProperty, () =>
                {
                    var placeholder = Device.RuntimePlatform == Device.UWP ? $"Assets/{LoadingPlaceHolder}" : LoadingPlaceHolder;
                    LottieLoading.Animation = placeholder;
                }),
                new BindablePropertyChanged(ErrorPlaceHolderProperty, () => FFImage.ErrorPlaceholder = ErrorPlaceHolder),
                new BindablePropertyChanged(WidthRequestProperty, () => SetWidthRequest(WidthRequest)),
                new BindablePropertyChanged(HeightRequestProperty, () => SetHeightRequest(HeightRequest))
            };
        }

        private void SetHeightRequest(double value)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LottieLoading.HeightRequest = FFImage.HeightRequest = value;
            });
        }

        private void SetWidthRequest(double value)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LottieLoading.WidthRequest = FFImage.WidthRequest = value;
            });
        }

        private void FFImage_Success(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            HideLottie();
        }

        private void FFImage_Error(object sender, CachedImageEvents.ErrorEventArgs e)
        {
            HideLottie();
        }

        private void FFImage_DownloadProgress(object sender, CachedImageEvents.DownloadProgressEventArgs e)
        {
            PlayLottie();
        }

        private void HideLottie()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (string.IsNullOrEmpty(LottieLoading.AnimationSource.ToString())) return;
                LottieLoading.StopAnimation();
                LottieLoading.IsVisible = false;
            });
        }

        private void PlayLottie()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                LottieLoading.PlayAnimation();
            });
        }

        #region Bindable Property Changed
        public List<BindablePropertyChanged> BindableProperties { get; set; }
        protected override void OnPropertyChanged(string propertyname = null)
        {
            base.OnPropertyChanged(propertyname);
            if (BindableProperties == null) return;
            BindableProperties.FirstOrDefault(b => b.PropertyChanged(propertyname));
        }

        public class BindablePropertyChanged
        {
            protected BindableProperty SourceProperty;
            protected Action Action { get; set; }
            public BindablePropertyChanged(BindableProperty sourceproperty, Action action)
            {
                SourceProperty = sourceproperty;
                Action = action;
            }

            public bool PropertyChanged(string propertyname)
            {
                if (SourceProperty.PropertyName != propertyname) return false;
                if (Action == null) return false;
                Device.BeginInvokeOnMainThread(Action);
                return true;
            }
        }
        #endregion
    }
}