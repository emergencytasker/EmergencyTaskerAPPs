using EmergencyTask.Model;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class JsonViewModel : ViewModelBase
    {

        #region Notified Property Json
        /// <summary>
        /// Json
        /// </summary>
        private string json;
        public string Json
        {
            get { return json; }
            set { json = value; OnPropertyChanged(); }
        }
        #endregion

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            var data = DataBase.GetVariable();
            Json = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }

    }
}
