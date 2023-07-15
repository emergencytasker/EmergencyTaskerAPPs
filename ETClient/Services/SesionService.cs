using Blazored.Toast.Services;
using ETClient.API.Enum;
using ETClient.API.ER;
using ETClient.Models;
using ETClient.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace ETClient.Services
{
    public class SesionService
    {
        public static EventHandler<object> StateReturn;

        public SesionService()
        {
        }

        public async Task<SesionModel> Read(Blazored.SessionStorage.ISessionStorageService storage)
        {
            SesionModel model = new SesionModel();
            var token = await storage.GetItemAsync<string>("session");

            if (!string.IsNullOrEmpty(token))
            {
                var user = JsonConvert.DeserializeObject<User>(token.Base64Decode());

                if (user != null)
                {
                   
                    model.Authenticated = true;
                    model.User = user;
                }
            }
            StateReturn?.Invoke(this, model);
            return model;
        }

        public async Task<SesionModel> Save(Blazored.SessionStorage.ISessionStorageService storage, User user)
        {
            SesionModel model = new SesionModel();
         
            await storage.SetItemAsync<string>("session", JsonConvert.SerializeObject(user).Base64Encode());
            model.Authenticated = true;
            model.User = user;
            StateReturn?.Invoke(this, model);
            return model;
        }

        public async Task<SesionModel> Remove(Blazored.SessionStorage.ISessionStorageService storage)
        {
            SesionModel model = new SesionModel();
            await storage.RemoveItemAsync("session");
            model.Authenticated = false;
            model.User = null;
            StateReturn?.Invoke(this, model);
            return model;
        }
    }
}
