using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GABCognitiveServices.Models
{
    public class ImageInfoViewModel
    {
        public List<Category> categories { get; set; }
        public Description description { get; set; }
        public Color color { get; set; }
        public string requestId { get; set; }
        public Metadata metadata { get; set; }
        public string imageFileString { get; set; }
        public string contentType { get; set; }
    }
}
