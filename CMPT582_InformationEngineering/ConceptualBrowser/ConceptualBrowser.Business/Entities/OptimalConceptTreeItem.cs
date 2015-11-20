using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class OptimalConceptTreeItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public OptimalConcept OptimalConcept { get; set; }
    }
}
