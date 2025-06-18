using MongoDB.Driver;
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
        var client = new MongoClient(config["MongoDb:ConnectionString"]);
        var database = client.GetDatabase(config["MongoDb:Database"]);

        _korisnici = database.GetCollection<Korisnik>("Korisnici");
        _recenzije = database.GetCollection<Recenzija>("Recenzije");
    }

    public IMongoCollection<Korisnik> Korisnici => _korisnici;
    public IMongoCollection<Recenzija> Recenzije => _recenzije;

    public async Task DodajKorisnikaAsync(Korisnik korisnik)
        => await _korisnici.InsertOneAsync(korisnik);

    public async Task<Korisnik> NadjiKorisnikaAsync(string korisnickoIme)
        => await _korisnici.Find(x => x.KorisnickoIme == korisnickoIme).FirstOrDefaultAsync();

    public async Task DodajRecenzijuAsync(Recenzija rec)
        => await _recenzije.InsertOneAsync(rec);

    public async Task<List<Recenzija>> VratiSveRecenzijeAsync()
        => await _recenzije.Find(_ => true).SortByDescending(r => r.DatumObjave).ToListAsync();

    public async Task DodajKomentarAsync(string recenzijaId, Komentar komentar)
    {
        var filter = Builders<Recenzija>.Filter.Eq(r => r.Id, recenzijaId);
        var update = Builders<Recenzija>.Update.Push(r => r.Komentari, komentar);
        await _recenzije.UpdateOneAsync(filter, update);
    }
}
