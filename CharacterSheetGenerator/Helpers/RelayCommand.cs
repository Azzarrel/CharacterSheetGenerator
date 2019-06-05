using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CharacterSheetGenerator.Helpers
{
  class RelayCommand : ICommand
  {
    readonly Func<bool> m_canExecute;
    readonly Action m_execute;

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      m_execute = execute ?? throw new ArgumentNullException("execute");
      m_canExecute = canExecute;

    }

    public RelayCommand(Action execute)
  : this(execute, null) { }

    public event EventHandler CanExecuteChanged;

    public void UpdateCanExecuteState()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool CanExecute(object parameter)
    {
      return m_canExecute == null ? true : m_canExecute();
    }

    public void Execute(object parameter)
    {
      m_execute();
    }
  }

    class RelayCommand<T> : ICommand
    {
        readonly Predicate<T> m_canExecute = null;
        readonly Action<T> m_execute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            m_execute = execute ?? throw new ArgumentNullException("execute");
            m_canExecute = canExecute;

        }

        public RelayCommand(Action<T> execute)
      : this(execute, null) { }

        public event EventHandler CanExecuteChanged;

        public void UpdateCanExecuteState()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


        public bool CanExecute(object parameter)
        {
            return m_canExecute == null ? true : m_canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            m_execute((T)parameter);
        }
    }
}
