using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Keyless_Entry_Transmission.Models;
using Keyless_Entry_Transmission.Services;

namespace Keyless_Entry_Transmission.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConnectionService _connectionService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _connectionService = new ConnectionService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ConnectSuccess()
        {
            var bitString = "";
            for (int i = 0; i < 40; i++)
            {
                bitString += "0";
            }

            _connectionService.Connect("192.168.1.144", "567432" + " " + bitString);

            return View();
        }

        public IActionResult ConnectKeyFailure()
        {
            var bitString = "";
            for (int i = 0; i < 40; i++)
            {
                bitString += "0";
            }

            _connectionService.Connect("192.168.1.144", "999996" + " " + bitString);

            return View();
        }

        public IActionResult ConnectTransmissionFailure()
        {
            var random = new Random();
            var bytes = new byte[5];
            random.NextBytes(bytes);
            var bitString = BitConverter.ToString(bytes);

            _connectionService.Connect("192.168.1.144", "567432" + " " + bitString);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
