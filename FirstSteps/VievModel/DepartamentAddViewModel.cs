using FirstSteps.Commands;
using FirstSteps.Model;
using FirstSteps.Services;
using FirstSteps.Services.Implementations;
using FirstSteps.VievModel.Base;
using FirstSteps.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstSteps.VievModel
{
    class DepartamentAddViewModel : BaseViewModel, IDisposable
    {
        private readonly IUserDialog _userDialog;
        private readonly IMessageBus _messageBus;
        private readonly IDisposable _subscription;
        
        private ObservableCollection<Departament> _departaments;
        public ObservableCollection<Departament> Departaments
        {
            get => _departaments;
            set => Set(ref _departaments, value);
        }

        public ICommand AcceptCommand { get; set; }
        private bool OnCanAccept(object p) => true;
        private void OnAccepting(object a)
        {
            _messageBus.Send(new Message(new Departament { Title = DepartamentName }));
            _userDialog.CloseSecondaryWindow();
        }

        public void Dispose() => _subscription?.Dispose();             // clean up resources 

        private string _departamentName;

        public string DepartamentName
        {
            get { return _departamentName; }
            set { Set(ref _departamentName, value); }
        }

        public DepartamentAddViewModel()
        {
            AcceptCommand = new RelayCommand(OnAccepting, OnCanAccept);
        }

        public DepartamentAddViewModel(IUserDialog userDialog, IMessageBus messageBus) : this() 
        {
            _subscription = messageBus.RegisterHandler<DepartamentCollectionMessage>(OnReceive);
            _messageBus = messageBus;
            _userDialog = userDialog;
            //var departaments = Enumerable.Range(1, 60).Select(i => new Departament { Title = $"Fakultet {i}"});
            //Departaments = new ObservableCollection<Departament>(departaments);
        }

        public void OnReceive(DepartamentCollectionMessage message)
        {
            Departaments = new ObservableCollection<Departament>(message.Departaments);
        }
    }
}
