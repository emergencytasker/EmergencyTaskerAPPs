﻿using System;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.Language
{
    [ContentProperty("Key")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Key { get; set; }
        static ResourceManager ResourceManagerInstance;

        #region IMarkupExtension implementation

        public static void Init(ResourceManager r)
        {
            ResourceManagerInstance = r;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ResourceManagerInstance == null)
            {
                throw new InvalidOperationException("Call TranslateExtension.Init(ResourceManager) in your App.cs");
            }
            var value = Key;
            try
            {
                value = ResourceManagerInstance.GetString(Key);
            }
            catch { }
            return value;
        }

        #endregion
    }
}
