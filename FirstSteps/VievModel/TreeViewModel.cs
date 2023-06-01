using FirstSteps.Commands;
using FirstSteps.Model;
using FirstSteps.Services;
using FirstSteps.VievModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstSteps.VievModel
{
    class TreeViewModel : BaseViewModel
    {
        public ICommand TreeViewItemSelectionChangedCommand { get; private set; }

        private bool CanSelect(object arg) => true;

        private void OnTreeViewItemSelectionChanged(object selectedItem)
        {
            // проверка на нужный мне тип и присвоение соответствующему свойству.
            if (selectedItem is Student student)
            {
                SelectedStudent = student;
            }
            else if (selectedItem is Group group)
            {
                SelectedGroup = group;
            }
        }


        #region --TREE VIEW ITEMS--
        private ObservableCollection<Student> students;

        public ObservableCollection<Student> Students
        {
            get { return students; }
            set { students = value; }
        }

        private ObservableCollection<Group> groups;

        public ObservableCollection<Group> Groups
        {
            get { return groups; }
            set { groups = value; }
        }

        private ObservableCollection<Departament> _departaments;

        public ObservableCollection<Departament> Departaments
        {
            get { return _departaments; }
            set { _departaments = value; }
        }
        #endregion


        #region --PROPERTIES--
        private Group _selectedGroup;

        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set { Set(ref _selectedGroup, value); }
        }

        private Student _selectedStudent;

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => Set(ref _selectedStudent, value);
        }
        #endregion

        public TreeViewModel()
        {
            TreeViewItemSelectionChangedCommand = new RelayCommand(OnTreeViewItemSelectionChanged, CanSelect);
        }
    }
}
