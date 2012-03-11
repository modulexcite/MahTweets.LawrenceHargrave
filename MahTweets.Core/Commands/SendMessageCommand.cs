using System;
using System.Windows;
using System.Windows.Input;

namespace MahTweets.Core.Commands
{
    public class SendMessageCommand : RoutedCommand
    {
        private readonly Action<object, IInputElement> _func;
        private readonly Action<object> _funcObj;

        public SendMessageCommand(Action<object> func)
        {
            _funcObj = func;
        }

        public SendMessageCommand(Action<object, IInputElement> func)
        {
            _func = func;
        }

        public bool CanExecute(object parameter)
        {
            return parameter.ToString().Length > 0;
        }

        public new void Execute(object parameter, IInputElement element)
        {
            if (_func != null)
                _func(parameter, element);
            else if (_funcObj != null)
                _funcObj(parameter);
        }
    }
}