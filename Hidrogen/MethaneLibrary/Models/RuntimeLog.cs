using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MethaneLibrary.Models {
    
    public class RuntimeLog {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Controller { get; set; }
        
        public string Action { get; set; }
        
        public string Data { get; set; }
        
        public string Briefing { get; set; }
        
        public string Details { get; set; }
        
        public string Severity { get; set; }
        
        [BsonDateTimeOptions]
        public DateTime? RecordedOn { get; set; }
    }
}