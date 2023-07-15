using EmergencyTask.API;
using EmergencyTask.Model;
using EmergencyTask.Strings;
using Plugin.PdfRasterizer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class PDFRenderViewModel : ViewModelBase
    {

        #region BindableProperty Term
        /// <summary>
        /// Term de la propiedad bindable
        /// </summary>
        private TermModel term;
        public TermModel Term
        {
            get { return term; }
            set { term = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty Terms
        /// <summary>
        /// Terms de la propiedad bindable
        /// </summary>
        private ObservableCollection<TermModel> terms;
        public ObservableCollection<TermModel> Terms
        {
            get { return terms; }
            set { terms = value; OnPropertyChanged(); }
        }
        #endregion
        
        #region BindableProperty BtnAnterior
        /// <summary>
        /// BtnAnterior de la propiedad bindable
        /// </summary>
        private Command btnanterior;
        public Command BtnAnterior
        {
            get { return btnanterior; }
            set { btnanterior = value; OnPropertyChanged(); }
        }
        #endregion

        #region BindableProperty BtnSiguiente
        /// <summary>
        /// BtnSiguiente de la propiedad bindable
        /// </summary>
        private Command btnsiguiente;
        public Command BtnSiguiente
        {
            get { return btnsiguiente; }
            set { btnsiguiente = value; OnPropertyChanged(); }
        }
        #endregion

        public int Index { get; set; }

        public PDFRenderViewModel()
        {

        }

        public override async void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            IsBusy = true;
            var images = await GetPdf();
            Terms = new ObservableCollection<TermModel>(images.Select(i => new TermModel
            {
                TermPage = i
            }));
            BtnAnterior = new Command(BtnAnterior_Clicked);
            BtnSiguiente = new Command(BtnSiguiente_Clicked);
            IsBusy = false;
        }

        private async Task<IEnumerable<string>> GetPdf()
        {
            try
            {
                string documentUrl = $"http://142.11.222.110/pdf/terminos.pdf";
                Debug.WriteLine($"[GetPdf] {documentUrl}");
                var rasterizer = CrossPdfRasterizer.Current;
                var document = await rasterizer.RasterizeAsync(documentUrl);
                return document.Pages.Select((p) => p.Path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to rasterize provided document: " + ex);
            }
            return new string[0];
        }

        private void BtnSiguiente_Clicked(object obj)
        {
            if (Term == null) return;
            if (Index >= Terms.Count - 1)
            {
                Finish();
                return;
            }
            Index++;
            Term = Terms[Index];
        }

        private async void Finish()
        {
            if (!await Confirm(AppResource.LeisteContrato)) return;
            await Navigation.PopAsync();
        }

        private void BtnAnterior_Clicked(object obj)
        {
            if (Term == null) return;
            if (Index <= 0) return;
            Index--;
            Term = Terms[Index];
        }
    }
}
