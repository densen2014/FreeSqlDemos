using AME.Helpers;
using AME.Services;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace AME.Colors
{
    public class PaletteSelectorViewModel
    {
        public PaletteSelectorViewModel(AppSetting appSetting)
        {
            Swatches = new SwatchesProvider().Swatches;
            //appSetting = AppSetting.Create(FileName);
            this.appSetting = appSetting;
            ApplyBase(appSetting.IsDark);
        }

        public ICommand ToggleBaseCommand { get; } = new AnotherCommandImplementation(o => ApplyBase((bool)o));

        private static void ApplyBase(bool isDark)
        {
            ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
        }

        public IEnumerable<Swatch> Swatches { get; }

        public ICommand ApplyPrimaryCommand { get; } = new AnotherCommandImplementation(o => ApplyPrimary((Swatch)o));

        private static void ApplyPrimary(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));
        }

        public ICommand ApplyAccentCommand { get; } = new AnotherCommandImplementation(o => ApplyAccent((Swatch)o));

        private static void ApplyAccent(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }

        //private static string FileName = Process.GetCurrentProcess().MainModule.ModuleName.ToString() + ".jpg";

        public AppSetting appSetting { get; set; }

    }


    /// <summary>
    /// No WPF project is complete without it's own version of this.
    /// </summary>
    public class AnotherCommandImplementation : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public AnotherCommandImplementation(Action<object> execute) : this(execute, null)
        {
        }

        public AnotherCommandImplementation(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute ?? (x => true);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class PaletteHelper
    {
        public virtual ITheme GetTheme()
        {
            if (Application.Current == null)
                throw new InvalidOperationException("Cannot get theme outside of a WPF application. Use ResourceDictionaryExtensions.GetTheme on the appropriate resource dictionary instead.");
            return Application.Current.Resources.GetTheme();
        }

        public virtual void SetTheme(ITheme theme)
        {
            if (theme == null) throw new ArgumentNullException(nameof(theme));
            if (Application.Current == null)
                throw new InvalidOperationException("Cannot set theme outside of a WPF application. Use ResourceDictionaryExtensions.SetTheme on the appropriate resource dictionary instead.");
            Application.Current.Resources.SetTheme(theme);
        }
    }

}
