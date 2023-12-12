using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Globalization;

namespace CodeSnipperManager1a.MVVM.Model
{

#pragma warning disable CS8618
    public class Snippet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string CodeSnippet { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-6);
    }
}
