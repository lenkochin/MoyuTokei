using Prism.Mvvm;
using System;

namespace MoyuTokei.ViewModels.ItemLevel
{
    internal class DesktopAppInfoViewModel(string appTitle, IntPtr appHandle) : BindableBase
    {
        private bool _isSelected;

        public string AppTitle => appTitle;

        public IntPtr AppHandle => appHandle;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}