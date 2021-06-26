// Generated using https://json2csharp.com/

using System;
using System.Collections.Generic;

namespace KafkaProducer.Models {
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class KFriend
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class KPerson
    {
        public string _id { get; set; }
        public int index { get; set; }
        public string guid { get; set; }
        public bool isActive { get; set; }
        public string balance { get; set; }
        public string picture { get; set; }
        public int age { get; set; }
        public string eyeColor { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string about { get; set; }
        public DateTime registered { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public List<string> tags { get; set; }
        public List<KFriend> KFriends { get; set; }
        public string greeting { get; set; }
        public string favoriteFruit { get; set; }
    }

    public class KMessage
    {
        public List<KPerson> KPeople { get; set; }
    }

}
