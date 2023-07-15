using EmergencyTask.Model;
using EmergencyTask.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class NewCategoryViewModel : ViewModelBase
    {

        #region Notified Property Categorias
        /// <summary>
        /// Categorias
        /// </summary>
        private List<CommonModel> categorias;
        public List<CommonModel> Categorias
        {
            get { return categorias; }
            set { categorias = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Categoria
        /// <summary>
        /// Categoria
        /// </summary>
        private CommonModel categoria;
        public CommonModel Categoria
        {
            get { return categoria; }
            set { categoria = value; OnPropertyChanged(); if (value != null) { FilterSubCategories(value.Id); } }
        }

        private void FilterSubCategories(int id)
        {
            if (Subcategorias == null) return;
            Subcategoria = null;
            Subcategorias.Clear();
            foreach (var item in SubcategoriasSource)
            {
                if(item.Categoria == id)
                {
                    Subcategorias.Add(item);
                }
            }
            CommitService?.ChangeCanExecute();
        }
        #endregion

        #region Notified Property Subcategoria
        /// <summary>
        /// Subcategoria
        /// </summary>
        private CommonModel subcategoria;
        public CommonModel Subcategoria
        {
            get { return subcategoria; }
            set { subcategoria = value; OnPropertyChanged(); if (value != null) { SubcategorySelected(value); } }
        }

        private void SubcategorySelected(CommonModel model)
        {
            CommitService?.ChangeCanExecute();
        }
        #endregion

        #region Notified Property GoBack
        /// <summary>
        /// GoBack
        /// </summary>
        private ICommand goback;
        public ICommand GoBack
        {
            get { return goback; }
            set { goback = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Subcategorias
        /// <summary>
        /// Subcategorias
        /// </summary>
        private ObservableCollection<CommonModel> subcategorias;
        public ObservableCollection<CommonModel> Subcategorias
        {
            get { return subcategorias; }
            set { subcategorias = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property SubcategoriasSource
        /// <summary>
        /// SubcategoriasSource
        /// </summary>
        private List<CommonModel> subcategoriassource;
        public List<CommonModel> SubcategoriasSource
        {
            get { return subcategoriassource; }
            set { subcategoriassource = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property Id
        /// <summary>
        /// Id
        /// </summary>
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
        #endregion

        #region Notified Property CommitService
        /// <summary>
        /// CommitService
        /// </summary>
        private Command commitservice;
        public Command CommitService
        {
            get { return commitservice; }
            set { commitservice = value; OnPropertyChanged(); }
        }
        #endregion

        public OfferServiceModel Service { get; set; }
        public int IdUsuario { get; internal set; }

        public event EventHandler<OfferServiceModel> Commit;

        public NewCategoryViewModel(IEnumerable<CommonModel> categoriassource, IEnumerable<CommonModel> subcategoriassource, CommonModel categoriadefault = null, CommonModel subcategoriadefault = null, double costo = 0)
        {
            if (categoriassource == null) throw new ArgumentNullException(nameof(categoriassource), AppResource.NullList);
            if (subcategoriassource == null) throw new ArgumentNullException(nameof(subcategoriassource), AppResource.NullList);

            Categorias = categoriassource.ToList();
            SubcategoriasSource = subcategoriassource.ToList();

            Subcategorias = new ObservableCollection<CommonModel>(); 
            Categoria = categoriadefault;
            Subcategoria = subcategoriadefault;
            CommitService = new Command(() =>
            {
                IsBusy = true;

                if(Categoria == null)
                {
                    Toast(AppResource.SeleccionaCategoria);
                    IsBusy = false;
                    return;
                }

                if (Subcategoria == null)
                {
                    Toast(AppResource.SeleccionaSubcategoria);
                    IsBusy = false;
                    return;
                }

                Commit?.Invoke(this, new OfferServiceModel
                {
                    Categoria = Categoria.Id,
                    Subcategoria = Subcategoria.Id,
                    Id = Id,
                    IdUsuario = IdUsuario
                });

                IsBusy = false;
            }, () =>
            {
                return Categoria != null && Subcategoria != null;
            });

            GoBack = new Command(async () => 
            {
                try
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                }
                catch { }
            });
        } 
    }
}
