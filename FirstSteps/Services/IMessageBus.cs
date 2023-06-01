using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSteps.Services
{
    internal interface IMessageBus
    {
        IDisposable RegisterHandler<T>(Action<T> handler);
        void Send<T>(T message);
        Task SendAsync<T>(T message);
    }
}
