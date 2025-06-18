using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

public class Recenzija
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string KnjigaId { get; set; }

    [Required]
    public string KorisnikId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Ocena { get; set; }

    public string Komentar { get; set; }
    public DateTime Datum { get; set; }
}
