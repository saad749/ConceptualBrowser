using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class SimpleNode // PLEASE DELETEEEE!!! NOT REQUIREDDDD
    {
        public int Index { get; set; }
        public int Number { get; set; }

        public SimpleNode(int index, int number)
        {
            Index = index;
            Number = number;
        }
    }
}
