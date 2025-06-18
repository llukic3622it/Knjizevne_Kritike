using KnjizevneKritikeApp.Models;
using KnjizevneKritikeApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KnjizevneKritikeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly KorisnikService _service;

        public AccountController(KorisnikService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Korisnik korisnik)
        {
            if (!ModelState.IsValid)
                return View(korisnik);

            if (await _service.KorisnickoImePostojiAsync(korisnik.KorisnickoIme))
            {
                ModelState.AddModelError("KorisnickoIme", "Korisničko ime već postoji.");
                return View(korisnik);
            }

            if (await _service.EmailPostojiAsync(korisnik.Email))
            {
                ModelState.AddModelError("Email", "Email već postoji.");
                return View(korisnik);
            }

            korisnik.DatumRegistracije = DateTime.Now;
            korisnik.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
            korisnik.Lozinka = null;
            korisnik.PotvrdaLozinke = null;

            await _service.CreateAsync(korisnik);

            TempData["Success"] = "Registracija uspešna! Prijavite se.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Success = TempData["Success"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string korisnickoIme, string lozinka)
        {
            if (string.IsNullOrEmpty(korisnickoIme) || string.IsNullOrEmpty(lozinka))
            {
                ModelState.AddModelError("", "Unesite korisničko ime i lozinku.");
                return View();
            }

            var korisnik = await _service.PrijaviSeAsync(korisnickoIme, lozinka);
            if (korisnik == null)
            {
                ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka.");
                return View();
            }

            HttpContext.Session.SetString("korisnikId", korisnik.Id);
            HttpContext.Session.SetString("korisnickoIme", korisnik.KorisnickoIme);

            return RedirectToAction("Index", "Recenzije");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
