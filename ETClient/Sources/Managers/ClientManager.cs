using ETClient.API.Enum;
using ETClient.API.ER;
using ETClient.Helpers;
using ETClient.Models;
using GoogleMapsComponents.Maps;
using Plugin.Net.Http;
using System.Linq.Expressions;
using System.Text;

namespace ETClient
{
    public class Client
    {

#if DEBUG_
        public const string BaseUrl = "http://192.168.1.113/emergencytasker/";
#else
        public const string BaseUrl = "http://app.emergencytasker.com/";
#endif

        public static string HOST { get { return BaseUrl; } }

        public static Dictionary<string, string> Headers = new Dictionary<string, string>
        {
            { "X-API-Key", "RW1lcmdlbmN5VGFza2Vy" }
        };

        public static IRestClient Restclient
        {
            get
            {
                var client = new RestClient(BaseUrl, Headers);
                return client;
            }
        }

        public static IApi<Reason> Reasons
        {
            get
            {
                return new Api<Reason>(Restclient);
            }
        }

        public static IApi<Log> Log
        {
            get { return new Api<Log>(Restclient); }
        }

        public static IApi<AppCenter> AppCenter
        {
            get { return new Api<AppCenter>(Restclient); }
        }

        public static IApi<Accessory> Accessory
        {
            get { return new Api<Accessory>(Restclient); }
        }

        public static IApi<Language> Language
        {
            get { return new Api<Language>(Restclient); }
        }

        public static IApi<User> User
        {
            get { return new Api<User>(Restclient); }
        }

        public static IApi<Balance> Balance
        {
            get { return new Api<Balance>(Restclient); }
        }

        public static IApi<Chat> Chat
        {
            get { return new Api<Chat>(Restclient); }
        }

        public static IApi<Cancelservice> Cenacelservice
        {
            get { return new Api<Cancelservice>(Restclient); }
        }

        public static IApi<API.ER.Calendar> Calendar
        {
            get { return new Api<Calendar>(Restclient); }
        }

        public static IApi<Category> Category
        {
            get { return new Api<Category>(Restclient); }
        }

        public static IApi<Notification> Notification
        {
            get { return new Api<Notification>(Restclient); }
        }

        public static IApi<Subcategory> Subcategory
        {
            get { return new Api<Subcategory>(Restclient); }
        }

        public static IApi<Ticket> Ticket
        {
            get { return new Api<Ticket>(Restclient); }
        }

        public static IApi<API.ER.Time> Time
        {
            get { return new Api<API.ER.Time>(Restclient); }
        }

        public static IApi<Hito> Hito
        {
            get { return new Api<Hito>(Restclient); }
        }

        public static IApi<Servicetraking> Servicetraking
        {
            get { return new Api<Servicetraking>(Restclient); }
        }

        public static IApi<Service> Service
        {
            get { return new Api<Service>(Restclient); }
        }

        public static IApi<Setting> Setting
        {
            get { return new Api<Setting>(Restclient); }
        }

        public static IApi<Stripeuser> Stripeuser
        {
            get { return new Api<Stripeuser>(Restclient); }
        }

        public static IApi<Requestservice> Requestservice
        {
            get { return new Api<Requestservice>(Restclient); }
        }

        public static IApi<Rewards> Rewards
        {
            get { return new Api<Rewards>(Restclient); }
        }

        public static IApi<Redeem> Redeem
        {
            get { return new Api<Redeem>(Restclient); }
        }

        public static IApi<Review> Review
        {
            get { return new Api<Review>(Restclient); }
        }

        public static IApi<Evidence> Evidence
        {
            get { return new Api<Evidence>(Restclient); }
        }

        public static IApi<Study> Study
        {
            get { return new Api<Study>(Restclient); }
        }

        public static IApi<Work> Work
        {
            get { return new Api<Work>(Restclient); }
        }

        public static IApi<Payout> Payout
        {
            get
            {
                return new Api<Payout>(Restclient);
            }
        }

        public static IApi<Galery> Galery
        {
            get
            {
                return new Api<Galery>(Restclient);
            }
        }

