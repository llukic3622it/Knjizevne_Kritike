using KnjizevneKritikeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly MongoDbService _db;

    public AccountController(MongoDbService db)
    {
        _db = db;
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

        // Provera da li korisnicko ime ili email vec postoji
        var postojiKorisnik = await _db.NadjiKorisnikaPoImenuIliEmailAsync(korisnik.KorisnickoIme, korisnik.Email);
        if (postojiKorisnik != null)
        {
            ModelState.AddModelError("", "Korisničko ime ili email već postoji.");
            return View(korisnik);
        }

        // Hashiranje lozinke
        korisnik.LozinkaHash = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
        korisnik.DatumRegistracije = DateTime.Now;

        // Brisanje plain lozinke iz objekta pre cuvanja
        korisnik.Lozinka = null;
        korisnik.PotvrdaLozinke = null;

        await _db.DodajKorisnikaAsync(korisnik);

        // Prebaci na Login posle uspešne registracije
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
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

        var korisnik = await _db.NadjiKorisnikaAsync(korisnickoIme);
        if (korisnik == null || !BCrypt.Net.BCrypt.Verify(lozinka, korisnik.LozinkaHash))
        {
            ModelState.AddModelError("", "Neispravno korisničko ime ili lozinka.");
            return View();
        }

        // Postavljanje session podataka
        HttpContext.Session.SetString("korisnikId", korisnik.Id);
        HttpContext.Session.SetString("korisnickoIme", korisnik.KorisnickoIme);

        // Prebaci na glavnu stranicu (npr. Recenzije/Index)
        return RedirectToAction("Index", "Recenzije");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
