using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Signal.Models
{
    public class Messages
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string FromId { get; set; }

        [Required]
        public string ToId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
