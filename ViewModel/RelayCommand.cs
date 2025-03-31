using System;
using System.Windows.Input;

namespace ViewModel
{
    internal class RelayCommand : ICommand
    {
        public RelayCommand(Action execute, Func<bool>? canExcute = null)
        {
            this.m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.m_CanExecute = canExcute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.m_CanExecute == null)
            {
                return true;
            }
            if (parameter == null)
            {
                return this.m_CanExecute();
            }
            return this.m_CanExecute();
        }

        public virtual void Execute(object parameter)
        {
            this.m_Execute();
        }

        public event EventHandler? CanExecuteChanged;

        internal void RaiseCanExcuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private readonly Action m_Execute;
        private readonly Func<bool>? m_CanExecute;
    }
}
