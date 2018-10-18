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
    public class SignalSessions
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string SessionId { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

    }
}
