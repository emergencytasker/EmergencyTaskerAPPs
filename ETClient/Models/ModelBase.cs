﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ETClient.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual bool Validation()
        {
            return true;
        }
    }
}
