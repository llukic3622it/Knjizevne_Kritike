using KnjizevneKritikeApp.Models;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;

namespace KnjizevneKritikeApp.Services
{
    public class KorisnikService
    {
        private readonly IMongoCollection<Korisnik> _korisnici;

        public KorisnikService(MongoDbService mongoDbService)
        {
            // Ne koristi GetDatabase(), već pristupi kolekciji preko javnog svojstva
            _korisnici = mongoDbService.Korisnici;
        }

        public async Task<bool> KorisnickoImePostojiAsync(string korisnickoIme)
        {
            var korisnik = await _korisnici.Find(k => k.KorisnickoIme == korisnickoIme).FirstOrDefaultAsync();
            return korisnik != null;
        }

        public async Task<bool> EmailPostojiAsync(string email)
        {
            var korisnik = await _korisnici.Find(k => k.Email == email).FirstOrDefaultAsync();
            return korisnik != null;
        }

        public async Task RegistrujAsync(Korisnik korisnik)
        {
            korisnik.LozinkaHash = HashLozinka(korisnik.Lozinka);
            korisnik.DatumRegistracije = DateTime.UtcNow;
            await _korisnici.InsertOneAsync(korisnik);
        }

        public async Task<Korisnik> PrijaviSeAsync(string korisnickoIme, string lozinka)
        {
            var korisnik = await _korisnici.Find(k => k.KorisnickoIme == korisnickoIme).FirstOrDefaultAsync();
            if (korisnik == null) return null;

            if (korisnik.LozinkaHash == HashLozinka(lozinka))
                return korisnik;

            return null;
        }

        private string HashLozinka(string lozinka)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(lozinka);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
