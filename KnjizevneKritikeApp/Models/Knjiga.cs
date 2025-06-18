using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace KnjizevneKritikeApp.Models
{
    public class Knjiga
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Naslov je obavezan.")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Autor je obavezan.")]
        public string Autor { get; set; }

        [Display(Name = "Datum dodavanja")]
        public DateTime DatumDodavanja { get; set; } = DateTime.Now;

        [BsonRepresentation(BsonType.ObjectId)]
        [Display(Name = "Dodao korisnik (Id)")]
        public string DodaoKorisnikId { get; set; }
    }
}
