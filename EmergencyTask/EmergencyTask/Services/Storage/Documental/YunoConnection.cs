using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Storage.Documental
{
    public class YunoConnection
    {
        private static YunoConnection _instance = new YunoConnection();
        public static YunoConnection Instance
        {
            get
            {
                _instance.Storage.AddConstraintExists += Storage_AddConstraintExists;
                return _instance;
            }
        }

        private static void Storage_AddConstraintExists(object sender, KeyValuePair<string, object> e)
        {
            Debugger.WriteLine($"[{e.Key},{e.Value}]");
        }

        public DocumentQuery<T> Query<T>()
        {
            var name = "Yuno_" + typeof(T).Name;
            return DocumentQuery<T>.From(name);
        }

        internal Settings Storage { get; set; }

        private YunoConnection()
        {
            Storage = Settings.Data = new Settings();
        }
        
        public List<string> ShowTables()
        {
            List<string> tables = new List<string>();
            foreach (var item in Storage)
            {
                if (item.Key.Contains("Yuno_"))
                {
                    tables.Add(item.Key.Replace("Yuno_", "Tabla: "));
                }
            }
            return tables;
        }

    }

    public class DocumentQuery<T> : List<T>
    {

        private string _tablename { get; set; }

        public static DocumentQuery<T> From(string key)
        {
            var dbmanager = YunoConnection.Instance;
            var storage = dbmanager.Storage;
            DocumentQuery<T> document = null;
            
            if (storage.ContainsKey(key))
            {
                var data = storage[key].ToString();
                Debugger.WriteLine("From: " + data);
                document = Newtonsoft.Json.JsonConvert.DeserializeObject<DocumentQuery<T>>(data);
            }
            else
            {
                document = new DocumentQuery<T>();
                storage.Add(key, Newtonsoft.Json.JsonConvert.SerializeObject(document));
            }
            document._tablename = key;
            return document;
        }
        
        public async Task SaveChanges()
        {
            var dbmanager = YunoConnection.Instance;
            var storage = dbmanager.Storage;
            storage.Replace = true;
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            if (storage.ContainsKey(this._tablename))
            {
                Debugger.WriteLine("Antes: []");
                Debugger.WriteLine("Despues: " + data);
                var antes = storage[this._tablename].ToString();
                storage[_tablename] = data;
            }
            else
            {
                Debugger.WriteLine("Antes: []");
                Debugger.WriteLine("Despues: " + data);
                storage.Add(_tablename, data);
            }
            await storage.SaveAsync();
            storage.Replace = false;
        }
    }
}