        public static async Task<API.Response.Auth> Auth(string email, string password, string lenguaje)
        {
            var auth = await Restclient.ExecuteTaskAsync<API.Response.Auth>($"api/auth", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("email", email, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("password", password, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("idperfil", 1, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("lenguaje", lenguaje, RestSharp.ParameterType.GetOrPost)
            });
            if (!auth.HasExecute) return new API.Response.Auth { code = -30, message = "Server Error", token = "", status = false, userid = 0 };
            return auth.Result ?? new API.Response.Auth { code = -40, message = "Message Error", token = "", status = false, userid = 0 };
        }

        public static string Path(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint)) return "/media/icon.png";
            return System.IO.Path.Combine(BaseUrl, endpoint);
        }

        public static async Task<IEnumerable<Categorylanguage>> GetCategoriesByLanguage(int idlenguaje)
        {
            var auth = await Restclient.ExecuteTaskAsync<List<Categorylanguage>>($"v1/categorylanguage", RestSharp.Method.GET, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idlenguaje", idlenguaje, RestSharp.ParameterType.GetOrPost),
            });
            if (!auth.HasExecute) return null;
            var list = auth.Result ?? new List<Categorylanguage>(0);
            return list;
        }

        public static async Task<IEnumerable<Subcategorylanguage>> GetSubCategoriesByLanguage(int idlenguaje)
        {
            var auth = await Restclient.ExecuteTaskAsync<List<Subcategorylanguage>>($"v1/subcategorylanguage", RestSharp.Method.GET, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idlenguaje", idlenguaje, RestSharp.ParameterType.GetOrPost),
            });
            if (!auth.HasExecute) return null;
            var list = auth.Result ?? new List<Subcategorylanguage>(0);
            return list;
        }

        public static void SetToken(string token)
        {
            if (Headers != null)
            {
                if (Headers.ContainsKey("Authorization"))
                {
                    Headers["Authorization"] = $"Bearer {token}";
                }
                else
                {
                    Headers.Add("Authorization", $"Bearer {token}");
                }
            }
        }
        
        public static async Task<UploadResponse> Upload(System.IO.Stream stream)
        {
            var bytearray = ConvertToArray(stream);
           
            return await Upload(bytearray);
        }
        
        public static async Task<UploadResponse> Upload(byte[] bytearray)
        {
            if (bytearray == null) return new UploadResponse { status = false, path = "", message = "data is null" };
            var uploadresponse = await Restclient.ExecuteTaskAsync<UploadResponse>($"api/upload", RestSharp.Method.POST, null, new List<Plugin.Net.Http.File>
            {
                new Plugin.Net.Http.File { Bytes = bytearray, FileName = "file.jpg", Name = "image" }
            });
            if (!uploadresponse.HasExecute) return new UploadResponse { message = "", status = false, path = "" };
            return uploadresponse.Result ?? new UploadResponse { message = "", status = false, path = "" };
        }
        
        /*
        /// <summary>
        /// Envia un sms
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static bool SendSms(string phone, string text, string from)
        {
            try
            {
                var twilio_sid = "ACeb532959c1574114021e592fefd27043";
                var twilio_token = "8f91b627724932ae1da54fc33a3c0381";
                TwilioClient.Init(twilio_sid, twilio_token);
                var message = MessageResource.Create(
                    new PhoneNumber(phone),
                    from: new PhoneNumber("+12015844189"),
                    body: $"{from}, {text}"
                );

                if (message.ErrorCode.HasValue)
                {
                    Task.Run(async () =>
                    {
                        await Log.Add(new Log
                        {
                            method = "SendSms",
                            process = "Send SMS",
                            controller = "Client",
                            parameters = phone,
                            response = message.ErrorMessage
                        });
                    });
                }

                return !message.ErrorCode.HasValue;
            }
            catch (Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }
            return false;
        }
        */
        public static async Task<bool> VerificationMail(int id, string email)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new API.Request.EmailVerified
            {
                id = id
            });
            var referencia = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            var update = await User.Update(id, new Dictionary<string, string>
            {
                { "referencia", referencia }
            });
            if (update != null && update.referencia == referencia)
            {
                var url = GetPath($"active/{referencia}");
                return await SendMail(email, "Activar cuenta de emergency tasker", $"Activa tu cuenta usando el siguiente link: {url}");
            }
            return false;
        }

        /// <summary>
        /// Envia un email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<bool> SendMail(string email, string subject, string message)
        {
            var uploadresponse = await Restclient.ExecuteTaskAsync<string>($"api/email", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("email", email, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("subject", subject, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("message", message, RestSharp.ParameterType.GetOrPost)
            });
            if (!uploadresponse.HasExecute) return false;
            return uploadresponse.Result == "1";
        }

        public static async Task<T> GetVar<T>(string key, int idperfil)
        {
            var setting = (await Setting.Where(new Setting
            {
                opcion = key,
                idperfil = idperfil
            })).FirstOrDefault();
            if (setting == null) return default(T);
            return setting.As<T>();
        }

        public static byte[] ConvertToArray(System.IO.Stream stream)
        {
            if (stream == null) return null;
            byte[] buffer = new byte[16 * 1024];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        
        public static string GetPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "/media/icon.png";
            }
            return BaseUrl + path;
        }

        public static byte[] DownloadData(string imageurl)
        {
            try
            {
                RestSharp.RestClient client = new RestSharp.RestClient();
                RestSharp.RestRequest request = new RestSharp.RestRequest(imageurl, RestSharp.Method.GET);
                return client.DownloadData(request);
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Actualiza el estado de un servicio
        /// </summary>
        /// <param name="id">Id Del Servicio</param>
        /// <param name="estado">Nuevo Estado</param>
        /// <returns></returns>
        public static async Task<bool> ChangeServiceStatus(int id, EstadoServicio estado, int idusuario, double latitud, double longitud, int tieneubicacion)
        {
            var estadodeservicio = (int)estado;
            var currentservice = await Requestservice.Get(id);
            if (currentservice == null) return false;
            if (currentservice.idestadoservicio == (int)estado) return true;
            var requestservice = await Requestservice.Update(id, new Dictionary<string, string>
            {
                { nameof(API.ER.Requestservice.idestadoservicio), estadodeservicio.ToString() }
            });
            var updated = requestservice != null && requestservice.idestadoservicio == (int)estado;
            if (updated)
            {
                await Servicetraking.Add(new Servicetraking
                {
                    idestadoservicio = estadodeservicio,
                    idsolicitudservicio = id,
                    idusuario = idusuario,
                    latitud = latitud,
                    longitud = longitud,
                    tieneubicacion = tieneubicacion
                });
            }
            return updated;
        }

        /// <summary>
        /// Devuelve el id de cargo actual
        /// </summary>
        /// <param name="idsolicitudservicio"></param>
        /// <returns></returns>
        public static async Task<string> GetCurrentChargeId(int idsolicitudservicio)
        {
            var hito = await GetCurrentHito(idsolicitudservicio);
            if (hito == null) return null;
            return hito.chargeid;
        }

        /// <summary>
        /// Devuelve el hito actual del servicio
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Hito> GetCurrentHito(int idsolicitudservicio)
        {
            var hito = await Hito.Where(new Hito
            {
                trabajoterminado = 1,
                idsolicitudservicio = idsolicitudservicio
            }) ?? new List<Hito>();
            var pendingforpay = hito.OrderByDescending(h => h.id).FirstOrDefault(h => h.estado == (int)HitoStatus.AuthorizedFunds);
            if (pendingforpay == null) return null;
            return pendingforpay;
        }

        /// <summary>
        /// Obtiene el ultimo tiempo contado
        /// </summary>
        /// <param name="idsolicitudservicio"></param>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public static async Task<API.ER.Time> GetLastTime(int idsolicitudservicio)
        {
            var chargeid = await GetCurrentChargeId(idsolicitudservicio);
            if (string.IsNullOrEmpty(chargeid)) return null;

            var times = (await Time.Query(new API.ER.Time
            {
                idsolicitudservicio = idsolicitudservicio,
                chargeid = chargeid
            }) ?? new List<API.ER.Time>());

            if (times.Count() == 0) return null;
            var time = new API.ER.Time
            {
                id = times.OrderByDescending(t => t.id).First().id,
                fechainicio = times.OrderBy(t => t.id).First().fechainicio,
                fechafin = times.OrderByDescending(t => t.id).First().fechainicio,
                costo = times.Sum(t => t.costo),
                finalizado = times.OrderByDescending(t => t.id).First().finalizado,
                idsolicitudservicio = idsolicitudservicio,
                tiempo = TimeSpan.FromSeconds(times.Sum(t => string.IsNullOrEmpty(t.tiempo) ? TimeSpan.FromSeconds(0).TotalSeconds : TimeSpan.Parse(t.tiempo).TotalSeconds)).ToString(),
                trabajador = times.First().trabajador
            };
            return time;
        }

        /// <summary>
        /// Devuelve la fecha
        /// </summary>
        /// <returns></returns>
        public static Task<DateTime?> GetDate()
        {
            return Task.Run<DateTime?>(() => DateTime.Now);
        }

        /// <summary>
        /// Permite saber si el servicio es valido
        /// </summary>
        /// <param name="currentservice"></param>
        /// <returns></returns>
        public static bool IsAvailableService(Requestservice currentservice)
        {
            return currentservice.idestadoservicio == (int)EstadoServicio.Aceptado
                || (currentservice.idestadoservicio > (int)EstadoServicio.Cancelado && currentservice.idestadoservicio < (int)EstadoServicio.TrabajoTerminado);
        }


        public static async Task<GoogleApi.Entities.Maps.DistanceMatrix.Response.Element> GoogleApiMatrixRequest(string key, double fromlat, double fromlng, double tolat, double tolng)
        {
            try
            {
                var algo = await GoogleApi.GoogleMaps.DistanceMatrix.QueryAsync(new GoogleApi.Entities.Maps.DistanceMatrix.Request.DistanceMatrixRequest
                {
                    Origins = new List<GoogleApi.Entities.Common.Location>
                {
                    new GoogleApi.Entities.Common.Location(fromlat, fromlng)
                },
                    Destinations = new List<GoogleApi.Entities.Common.Location>
                {
                    new GoogleApi.Entities.Common.Location(tolat, tolng)
                },
                    Language = GoogleApi.Entities.Common.Enums.Language.English,
                    Units = GoogleApi.Entities.Maps.Common.Enums.Units.Imperial,
                    TrafficModel = GoogleApi.Entities.Maps.Common.Enums.TrafficModel.Optimistic,
                    TravelMode = GoogleApi.Entities.Maps.Common.Enums.TravelMode.Bicycling,
                    Key = key
                });
                if (algo == null) return null;
                if (algo.Status != GoogleApi.Entities.Common.Enums.Status.Ok) return null;
                if (algo.Rows == null || algo.Rows.Count() == 0) return null;
                var row = algo.Rows.FirstOrDefault();
                if (row?.Elements == null || row.Elements.Count() == 0) return null;
                var element = row.Elements.FirstOrDefault();
                if (element == null || element.Status != GoogleApi.Entities.Common.Enums.Status.Ok) return null;
                return element;
            }
            catch { }
            return null;
        }

        public static async Task<IList<Position>> GoogleMapsApiRoute(string key, double fromlat, double fromlng, double tolat, double tolng)
        {
            try
            {
                var algo = await GoogleApi.GoogleMaps.Directions.QueryAsync(new GoogleApi.Entities.Maps.Directions.Request.DirectionsRequest
                {
                    Origin = new GoogleApi.Entities.Common.Location(fromlat, fromlng),
                    Destination = new GoogleApi.Entities.Common.Location(tolat, tolng),
                    Language = GoogleApi.Entities.Common.Enums.Language.English,
                    Units = GoogleApi.Entities.Maps.Common.Enums.Units.Imperial,
                    TrafficModel = GoogleApi.Entities.Maps.Common.Enums.TrafficModel.Pessimistic,
                    TravelMode = GoogleApi.Entities.Maps.Common.Enums.TravelMode.Driving,
                    Key = key
                });
                if (algo == null) return null;
                if (algo.Status != GoogleApi.Entities.Common.Enums.Status.Ok) return null;
                if (algo.Routes == null || algo.Routes.Count() == 0) return null;
                var positions = (Enumerable.ToList(PolylineHelper.Decode(algo.Routes.First().OverviewPath.Points)));
                return positions;
            }
            catch { }
            return new List<Position>(0);
        }


        /// <summary>
        /// Cambia la contraseña del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static async Task<bool> UpdatePassword(int idusuario, string password)
        {
            var status = await Restclient.ExecuteTaskAsync<string>($"api/changepassword/{idusuario}", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("password", password, RestSharp.ParameterType.GetOrPost)
            });
            if (!status.HasExecute) return false;
            return status.Result == "1";
        }

        /// <summary>
        /// Devuelve un servicio en proceso
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Requestservice> GetCurrentService(Perfil perfil, int idusuario)
        {
            var date = await GetDate();
            if (date == null) return null;
            var now = date.Value;
            Console.WriteLine($"[GetCurrentService] {perfil} | {idusuario}");
            Expression<Func<Requestservice, object>> usuario = perfil == Perfil.Client ? (s => s.cliente) : (Expression<Func<Requestservice, object>>)(s => s.trabajador);
            var servicesrequest = await Requestservice.Where(usuario, s => s.idestadoservicio).In(new List<object>(1)
            {
                idusuario
            }, new List<object>()
            {
                (int) EstadoServicio.Aceptado, (int) EstadoServicio.HerramientasCompradas, (int) EstadoServicio.EnCaminoADomicilio, (int) EstadoServicio.LlegadaADomicilio,
                (int) EstadoServicio.TrabajoIniciado
            }).Execute();

            if (!servicesrequest.HasExecute) return null;
            var services = (servicesrequest.Result ?? new List<Requestservice>(0)).Where(s => s.eliminado == 0);
            return services.Where(s => IsAvailableService(s) && now > s.fechadeservicio.FromMySqlDateTimeFormat())
                .OrderBy(r => r.fechadeservicio.FromMySqlDateTimeFormat())
                .FirstOrDefault();
        }

        public static async Task<Requestservice> GetReviewService(Perfil perfil, int idusuario)
        {
            var servicesrequest = await Requestservice.Where(new Requestservice
            {
                idestadoservicio = (int)EstadoServicio.TrabajoTerminado,
                trabajador = perfil == Perfil.Tasker ? idusuario : 0,
                cliente = perfil == Perfil.Client ? idusuario : 0
            });
            return servicesrequest.FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el balance del usuario
        /// </summary>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public static async Task<double> GetBalance(int idusuario)
        {
            var response = await Restclient.ExecuteTaskAsync<string>("api/getbalance", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idusuario", idusuario, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return 0;
            double.TryParse(response.Content, out double balance);
            return balance;
        }

        /// <summary>
        /// Get User Language
        /// </summary>
        /// <param name="lenguaje"></param>
        /// <returns></returns>
        public static async Task<int> GetLanguage(string lenguaje)
        {
            var response = await Restclient.ExecuteTaskAsync<Language>("v1/language", RestSharp.Method.GET, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("codigo", lenguaje, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return 1;
            return response?.Result?.id ?? 1;
        }

        /// <summary>
        /// Devuelve la calificacion de un usuario
        /// </summary>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public static async Task<double> GetReview(int idusuario)
        {
            var response = await Restclient.ExecuteTaskAsync<string>("api/getrating", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idusuario", idusuario, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return 0;
            double.TryParse(response.Content, out double balance);
            return balance;
        }

        /// <summary>
        /// Devuelve los servicios realizados
        /// </summary>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public static async Task<long> GetCompletedServices(int idusuario, int idperfil)
        {
            var response = await Restclient.ExecuteTaskAsync<string>("api/getcompletedservices", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idusuario", idusuario, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("idperfil", idperfil, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return 0;
            long.TryParse(response.Content, out long completedservices);
            return completedservices;
        }

        /// <summary>
        /// Obtiene todos los servicios del sistema
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<API.Response.Service>> GetServices(string lang)
        {
            var response = await Restclient.ExecuteTaskAsync<IEnumerable<API.Response.Service>>($"api/getservices?lang={lang}", RestSharp.Method.GET);
            if (!response.HasExecute) return null;
            return response.Result;
        }

        /// <summary>
        /// Delega el servicio solicitado a traves de un usuario [tasker]
        /// </summary>
        /// <param name="idsolicitudservicio"></param>
        /// <param name="idusuario"></param>
        /// <returns></returns>
        public static async Task<int> DelegateWork(int idsolicitudservicio, int trabajador)
        {
            var response = await Restclient.ExecuteTaskAsync<string>("api/delegateservice", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idsolicitudservicio", idsolicitudservicio, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("trabajador", trabajador, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return 0;
            int.TryParse(response.Result, out int intresponse);
            return intresponse;
        }

        /// <summary>
        /// Envia una notificacion a un usuario en especifico
        /// </summary>
        /// <param name="idusuario"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<API.Response.SendNotification> SendNotification(int idusuario, string title, string message, int data, int action)
        {
            var response = await Restclient.ExecuteTaskAsync< API.Response.SendNotification >("api/sendnotification", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idusuario", idusuario, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("title", title, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("message", message, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("data", data, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("action", action, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return null;
            return response.Result;
        }

        
        public static async Task<IEnumerable<string>> GetAddressByPoint(string key, double latitud, double longitud)
        {
            try
            {
                var algo = await GoogleApi.GoogleMaps.LocationGeocode.QueryAsync(new GoogleApi.Entities.Maps.Geocoding.Location.Request.LocationGeocodeRequest
                {
                    Location = new GoogleApi.Entities.Common.Location(latitud, longitud),
                    Language = GoogleApi.Entities.Common.Enums.Language.English,
                    Key = key
                });
                if (algo == null) return null;
                if (algo.Status != GoogleApi.Entities.Common.Enums.Status.Ok) return null;
                if (algo.Results == null) return null;
                return algo.Results.Select(s => s.FormattedAddress);
            }
            catch { }
            return new List<string>(0);
        }
        
        /// <summary>
        /// Realiza un switch para decir si el usuario esta online/offline
        /// </summary>
        /// <param name="idusuario"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static async Task<bool> SwitchSchedule(int idusuario, bool status)
        {
            var response = await Restclient.ExecuteTaskAsync<string>("api/switchschedule", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idusuario", idusuario, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("estado", status ? 1 : 0, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return false;
            int.TryParse(response.Result, out int result);
            return result == 1;
        }

        /// <summary>
        /// Realiza la accion de logout
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="id"></param>
        public static async Task<bool> LogOut(string sessionid, int idusuario)
        {
            var response = await Restclient.ExecuteTaskAsync<string>("api/logout", RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("idusuario", idusuario, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("idsesion", sessionid, RestSharp.ParameterType.GetOrPost)
            });
            return !response.HasExecute ? false : response.Result == "Success";
        }

        public static async Task<IEnumerable<API.Response.Tasker>> ListTaskersByAvailability(double latitud, double longitud, int idcategoria, int idsubcategoria,
            DateTime fecha, TimeSpan time)
        {
            var response = await Restclient.ExecuteTaskAsync<IEnumerable< API.Response.Tasker >>("api/listtaskers", RestSharp.Method.GET, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("latitud", latitud, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("longitud", longitud, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("idcategoria", idcategoria, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("idsubcategoria", idsubcategoria, RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("fecha", fecha.ToMySqlDateTimeFormat(), RestSharp.ParameterType.GetOrPost),
                new RestSharp.Parameter("tiempo", ((int) time.TotalHours), RestSharp.ParameterType.GetOrPost)
            });
            return response.HasExecute ? (response.Result ?? new List<API.Response.Tasker>()) : new List<API.Response.Tasker>();
        }

        /// <summary>
        /// Reward translate
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RewardTranslate>> GetRewardsTranslate(string lang)
        {
            var path = "api/getrewardstranslate";
            var response = await Restclient.ExecuteTaskAsync<IEnumerable<RewardTranslate>>(path, RestSharp.Method.POST, new List<RestSharp.Parameter>
            {
                new RestSharp.Parameter("lang", lang, RestSharp.ParameterType.GetOrPost)
            });
            if (!response.HasExecute) return new List<RewardTranslate>();
            return response.Result ?? new List<RewardTranslate>();
        }
    }
}
