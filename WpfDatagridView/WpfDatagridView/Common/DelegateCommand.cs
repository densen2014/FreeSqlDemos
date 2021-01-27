using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace System.Windows.InputB
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _handler;
        private bool _isEnabled = true;

        public DelegateCommand(Action handler)
        {
            _handler = handler;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _handler();
        }

    }


    public class DelegateCommand<T> : ICommand
    {
        public DelegateCommand() : this(null, null)
        { }
        public DelegateCommand(Action<T> executeMethod) : this(executeMethod, null)
        { }
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            TargetExecuteMethod = executeMethod;
            TargetCanExecuteMethod = canExecuteMethod;
        }
        public Action<T> TargetExecuteMethod
        { get; set; }
        public Func<T, bool> TargetCanExecuteMethod
        { get; set; }

        public void OnCanExecuteChanged()
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }

        public void Execute(T parameter)
        {
            if (TargetExecuteMethod != null)
                TargetExecuteMethod(parameter);
        }

        public bool CanExecute(T parameter)
        {
            if (TargetCanExecuteMethod != null)
                return TargetCanExecuteMethod(parameter);
            if (TargetExecuteMethod != null)
                return true;
            return false;
        }
        #region ICommand    
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }
        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }
        public event EventHandler CanExecuteChanged;
        #endregion
    }
}
