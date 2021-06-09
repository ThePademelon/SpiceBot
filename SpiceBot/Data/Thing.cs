
using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SpiceBot.Data
{
    public class Thing
    {
        [Key]
        public Guid ThingId { get; set; }
        public string Name { get; set; }
        public bool IsBased { get; set; }
    }
}
