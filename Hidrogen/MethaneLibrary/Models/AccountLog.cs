using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MethaneLibrary.Models {
    
    public class AccountLog {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        //Can only be one of Account, Profile, Preference
        public string UpdateFor { get; set; }
        
        public List<ContentChangeLog> Changes { get; set; }

        [BsonDateTimeOptions]
        public DateTime? UpdatedOn { get; set; }
        
        [BsonDateTimeOptions]
        public DateTime? LastOnline { get; set; }
        
        public string LastBrowser { get; set; }
        
        public string LastDevice { get; set; }
        
        public string LastOs { get; set; }
        
        public string LastLocation { get; set; }
        
        public string LastIpAddress { get; set; }
        
        [BsonDateTimeOptions]
        public DateTime? LastOffline { get; set; }
        
        [BsonDateTimeOptions]
        public DateTime? RecordedOn { get; set; }
    }

    public class ContentChangeLog {
        
        public string Field { get; set; }
        
        public string OldContent { get; set; }
        
        public string NewContent { get; set; }
        
        //Indicate Delete, Update, Remove, Add, Replace
        public string Type { get; set; }
    }
}