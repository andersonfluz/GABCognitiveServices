using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GABCognitiveServices.Models
{
    public class Line
    {
        public string boundingBox { get; set; }
        public List<Word> words { get; set; }
    }
}
