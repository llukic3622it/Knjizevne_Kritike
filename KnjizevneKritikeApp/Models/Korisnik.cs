using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace KnjizevneKritikeApp.Models
{
    public class Korisnik
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string LozinkaHash { get; set; }

        public DateTime DatumRegistracije { get; set; }

        [BsonIgnore]
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Lozinka mora imati najmanje 6 karaktera.")]
        public string Lozinka { get; set; }

        [BsonIgnore]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string PotvrdaLozinke { get; set; }
    }
}
