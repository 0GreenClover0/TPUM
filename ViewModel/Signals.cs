using System.Windows.Input;

namespace ViewModel
{
    internal class Signals : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Signals(Action action)
        {
            this.action = action;
        }

        private Action action;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.action();
        }
    }
}
