using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using KnjizevneKritikeApp.Models;

public class RecenzijeController : Controller
{
    private readonly MongoDbService _db;

    public RecenzijeController(MongoDbService db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var recenzije = await _db.VratiSveRecenzijeAsync();
        return View(recenzije);
    }

    [HttpPost]
    public async Task<IActionResult> DodajKomentar(string recenzijaId, string tekstKomentara)
    {
        if (!HttpContext.Session.Keys.Contains("korisnikId"))
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(tekstKomentara))
        {
            TempData["Greska"] = "Komentar ne može biti prazan.";
            return RedirectToAction("Index");
        }

        var komentar = new Komentar
        {
            Tekst = tekstKomentara,
            KorisnikId = HttpContext.Session.GetString("korisnikId"),
            KorisnickoIme = HttpContext.Session.GetString("korisnickoIme"),
            Datum = DateTime.Now
        };

        await _db.DodajKomentarAsync(recenzijaId, komentar);

        return RedirectToAction("Index");
    }
}

