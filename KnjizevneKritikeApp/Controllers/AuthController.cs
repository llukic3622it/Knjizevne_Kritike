using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using KnjizevneKritikeApp.Models;
using MongoDB.Driver;
using BCrypt.Net;

namespace KnjizevneKritikeApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly MongoDbService _db;

        public AuthController(MongoDbService db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                // Hashuj lozinku
                korisnik.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
                korisnik.DatumRegistracije = DateTime.Now;

                // Ubaci korisnika u kolekciju
                _db.Korisnici.InsertOne(korisnik);

                return RedirectToAction("Login");
            }

            return View(korisnik);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string korisnickoIme, string lozinka)
        {
            var filter = Builders<Korisnik>.Filter.Eq(k => k.KorisnickoIme, korisnickoIme);
            var korisnik = _db.Korisnici.Find(filter).FirstOrDefault();

            if (korisnik != null && BCrypt.Net.BCrypt.Verify(lozinka, korisnik.Lozinka))
            {
                HttpContext.Session.SetString("User", korisnik.KorisnickoIme);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Pogrešno korisničko ime ili lozinka.");
            ViewData["KorisnickoIme"] = korisnickoIme; // da korisnik ne mora opet da unosi
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
