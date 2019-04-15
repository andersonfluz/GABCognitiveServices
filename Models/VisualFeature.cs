using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GABCognitiveServices.Models
{
    public class VisualFeature
    {
        public IFormFile upload { get; set; }
        public bool Categories { get; set; }
        public bool Tags { get; set; }
        public bool Description { get; set; }
        public bool Face { get; set; }
        public bool ImageType { get; set; }
        public bool Color { get; set; }
        public bool Adult { get; set; }

        public String visualFeaturesTags(VisualFeature vf)
        {
            String tags = "";
            foreach (PropertyInfo propertyInfo in vf.GetType().GetProperties())
            {
                if ((bool)propertyInfo.GetValue(vf))
                {
                    tags = tags + propertyInfo.Name;
                }               
            }
            return tags;
        }
    }
}
