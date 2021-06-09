using System;
using System.ComponentModel.DataAnnotations;

namespace SpiceBot
{
    public class Statement
    {
        [Key]
        public Guid StatementId { get; set; }
        public string Format { get; set; }
        public bool Negates { get; set; }
    }
}