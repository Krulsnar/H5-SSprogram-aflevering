using System;
using System.Collections.Generic;

#nullable disable

namespace H5_SSP_aflevering.Models
{
    public partial class Todo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
    }
}
