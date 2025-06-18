using KnjizevneKritikeApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnjizevneKritikeApp.Services
{
    public class KorisnikService
    {
        private readonly IMongoCollection<Korisnik> _korisnici;

        public KorisnikService(MongoDbService mongoDbService)
        {
            _korisnici = mongoDbService.Korisnici;
        }

        // CRUD
        public async Task<List<Korisnik>> GetAllAsync()
        {
            return await _korisnici.Find(_ => true).ToListAsync();
        }

        public async Task<Korisnik> GetByIdAsync(string id)
        {
            return await _korisnici.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Korisnik korisnik)
        {
            await _korisnici.InsertOneAsync(korisnik);
        }

        public async Task UpdateAsync(string id, Korisnik korisnik)
        {
            await _korisnici.ReplaceOneAsync(x => x.Id == id, korisnik);
        }

        public async Task DeleteAsync(string id)
        {
            await _korisnici.DeleteOneAsync(x => x.Id == id);
        }

        // Login
        public async Task<Korisnik> PrijaviSeAsync(string korisnickoIme, string lozinka)
        {
            var korisnik = await _korisnici.Find(x => x.KorisnickoIme == korisnickoIme).FirstOrDefaultAsync();
            if (korisnik == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(lozinka, korisnik.LozinkaHash)) return null;
            return korisnik;
        }

        // Provere za registraciju
        public async Task<bool> KorisnickoImePostojiAsync(string korisnickoIme)
        {
            return await _korisnici.Find(x => x.KorisnickoIme == korisnickoIme).AnyAsync();
        }

        public async Task<bool> EmailPostojiAsync(string email)
        {
            return await _korisnici.Find(x => x.Email == email).AnyAsync();
        }
    }
}
