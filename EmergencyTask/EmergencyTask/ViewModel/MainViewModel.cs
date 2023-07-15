using EmergencyTask.Model;
using EmergencyTask.Strings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        #region BindableProperty TapComenzar
        /// <summary>
        /// BtnComenzar de la propiedad bindable
        /// </summary>
        private Command tapcomenzar;
        public Command TapComenzar
        {
            get { return tapcomenzar; }
            set { tapcomenzar = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Carousel
        /// <summary>
        /// Carousel de la propiedad bindable
        /// </summary>
        private ObservableCollection<CarouselModel> carousel;
        public ObservableCollection<CarouselModel> Carousel
        {
            get { return carousel; }
            set { carousel = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Card
        /// <summary>
        /// Card de la propiedad bindable
        /// </summary>
        private CarouselModel card;
        public CarouselModel Card
        {
            get { return card; }
            set { card = value; OnPropertyChanged(); OnCarouselModelChanged(value); }
        }

        #region BindableProperty CardChanged
        /// <summary>
        /// CardChanged de la propiedad bindable
        /// </summary>
        private Command cardchanged;
        public Command CardChanged
        {
            get { return cardchanged; }
            set { cardchanged = value; OnPropertyChanged(); }
        }
        #endregion

        private void OnCarouselModelChanged(CarouselModel model)
        {
            if(model != null)
            {
                var index = Carousel.IndexOf(model);
                if(Circles != null)
                {
                    for (int i = 0; i < Circles.Count(); i++)
                    {
                        var circle = Circles.ElementAt(i);
                        if(index == i)
                        {
                            Device.BeginInvokeOnMainThread(() => circle.Color = (Color) App.Resources["Accent"]);
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() => circle.Color = Color.LightGray);
                        }
                    }
                }
            }
        }
        #endregion

        public MainViewModel()
        {
            TapComenzar = new Command(TapComenzar_Clicked);
            CardChanged = new Command(CardChanged_Clicked);
            Carousel = new ObservableCollection<CarouselModel>
            {
                new CarouselModel
                {
                    Wallpaper = "https://cdn.pixabay.com/photo/2015/12/07/10/49/electrician-1080554_960_720.jpg",
                    Icon = "icon.png",
                    Text = AppResource.AyudaBuscas,
                    Animation = "Hello.json"
                },

                new CarouselModel
                {
                    Wallpaper = "https://cdn.pixabay.com/photo/2017/11/29/11/03/ecology-2985781_960_720.jpg",
                    Icon = "icon.png",
                    Text = AppResource.TenSeguridad,
                    Animation = "security.json",
                },

                new CarouselModel
                {
                    Wallpaper = "https://cdn.pixabay.com/photo/2015/04/20/13/30/kitchen-731351_960_720.jpg",
                    Icon = "icon.png",
                    Text = AppResource.ApoyoMereces,
                    Animation = "Navigation.json",
                },

                new CarouselModel
                {
                    Wallpaper = "https://cdn.pixabay.com/photo/2017/06/28/04/29/board-2449726_960_720.jpg",
                    Icon = "icon.png",
                    Text = AppResource.Bienvenido,
                    Animation = "Clean.json",
                }
                
            };

            MessagingCenter.Instance.Subscribe<App, bool>(App, "IsBusy", (app, param) => IsBusy = param);

        }

        private void CardChanged_Clicked(object obj)
        {
            if(obj is CarouselModel model)
            {
                OnCarouselModelChanged(model);
            }
        }

        public IEnumerable<BoxView> Circles { get; set; }

        public void SetCircles(IEnumerable<BoxView> circles)
        {
            Circles = circles;
        }

        private async void TapComenzar_Clicked(object obj)
        {
            await Navigation.PushAsync(new LoginPage());
        }

    }
}
