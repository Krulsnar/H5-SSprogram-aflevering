using H5_SSP_aflevering.Code;
using H5_SSP_aflevering.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace H5_SSP_aflevering.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtector _dataProtector;
        private readonly Encryption _encryption;

        public HomeController(
            ILogger<HomeController> logger,
            IDataProtectionProvider dataProtector,
            Encryption encryption
            )
        {
            _logger = logger;
            _dataProtector = dataProtector.CreateProtector("testKey");
            _encryption = encryption;
        }

        public IActionResult Index()
        {
            string input = "Hello Andreas og Linette";
            string encryptedInput = _encryption.Encrypt(input, _dataProtector);
            string decryptedInput = _encryption.Decrypt(encryptedInput, _dataProtector);

            IndexModel inputModel = new IndexModel()
            {
                EncryptedDescription = encryptedInput,
                DecryptedDescription = decryptedInput
            };
            
            return View(model: inputModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
