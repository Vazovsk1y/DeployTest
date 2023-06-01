using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSteps.Model
{
    public class DepartamentCollectionMessage
    {
        public ObservableCollection<Departament> Departaments { get; set; }

        public DepartamentCollectionMessage(ObservableCollection<Departament> departaments)
        {
            Departaments = departaments;
        }
    }
}
