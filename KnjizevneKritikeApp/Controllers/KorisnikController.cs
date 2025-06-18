using KnjizevneKritikeApp.Models;
using KnjizevneKritikeApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace KnjizevneKritikeApp.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly KorisnikService _service;

        public KorisnikController(KorisnikService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var korisnici = await _service.GetAllAsync();
            return View(korisnici);
        }

        public async Task<IActionResult> Details(string id)
        {
            var korisnik = await _service.GetByIdAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Korisnik korisnik)
        {
            if (!ModelState.IsValid) return View(korisnik);
            await _service.CreateAsync(korisnik);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var korisnik = await _service.GetByIdAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Korisnik korisnik)
        {
            if (!ModelState.IsValid) return View(korisnik);
            await _service.UpdateAsync(id, korisnik);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var korisnik = await _service.GetByIdAsync(id);
            if (korisnik == null) return NotFound();
            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
