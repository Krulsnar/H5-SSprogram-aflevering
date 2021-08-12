using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace H5_SSP_aflevering.Models
{
    public class IndexModel
    {
        public string Input { get; set; }
        public string HashedText_MD5 { get; set; }
        public string HashedText_BCrypt { get; set; }
        public string HashedText_PBKDF2 { get; set; }
        public string EncryptedDescription { get; set; }
        public string DecryptedDescription { get; set; }
    }
}
