using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GABCognitiveServices.Models
{
    public class Description
    {
        public List<string> tags { get; set; }
        public List<Caption> captions { get; set; }
    }
}
