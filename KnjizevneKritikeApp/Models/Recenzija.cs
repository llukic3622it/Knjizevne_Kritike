using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace KnjizevneKritikeApp.Models
{
    public class Recenzija
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string NaslovKnjige { get; set; }
        public string AutorKnjige { get; set; }
        public int Ocena { get; set; } // 1-5
        public string TekstRecenzije { get; set; }
        public string KorisnikId { get; set; }
        public string KorisnickoIme { get; set; }
        public DateTime DatumObjave { get; set; }
        public List<Komentar> Komentari { get; set; } = new List<Komentar>();
    }
}
