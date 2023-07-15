using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class ScheduleEditViewModel : ViewModelBase
    {

        #region BindableProperty Horarios
        /// <summary>
        /// Horarios de la propiedad bindable
        /// </summary>
        private ObservableCollection<ScheduleDayModel> horarios;
        public ObservableCollection<ScheduleDayModel> Horarios
        {
            get { return horarios; }
            set { horarios = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnSave
        /// <summary>
        /// BtnSave de la propiedad bindable
        /// </summary>
        private Command btnsave;
        public Command BtnSave
        {
            get { return btnsave; }
            set { btnsave = value; OnPropertyChanged(); }
        }
        #endregion

        public ScheduleEditViewModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            BtnSave = new Command(BtnSave_Clicked);

            var me = Usuario.GetUserLogin();
            if (me == null) return;

            var agenda = await Client.Calendar.Where(new API.ER.Calendar
            {
                idusuario = me.id
            });

            var list = new List<ScheduleDayModel>();
            foreach (DayOfWeek item in Enum.GetValues(typeof(DayOfWeek)))
            {
                var dia = 0;

                switch (item)
                {
                    case DayOfWeek.Friday:
                        dia = 6;
                        break;
                    case DayOfWeek.Monday:
                        dia = 2;
                        break;
                    case DayOfWeek.Saturday:
                        dia = 7;
                        break;
                    case DayOfWeek.Sunday:
                        dia = 1;
                        break;
                    case DayOfWeek.Thursday:
                        dia = 5;
                        break;
                    case DayOfWeek.Tuesday:
                        dia = 3;
                        break;
                    case DayOfWeek.Wednesday:
                        dia = 4;
                        break;
                    default:
                        break;
                }

                var day = item.ToString();

                var model = new ScheduleDayModel
                {
                    Day = item.ToString(),
                    AddSchedule = new Command(AddSchedule),
                    Schedules = new ObservableCollection<ScheduleModel>(),
                    NumberDay = dia
                };

                list.Add(model);

                var daydatabase = agenda.Where(a => a.dia == dia);
                if (daydatabase == null) continue;

                foreach (var itemdatabase in daydatabase)
                {
                    model.Add(new ScheduleModel
                    {
                        Fin = TimeSpan.Parse(itemdatabase.fin),
                        Inicio = TimeSpan.Parse(itemdatabase.inicio),
                        Id = itemdatabase.id,
                        NumberDay = itemdatabase.dia,
                        Delete = new Command(DeleteSchedule)
                    });
                }
            }
            Horarios = new ObservableCollection<ScheduleDayModel>(list);
        }

        private async void DeleteSchedule(object obj)
        {
            if (!(obj is ScheduleModel model)) return;
            var horario = Horarios.FirstOrDefault(h => h.NumberDay == model.NumberDay);
            if (horario == null) return;
            if (!await Confirm(AppResource.SeguroDeEliminarElHorario)) return;
            horario.Remove(model);
        }

        private void AddSchedule(object obj)
        {
            if (!(obj is ScheduleDayModel model)) return;
            if (model.Schedules.Count < 2)
            {
                model.Add(new ScheduleModel
                {
                    Inicio = new TimeSpan(9, 0, 0),
                    Fin = new TimeSpan(12, 0, 0),
                    NumberDay = model.NumberDay
                });
            }
            else
            {
                Toast(AppResource.LimiteHorario);
            }
        }

        private async void BtnSave_Clicked(object obj)
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;

            IsBusy = true;
            var horario = Horarios.FirstOrDefault(h => !h.IsValid());
            
            if (horario != null)
            {
                Toast(string.Format(AppResource.HorarioInvalido, horario.Day));
                IsBusy = false;
                return;
            }

            var successcount = 0;

            foreach (var item in Horarios)
            {
                foreach (var schedule in item.Schedules)
                {
                    if (schedule.Id > 0)
                    {
                        var updated = await Client.Calendar.Update(schedule.Id, new Dictionary<string, string>
                        {
                            { nameof(API.ER.Calendar.inicio), schedule.Inicio.ToString() },
                            { nameof(API.ER.Calendar.fin), schedule.Fin.ToString() },
                            { nameof(API.ER.Calendar.dia), schedule.NumberDay.ToString() },
                            { nameof(API.ER.Calendar.eliminado), item.Estado ? "0" : "1" }
                        });

                        if (updated == null || updated.inicio != schedule.Inicio.ToString() || updated.fin != schedule.Fin.ToString()) continue;
                        
                        successcount++;
                    }
                    else
                    {
                        var inserted = await Client.Calendar.Add(new API.ER.Calendar
                        {
                            inicio = schedule.Inicio.ToString(),
                            fin = schedule.Fin.ToString(),
                            idusuario = me.id,
                            dia = item.NumberDay
                        });

                        if (inserted == null || inserted.id <= 0) continue;

                        schedule.Id = inserted.id;

                        successcount++;
                    }
                }
            }

            if(Horarios.Sum(h => h.Schedules.Count) == successcount)
            {
                Toast(AppResource.HorariosGuardados);
                MessagingCenter.Instance.Send(Application.Current, "ReloadSchedule");
            }
            else
            {
                Toast(AppResource.ErrorAlGuardarLosHorarios);
            }

            IsBusy = false;
        }
    }
}
