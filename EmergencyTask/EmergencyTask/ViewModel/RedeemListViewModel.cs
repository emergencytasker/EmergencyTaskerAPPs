using EmergencyTask.API;
using EmergencyTask.API.ER;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using EmergencyTask.View;
using EmergencyTask.ViewModel.Commands;
using EmergencyTask.ViewModel.Validators;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class RedeemListViewModel : ViewModelBase
    {

        #region BindableProperty Redeems
        /// <summary>
        /// Redeems de la propiedad bindable
        /// </summary>
        private ObservableCollection<RedeemModel> redeems;
        public ObservableCollection<RedeemModel> Redeems
        {
            get { return redeems; }
            set { redeems = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Referencia
        /// <summary>
        /// Referencia
        /// </summary>
        private string referencia;
        public string Referencia
        {
            get { return referencia; }
            set { referencia = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property BtnRegistrarReferido
        /// <summary>
        /// BtnRegistrarReferido
        /// </summary>
        private ExtendCommand btnregistrarreferido;
        public ExtendCommand BtnRegistrarReferido
        {
            get { return btnregistrarreferido; }
            set { btnregistrarreferido = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Codigo
        /// <summary>
        /// Codigo
        /// </summary>
        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ChangeReference
        /// <summary>
        /// ChangeReference
        /// </summary>
        private Command changereference;
        public Command ChangeReference
        {
            get { return changereference; }
            set { changereference = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property IsTasker
        /// <summary>
        /// IsTasker
        /// </summary>
        private bool istasker;
        public bool IsTasker
        {
            get { return istasker; }
            set { istasker = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property ShareCode
        /// <summary>
        /// ShareCode
        /// </summary>
        private Command sharecode;
        public Command ShareCode
        {
            get { return sharecode; }
            set { sharecode = value; OnPropertyChanged(); }
        }
        #endregion

        public IEnumerable<Rewards> RewardList { get; private set; }
        public List<Redeem> UserRedeems { get; private set; }
        public IEnumerable<RewardTranslate> Translates { get; private set; }
        public IExecuteValidator[] Validators { get; set; }

        public RedeemListViewModel()
        {
        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            if (!(page is RedeemListPage redeemlistpage)) return;

            IsBusy = true;

            if (!await EnsureApis())
            {
                return;
            }

            await GenerateReference();

            Validators = new IExecuteValidator[2]
            {
                new UserValidator(), new InternetValidator()
            };

            var me = Usuario.GetUserLogin();
            if (me == null) return;

            IsTasker = me.Perfil == API.Enum.Perfil.Tasker;

            Referencia = me.referencia;

            var notinuser = RewardList.Where(r => 
            {
                var exists = UserRedeems != null && !UserRedeems.Any(ru => ru.idrecompensas == r.id);
                var need = r.requierereferido == 0 && r.requierereferencia == 0;
                return exists && need;
            });

            var addedtome = await AddToUser(notinuser, me.id);
            if (addedtome != null) UserRedeems.AddRange(addedtome);

            RenderRedeems(UserRedeems);

            BtnRegistrarReferido = new ExtendCommand(BtnRegistrarReferido_Clicked, Validators);

            ChangeReference = new Command(ChangeReference_Command);
            ShareCode = new Command(ShareCode_Command);
            IsBusy = false;
        }

        private async void ShareCode_Command(object code)
        {
            if (code == null) return;
            IsBusy = true;
            var platform = Device.RuntimePlatform.ToLower();
            var url = $"https://web.emergencytasker.com/share";
            if (App.Perfil == API.Enum.Perfil.Client)
                url += $"/client/{code}";
            else
                url += $"/tasker/{code}";
            await Share.RequestAsync(url);
            IsBusy = false;
        }

        private async void ChangeReference_Command(object obj)
        {
            var code = obj.ToString();

            var me = Usuario.GetUserLogin();
            if (me == null)
            {
                Toast(AppResource.NoPodemosContinuar);
                return;
            }

            var newcode = await Promt<string>(AppResource.AquiPuedesCambiarTuCodigo, AppResource.Cancelar, AppResource.CambiarCodigo, Acr.UserDialogs.InputType.Default);
            if (string.IsNullOrEmpty(newcode))
            {
                Toast(AppResource.ElCodigoNoPuedeEstarVacio);
                return;
            }

            if(newcode.Length > 16)
            {
                Toast(AppResource.CodigoMuyGrande);
                return;
            }

            IsBusy = true;

            var update = await Client.User.Update(me.id, new Dictionary<string, string>
            {
                { nameof(User.referencia), newcode }
            });

            if(update == null || update.referencia != newcode)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }

            Referencia = me.referencia = newcode;
            Usuario.SetUserLogin(me);

            Toast(string.Format(AppResource.CodigoActualizado, newcode));
            IsBusy = false;
        }

        private async Task GenerateReference()
        {
            var me = Usuario.GetUserLogin();
            if (me == null) return;

            if (!string.IsNullOrEmpty(me.referencia)) return;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                id = me.id
            });

            var referencia = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            var update = await Client.User.Update(me.id, new Dictionary<string, string>
            {
                { "referencia", referencia }
            });

            if (update == null || update.referencia != referencia) return;

            me.referencia = referencia;
            Usuario.SetUserLogin(me);
        }

        /// <summary>
        /// Verifica que las apis se hayan ejecutado
        /// </summary>
        /// <returns></returns>
        private async Task<bool> EnsureApis()
        {
            var usuario = Usuario.GetUserLogin();
            if (usuario == null) return false;
            RewardList = await Client.Rewards.Where(new Rewards
            {
                idperfil = (int)App.Perfil
            });
            UserRedeems = (await Client.Redeem.Where(new Redeem
            {
                idusuario = usuario.id
            })).ToList();
            Translates = (await Client.GetRewardsTranslate(Usuario.GetUserLogin().lenguaje ?? Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName));
            return RewardList != null && RewardList.Count() > 0 && UserRedeems != null;
        }

        /// <summary>
        /// Asignar recompensas
        /// </summary>
        /// <param name="recompensausuario"></param>
        private void RenderRedeems(List<Redeem> recompensausuario)
        {
            if (recompensausuario == null || recompensausuario.Count() == 0) return;
            if (RewardList == null || RewardList.Count() == 0) return;
            foreach (var item in recompensausuario)
            {
                var recompensa = RewardList.FirstOrDefault(r => r.id == item.idrecompensas);
                if (recompensa == null) continue;
                var model = GetModel(item, recompensa);
                if (model == null) continue;
                AddRedeem(model);
            }
        }

        /// <summary>
        /// Agrega un Redeem
        /// </summary>
        /// <param name="redeemmodel"></param>
        public void AddRedeem(RedeemModel redeemmodel)
        {
            if (Redeems == null) Redeems = new ObservableCollection<RedeemModel>();
            if (Redeems.FirstOrDefault(r => r.IdReward == redeemmodel.IdReward) != null) return;
            var translate = Translates.FirstOrDefault(t => t.idrecompensa == redeemmodel.IdReward);
            if(translate != null)
            {
                redeemmodel.Title = translate.traduccion;
                redeemmodel.Detail = translate.descripcion;
            }
            Redeems.Add(redeemmodel);
        }

        /// <summary>
        /// Devuelve un redeem model
        /// </summary>
        /// <param name="item"></param>
        /// <param name="recompensa"></param>
        /// <param name="validators"></param>
        /// <returns></returns>
        private RedeemModel GetModel(Redeem item, Rewards recompensa)
        {
            if (item == null) return null;
            if (recompensa == null) return null;
            return new RedeemModel
            {
                Goal = item.variable,
                Now = item.realizado,
                Title = recompensa.nombre,
                Redeem = $"${recompensa.valor}",
                IsRedeedRewardVisible = item.reclamada == 0 && item.realizado / item.variable == 1,
                RedeemReward = new ExtendCommand(RedeemReward_Clicked, Validators),
                Id = item.id,
                IdReward = recompensa.id,
                Detail = recompensa.descripcion
            };
        }

        /// <summary>
        /// Accion para registrar un referido
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private async void BtnRegistrarReferido_Clicked(object arg1, IExecuteValidator[] arg2)
        {
            if (!arg2.TryGetComparator(out Usuario me)) return;

            IsBusy = true;
            if (!await EnsureApis()) 
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }

            if (string.IsNullOrEmpty(Codigo))
            {
                Toast(AppResource.CodigoInvalido);
                IsBusy = false;
                return;
            }

            if(Codigo == Referencia)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }

            foreach (var reward in RewardList.Where(r => r.requierereferencia == 1))
            {
                if (UserRedeems.Any(u => u.idrecompensas == reward.id))
                {
                    Toast(string.Format(AppResource.SeAgregoLaRecompensaDeLosTrabajos, reward.variable));
                    IsBusy = false;
                    return;
                }
            }

            // buscamos en las rewards de la app si ya se han agregado estas referencias
            var referencias = await Client.Redeem.Where(new Redeem
            {
                codigo = Codigo
            });

            if (UserRedeems.Any(u => u.codigo == Codigo) && referencias.Count() == 2)
            {
                Toast(AppResource.CodigoYaRegistrado);
                IsBusy = false;
                return;
            }

            var codeuserrewards = RewardList.Where(r => r.requierereferido == 1);
            var merewards = RewardList.Where(r => r.requierereferencia == 1);

            if (codeuserrewards.Count() == 0)
            {
                Toast(AppResource.NoPodemosContinuar);
                IsBusy = false;
                return;
            }

            var medb = await Client.User.Get(me.id);
            if (medb == null)
            {
                Toast(AppResource.SinContinuar);
                IsBusy = false;
                return;
            }

            var codeuser = (await Client.User.Where(new User
            {
                referencia = Codigo
            })).FirstOrDefault();

            if(codeuser == null || string.IsNullOrEmpty(codeuser.referencia))
            {
                Toast(AppResource.ReferenciaInvalida);
                IsBusy = false;
                return;
            }

            var addedtouser = await AddToUser(codeuserrewards, codeuser.id, me.id, Codigo);

            var addedtome = await AddToUser(merewards, me.id, codeuser.id, Codigo);

            UserRedeems.AddRange(addedtome);
            RenderRedeems(UserRedeems);

            Toast(AppResource.CodigoAgregado);

            IsBusy = false;
        }

        /// <summary>
        /// Agrega recompensas a un id de usuario
        /// </summary>
        /// <param name="rewards"></param>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        private async Task<List<Redeem>> AddToUser(IEnumerable<Rewards> rewards, int idusuario, int? idreferencia = null, string codigo = "")
        {
            if (rewards == null) return new List<Redeem>(0);
            if (rewards.Count() == 0) return new List<Redeem>(0);
            List<Redeem> redeems = new List<Redeem>(rewards.Count());
            foreach (var item in rewards)
            {
                if (UserRedeems == null || UserRedeems.Any(u => u.idrecompensas == item.id)) continue;
                var redeem = await Client.Redeem.Add(new Redeem
                {
                    idrecompensas = item.id,
                    idusuario = idusuario,
                    variable = item.variable,
                    trabajoterminado = item.trabajoterminado,
                    referencia = idreferencia,
                    codigo = codigo
                });
                if (redeem == null || redeem.id <= 0) continue;
                redeems.Add(redeem);
            }
            return redeems;
        }

        /// <summary>
        /// Accion para reclamar una recompensa
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="validators"></param>
        private async void RedeemReward_Clicked(object obj, IExecuteValidator[] validators)
        {
            if (!(obj is RedeemModel model)) return;
            IsBusy = false;

            var update = await Client.Redeem.Update(model.Id, new Dictionary<string, string>
            {
                { nameof(Redeem.reclamada), "1" }
            });

            if(update == null || update.reclamada != 1)
            {
                Toast(AppResource.RecompensaInvalida);
                IsBusy = false;
                return;
            }

            Toast(AppResource.VerificandoRecompensa);
            model.IsRedeedRewardVisible = false;

            IsBusy = false;
        }
    }
}
