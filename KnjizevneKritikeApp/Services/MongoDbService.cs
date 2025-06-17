using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using KnjizevneKritikeApp.Models;


public class MongoDbService
{
    private readonly IMongoCollection<Korisnik> _korisnici;
    private readonly IMongoCollection<Recenzija> _recenzije;

    public MongoDbService(IConfiguration config)
    {
        // Kreira MongoDB klijenta sa konekcijom iz appsettings.json
        var client = new MongoClient(config["MongoDb:ConnectionString"]);

        // Otvara bazu podataka iz appsettings.json
        var database = client.GetDatabase(config["MongoDb:Database"]);

        // Kolekcije u bazi
        _korisnici = database.GetCollection<Korisnik>("Korisnici");
        _recenzije = database.GetCollection<Recenzija>("Recenzije");
    }

    // Dodaje novog korisnika u kolekciju Korisnici
    public async Task DodajKorisnikaAsync(Korisnik korisnik)
        => await _korisnici.InsertOneAsync(korisnik);

    // Pronadje korisnika po korisnickom imenu ili vrati null
    public async Task<Korisnik> NadjiKorisnikaAsync(string korisnickoIme)
        => await _korisnici.Find(x => x.KorisnickoIme == korisnickoIme).FirstOrDefaultAsync();

    // Dodaje novu recenziju u kolekciju Recenzije
    public async Task DodajRecenzijuAsync(Recenzija rec)
        => await _recenzije.InsertOneAsync(rec);

    // Vrati sve recenzije sortirane po datumu objave opadajuce
    public async Task<List<Recenzija>> VratiSveRecenzijeAsync()
        => await _recenzije.Find(_ => true).SortByDescending(r => r.DatumObjave).ToListAsync();

    // Dodaje komentar u postojeću recenziju (push u listu komentara)
    public async Task DodajKomentarAsync(string recenzijaId, Komentar komentar)
    {
        var filter = Builders<Recenzija>.Filter.Eq(r => r.Id, ObjectId.Parse(recenzijaId));
        var update = Builders<Recenzija>.Update.Push(r => r.Komentari, komentar);
        await _recenzije.UpdateOneAsync(filter, update);
    }
}

