using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace KnjizevneKritikeApp.Models
{
    public class Komentar
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string KorisnikId { get; set; }
        public string KorisnickoIme { get; set; }
        public string Tekst { get; set; }
        public DateTime Datum { get; set; }
    }
}
