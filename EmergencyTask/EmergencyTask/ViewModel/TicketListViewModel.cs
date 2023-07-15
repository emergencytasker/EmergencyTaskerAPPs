using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Rg.Plugins.Popup.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class TicketListViewModel : ViewModelBase
    {

        public string DefaultTextForDescription = AppResource.HazClickParaAgregarUnaDescripcion;

        #region BindableProperty Ticket
        /// <summary>
        /// Ticket de la propiedad bindable
        /// </summary>
        private TicketListModel ticket;
        public TicketListModel Ticket
        {
            get { return ticket; }
            set { ticket = value; OnPropertyChanged(); if (value != null) { OnTicketSelected(value); } }
        }
        #endregion

        #region BindableProperty Tickets
        /// <summary>
        /// Tickets de la propiedad bindable
        /// </summary>
        private ObservableCollection<TicketListModel> tickets;
        public ObservableCollection<TicketListModel> Tickets
        {
            get { return tickets; }
            set { tickets = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnAgregar
        /// <summary>
        /// BtnAgregar de la propiedad bindable
        /// </summary>
        private Command btnagregar;
        public Command BtnAgregar
        {
            get { return btnagregar; }
            set { btnagregar = value; OnPropertyChanged(); }
        }
        #endregion

        public int IdSolicitudServicio { get; set; }

        public TicketListViewModel(int idsolicitudservicio)
        {
            IdSolicitudServicio = idsolicitudservicio;
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;

            BtnAgregar = new Command(BtnAgregar_Clicked);
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return;
            var tickets = await Client.Ticket.Where(new Ticket
            {
                idsolicitudservicio = IdSolicitudServicio
            });

            Tickets = new ObservableCollection<TicketListModel>(tickets.Select(t => new TicketListModel
            {
                Description = string.IsNullOrEmpty(t.detalle) ? DefaultTextForDescription : t.detalle,
                Image = Client.GetPath(t.imagen),
                Id = t.id
            }));

            MessagingCenter.Instance.Subscribe<App, TicketListModel>(App, "Refresh", (app, model) =>
            {
                IsBusy = true;
                var exists = Tickets.FirstOrDefault(t => t.Id == model.Id);
                if (exists != null)
                {
                    exists.Description = string.IsNullOrEmpty(model.Description) ? DefaultTextForDescription : model.Description;
                    exists.Image = model.Image;
                    exists.Id = model.Id;
                }
                else
                {
                    Tickets.Add(new TicketListModel
                    {
                        Image = model.Image,
                        Description = string.IsNullOrEmpty(model.Description) ? DefaultTextForDescription : model.Description,
                        Id = model.Id
                    });
                }
                IsBusy = false;
            });

            IsBusy = false;
        }

        private async void OnTicketSelected(TicketListModel value)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PushPopupAsync(new NuevoTicketPopup
                {
                    BindingContext = new NuevoTicketViewModel(IdSolicitudServicio, new TicketListModel
                    {
                        Description = value.Description == DefaultTextForDescription ? "" : value.Description,
                        Id = value.Id,
                        Image = value.Image
                    })
                });
            });
        }

        private async void BtnAgregar_Clicked(object obj)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PushPopupAsync(new NuevoTicketPopup
                {
                    BindingContext = new NuevoTicketViewModel(IdSolicitudServicio)
                });
            });
        }
    }
}
