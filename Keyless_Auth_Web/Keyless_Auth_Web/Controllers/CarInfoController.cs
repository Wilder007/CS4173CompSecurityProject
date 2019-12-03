using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Keyless_Auth_Web.Data;
using Keyless_Auth_Web.Models;

namespace Keyless_Auth_Web.Controllers
{
    public class CarInfoController : Controller
    {
        private readonly CarInfoContext _context;

        public CarInfoController(CarInfoContext context)
        {
            _context = context;
        }

        // GET: CarInfo
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarInfo.Where(x=>x.Owner.Equals("Treyson")).ToListAsync());
        }

        public async Task<IActionResult> Unlock()
        {
            return View();
        }

        // GET: CarInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carInfo = await _context.CarInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carInfo == null)
            {
                return NotFound();
            }

            return View(carInfo);
        }

        // GET: CarInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SendSMS,PhoneNum,Email")] CarInfo carInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carInfo);
        }

        // GET: CarInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carInfo = await _context.CarInfo.FindAsync(id);
            if (carInfo == null)
            {
                return NotFound();
            }
            return View(carInfo);
        }

        // POST: CarInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarInfo carInfo)
        {
            if (id != carInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var keyId = Request.Form["KeyId"].ToString();
                    //Upload KeyId to sql.
                    KeyInfo keyInfo = new KeyInfo(keyId);
                    keyInfo.Car_Id = carInfo.Id;
                    


                    //_keyContext.Add(keyInfo);
                    //await _keyContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarInfoExists(carInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carInfo);
        }

        // GET: CarInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carInfo = await _context.CarInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carInfo == null)
            {
                return NotFound();
            }

            return View(carInfo);
        }

        // POST: CarInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carInfo = await _context.CarInfo.FindAsync(id);
            _context.CarInfo.Remove(carInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarInfoExists(int id)
        {
            return _context.CarInfo.Any(e => e.Id == id);
        }
    }
}
