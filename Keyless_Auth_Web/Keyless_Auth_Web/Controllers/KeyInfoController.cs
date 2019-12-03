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
    public class KeyInfoController : Controller
    {
        private readonly KeyInfoContext _context;

        public KeyInfoController(KeyInfoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(KeyInfo keyInfo)
        {
            _context.Add(keyInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "CarInfo", null);
        }
    }
}