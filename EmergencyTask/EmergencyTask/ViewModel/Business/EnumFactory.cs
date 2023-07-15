using System;
using System.Collections.Generic;

namespace EmergencyTask.ViewModel.Business
{
    public class EnumFactory<TEnum, KInterface>
    {
        private readonly Dictionary<TEnum, KInterface> _factories;

        public EnumFactory(string space, string subname)
        {
            _factories = new Dictionary<TEnum, KInterface>();

            foreach (TEnum action in Enum.GetValues(typeof(TEnum)))
            {
                try
                {
                    KInterface factory = default(KInterface);
                    factory = (KInterface)Activator.CreateInstance(Type.GetType(space + Enum.GetName(typeof(TEnum), action) + subname));
                    _factories.Add(action, factory);
                }
                catch(Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
            }
        }

        public KInterface Resolve(TEnum action)
        {
            if (_factories.ContainsKey(action))
            {
                return _factories[action];
            }
            else
            {
                return default(KInterface);
            }
        }
    }
}
