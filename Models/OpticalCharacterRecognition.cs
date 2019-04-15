using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GABCognitiveServices.Models
{
    public class OpticalCharacterRecognition
    {
        public double textAngle { get; set; }
        public string language { get; set; }
        public string orientation { get; set; }
        public List<Region> regions { get; set; }
    }
}
