using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GABCognitiveServices.Models
{
    public class Region
    {
        public string boundingBox { get; set; }
        public List<Line> lines { get; set; }
    }
}
