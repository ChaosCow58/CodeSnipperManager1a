using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeSnipperManager1a.MVVM.Model
{
    public class Snippet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string CodeSnippet { get; set; }
    }
}
