using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GABCognitiveServices.Models
{
    public class Color
    {
        public string dominantColorForeground { get; set; }
        public string dominantColorBackground { get; set; }
        public List<string> dominantColors { get; set; }
        public string accentColor { get; set; }
        public bool isBwImg { get; set; }
    }
}
