using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H5_SSP_aflevering.Code
{
    public class Encryption
    {
        public string Encrypt(string payload, IDataProtector _protector)
        {
            return _protector.Protect(payload);
        }

        public string Decrypt(string payload, IDataProtector _protector)
        {
            return _protector.Unprotect(payload);
        }
    }
}
