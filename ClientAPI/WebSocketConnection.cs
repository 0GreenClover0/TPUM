using System;
using System.Threading.Tasks;

namespace ConnectionAPI
{
    public abstract class WebSocketConnection : IObservable<string>
    {
        // public virtual Action<string>? OnMessage { set; get; } = x => { };
        public virtual Action? OnClose { set; protected get; } = () => { };
        public virtual Action? OnError { set; protected get; } = () => { };

        private readonly List<IObserver<string>> _observers = new();

        public void OnNext(string value)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }

        public abstract Task DisconnectAsync();
        protected abstract Task SendTask(string message);

        public async Task SendAsync(string message)
        {
            await SendTask(message);
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            _observers.Add(observer);
            return null;
        }
    }
}
