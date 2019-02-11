using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace product_catalog_service.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public IEnumerable<string> ImageLinks {get;set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}