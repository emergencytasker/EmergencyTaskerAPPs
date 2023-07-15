using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmergencyTask.ViewModel
{
    public class NavigationService : INavigation
    {
        private INavigation Instance { get; }

        public NavigationService(INavigation navigation)
        {
            Instance = navigation;
        }

        public IReadOnlyList<Page> ModalStack => Instance.ModalStack;

        public IReadOnlyList<Page> NavigationStack => Instance.NavigationStack;

        public void InsertPageBefore(Page page, Page before)
        {
            Instance.InsertPageBefore(page, before);
        }

        public async Task<Page> PopAsync()
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                return await Instance.PopAsync();
            });
        }

        public async Task<Page> PopAsync(bool animated)
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                return await Instance.PopAsync(animated);
            });
        }

        public async Task<Page> PopModalAsync()
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                return await Instance.PopModalAsync();
            });
        }

        public async Task<Page> PopModalAsync(bool animated)
        {
            return await Device.InvokeOnMainThreadAsync(async () =>
            {
                return await Instance.PopModalAsync(animated);
            });
        }

        public async Task PopToRootAsync()
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Instance.PopToRootAsync();
            });
        }

        public async Task PopToRootAsync(bool animated)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Instance.PopToRootAsync(animated);
            });
        }

        public async Task PushAsync(Page page)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Instance.PushAsync(page);
            });
        }

        public async Task PushAsync(Page page, bool animated)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Instance.PushAsync(page, animated);
            });
        }

        public async Task PushModalAsync(Page page)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Instance.PushModalAsync(page);
            });
        }

        public async Task PushModalAsync(Page page, bool animated)
        {
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Instance.PushModalAsync(page, animated);
            });
        }

        public void RemovePage(Page page)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Instance.RemovePage(page);
            });
        }
    }
}
