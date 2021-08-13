using H5_SSP_aflevering.Areas.Identity.Code;
using H5_SSP_aflevering.Code;
using H5_SSP_aflevering.Models;
using H5ServersideProgrammering.Codes;
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
        private readonly Hashing _hashing;
        private readonly RoleHandler _roleHandler;
        private readonly IServiceProvider _serviceProvider;

        public HomeController(
            ILogger<HomeController> logger,
            IDataProtectionProvider dataProtector,
            Encryption encryption,
            Hashing hashing,
            RoleHandler roleHandler,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _dataProtector = dataProtector.CreateProtector("testKey");
            _encryption = encryption;
            _hashing = hashing;
            _roleHandler = roleHandler;
            _serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index(string input)
        {
            string txt = "Hello Andreas og Linette";
            string encryptedInput = _encryption.Encrypt(txt, _dataProtector);
            string decryptedInput = _encryption.Decrypt(encryptedInput, _dataProtector);

            await _roleHandler.CreateRole("Admin", _serviceProvider);
            await _roleHandler.SetRole("andreas@moinichen.com", "Admin", _serviceProvider);

            IndexModel inputModel = new IndexModel()
            {
                EncryptedDescription = encryptedInput,
                DecryptedDescription = decryptedInput,
                Input = input,
            };

            if (!String.IsNullOrEmpty(input))
            {
                inputModel.HashedText_MD5 = _hashing.GetHashedText_MD5(input);
                inputModel.HashedText_PBKDF2 = _hashing.GetHashedText_PBKDF2(input);
                inputModel.HashedText_BCrypt = _hashing.GetHashedText_BCrypt(input);

                return View(model: inputModel);
            }

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
