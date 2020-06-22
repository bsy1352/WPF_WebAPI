using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIClient
{

    public class RootObject
    {
        [JsonProperty("book")]
        public IEnumerable<Book> book { get; set; }
    }

    public class Book
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("author")]
        public string author { get; set; }

        [JsonProperty("isbn")]
        public string isbn { get; set; }

        
    }

    
}
