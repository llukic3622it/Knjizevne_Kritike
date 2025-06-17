using Microsoft.AspNetCore.Mvc;
using KnjizevneKritikeApp.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace KnjizevneKritikeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MongoDbService _db;

        public AccountController(MongoDbService db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(Korisnik korisnik, string lozinka)
        {
            if (!ModelState.IsValid)
            {
                return View(korisnik);
            }

            korisnik.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(lozinka);
            korisnik.DatumRegistracije = DateTime.Now;

            await _db.DodajKorisnikaAsync(korisnik);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string korisnickoIme, string lozinka)
        {
            var korisnik = await _db.NadjiKorisnikaAsync(korisnickoIme);
            if (korisnik == null || !BCrypt.Net.BCrypt.Verify(lozinka, korisnik.LozinkaHash))
            {
                ModelState.AddModelError(string.Empty, "Neispravno korisničko ime ili lozinka");
                return View("Login");
            }

            HttpContext.Session.SetString("korisnikId", korisnik.Id);
            HttpContext.Session.SetString("korisnickoIme", korisnik.KorisnickoIme);

            return RedirectToAction("Index", "Recenzije");
        }

        [HttpPost]  // Logout treba da bude POST za sigurnost
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

