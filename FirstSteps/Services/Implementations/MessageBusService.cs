using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstSteps.Services.Implementations
{
    internal class MessageBusService : IMessageBus
    {
        private class Subscription<T> : IDisposable
        {
            private readonly WeakReference<MessageBusService> _bus;
            public Action<T> _handler { get; }

            public Subscription(MessageBusService bus, Action<T> handler)
            {
                _bus = new(bus);
                _handler = handler;
            }

            public void Dispose()
            {
                if (!_bus.TryGetTarget(out var bus))
                    return;

                var Lock = bus._lock;
                Lock.EnterWriteLock();
                var messageType = typeof(T);
                try
                {
                    if (!bus._subscriptions.TryGetValue(messageType, out var refs))
                        return;

                    var updatedRefs = refs.Where(r => r.IsAlive).ToList();

                    WeakReference? currentReference = null;
                    foreach (var item in updatedRefs)
                    {
                        if (ReferenceEquals(item.Target, this))
                        {
                            currentReference = item;
                            break;
                        }
                    }
                    if (currentReference == null)
                        return;

                    updatedRefs.Remove(currentReference);
                    bus._subscriptions[messageType] = updatedRefs;

                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        private readonly Dictionary<Type, List<object>> _unprocessedMessages = new();
        private readonly Dictionary<Type, IEnumerable<WeakReference>> _subscriptions = new();
        private readonly ReaderWriterLockSlim _lock = new();

        public IDisposable RegisterHandler<T>(Action<T> handler)
        {
            var subscription = new Subscription<T>(this, handler);

            _lock.EnterWriteLock();
            try
            {
                var weakRef = new WeakReference(subscription);
                var messageType = typeof(T);

                _subscriptions[messageType] = _subscriptions.TryGetValue(messageType, out var subscriptions) ?
                    subscriptions.Append(weakRef) : new[] { weakRef };
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return subscription;
        }

        private IEnumerable<Action<T>>? GetHandlers<T>()
        {
            var handlers = new List<Action<T>>();
            var messageType = typeof(T);
            bool isRefDied = false;

            _lock.EnterReadLock();
            try
            {
                if (!_subscriptions.TryGetValue(messageType, out var refs))
                    return null;

                foreach(var item in refs)
                {
                    if (item.Target is Subscription<T> { _handler: var handler })
                        handlers.Add(handler);
                    else
                        isRefDied = true;
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (!isRefDied) return handlers;

            _lock.EnterWriteLock();
            try
            {
                if (!_subscriptions.TryGetValue(messageType, out var refs))
                    if (refs.Where(r => r.IsAlive).ToArray() is { Length: > 0 } newRefs)
                        _subscriptions[messageType] = newRefs;
                    else
                        _subscriptions.Remove(messageType);
            }
            finally
            {
                _lock.ExitWriteLock();
            }

            return handlers;
        }

        public void Send<T>(T message)
        {
            if (GetHandlers<T>() is not { } handlers)
                return;
            //{
            //    // Если для данного типа сообщения нет подписчиков,
            //    // то добавляем непрочитанное сообщение в список.
            //    lock (_unprocessedMessages)
            //    {
            //        if (!_unprocessedMessages.ContainsKey(typeof(T)))
            //        {
            //            _unprocessedMessages[typeof(T)] = new List<object>();
            //        }
            //        _unprocessedMessages[typeof(T)].Add(message);
            //    }
            //    return;
            //}

            foreach (var item in handlers)
            {
                item(message);
            }

            
        }

        public Task SendAsync<T>(T message)
        {
            if (GetHandlers<T>() is not { } handlers)
                return Task.CompletedTask;

            foreach (var item in handlers)
            {
                item(message);
            }

            return Task.CompletedTask;
        }
    }
}
