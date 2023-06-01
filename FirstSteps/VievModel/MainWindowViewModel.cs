using FirstSteps.Commands;
using FirstSteps.Model;
using FirstSteps.VievModel.Base;
using System.Windows.Input;
using FirstSteps.Services;
using System;
using FirstSteps.Services.Implementations;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FirstSteps.VievModel
{
    class MainWindowViewModel : BaseViewModel, IDisposable
    {
        public TreeViewModel TreeViewModel { get; set; }
        private readonly IUserDialog _userDialog;
        private readonly IMessageBus _messageBus;
        private readonly IDisposable _subscription;

        #region --COMMANDS--

        public ICommand AddDepartamentCommand { get; }

        private bool CanCreate(object arg) => true;

        private void OnCreate(object obj)
        {
            _messageBus.Send(new DepartamentCollectionMessage(TreeViewModel.Departaments));
           _userDialog.OpenSecondaryWindow();
        }
        #endregion

        public MainWindowViewModel(IUserDialog service, IMessageBus messageBus) : this()
        {
            _userDialog = service;
            _messageBus = messageBus;
            _subscription = _messageBus.RegisterHandler<Message>(OnReceiveMessage);
            TreeViewModel = new TreeViewModel();
            int index = 0;
            var students = new ObservableCollection<Student>(Enumerable.Range(1, 10).Select(i => new Student { Name = $"Name {i}", SecondName = $"SecondName {index++}", ThirdName = $"ThirdName {index++}" }));
            var groups = new ObservableCollection<Group>(Enumerable.Range(1, 60).Select(i => new Group { Title = $"Group {i}", Students = students }));
            var departaments = Enumerable.Range(1, 60).Select(i => new Departament { Title = $"Fakultet {i}", Groups = groups });
            TreeViewModel.Departaments = new ObservableCollection<Departament>(departaments);
        }

        private void OnReceiveMessage(Message message)
        {
            if (message is null || message.departament.Title == null) return;

            if (message.departament.Title.Length != 0)
                TreeViewModel.Departaments.Add(new Departament { Title = message.departament.Title });
        }

        public void Dispose() => _subscription?.Dispose();

        public MainWindowViewModel()
        {
            AddDepartamentCommand = new RelayCommand(OnCreate, CanCreate);
        }
    }
}

