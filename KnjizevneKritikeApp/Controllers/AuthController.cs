using KnjizevneKritikeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using KnjizevneKritikeApp.Services;
using System;

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
            if (!ModelState.IsValid)
                return View(korisnik);

            // Provera da li korisničko ime ili email već postoje
            var postojiKorisnickoIme = _db.Korisnici.Find(x => x.KorisnickoIme == korisnik.KorisnickoIme).Any();
            if (postojiKorisnickoIme)
            {
                ModelState.AddModelError("KorisnickoIme", "Korisničko ime već postoji.");
                return View(korisnik);
            }

            var postojiEmail = _db.Korisnici.Find(x => x.Email == korisnik.Email).Any();
            if (postojiEmail)
            {
                ModelState.AddModelError("Email", "Email već postoji.");
                return View(korisnik);
            }

            korisnik.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
            korisnik.Lozinka = null;
            korisnik.PotvrdaLozinke = null;
            korisnik.DatumRegistracije = DateTime.Now;

            _db.Korisnici.InsertOne(korisnik);

            TempData["Success"] = "Registracija uspešna! Prijavite se.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Success = TempData["Success"];
            return View();
        }

        // FIKSNI LOGIN: uvek dozvoljava prijavu
        [HttpPost]
        public IActionResult Login(string korisnickoIme, string lozinka)
        {
            if (string.IsNullOrEmpty(korisnickoIme))
                korisnickoIme = "testkorisnik";

            HttpContext.Session.SetString("User", korisnickoIme);
            HttpContext.Session.SetString("korisnikId", "fiksni-id");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}