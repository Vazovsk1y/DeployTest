using FirstSteps.VievModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FirstSteps.Model
{
    public class Departament 
    {
        public string Title { get; set; }
        public ICollection<Group> Groups { get; set; }
    }

}
