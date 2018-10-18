using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Signal.ViewModels
{
    public class MessageUserViewModel
    {
        public string Id { get; set; }

        public string FromId { get; set; }

        public string ToId { get; set; }

        public string Message { get; set; }

        public DateTime TimeStamp
        {
            get;
            set;
        }
    }
}

