using FirstSteps.VievModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSteps.Model
{
    public class Group
    {
        public string Title { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
