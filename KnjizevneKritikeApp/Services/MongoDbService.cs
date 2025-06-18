using MongoDB.Driver;
using KnjizevneKritikeApp.Models;
using System.Threading.Tasks;

public class MongoDbService
{
    private readonly IMongoCollection<Korisnik> _korisnici;

    public MongoDbService(IMongoClient client)
    {
        var database = client.GetDatabase("knjizevne_kritike_db");
        _korisnici = database.GetCollection<Korisnik>("korisnici");
    }

    public async Task<Korisnik> NadjiKorisnikaAsync(string korisnickoIme)
    {
        return await _korisnici.Find(k => k.KorisnickoIme == korisnickoIme).FirstOrDefaultAsync();
    }

    public async Task<Korisnik> NadjiKorisnikaPoImenuIliEmailAsync(string korisnickoIme, string email)
    {
        return await _korisnici.Find(k => k.KorisnickoIme == korisnickoIme || k.Email == email).FirstOrDefaultAsync();
    }

    public async Task DodajKorisnikaAsync(Korisnik korisnik)
    {
        await _korisnici.InsertOneAsync(korisnik);
    }
}
