using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Signal.ViewModels
{
    public class TotalUsersViewModel
    {
        public float UserCount { get; set; }

        public List<AppUsersViewModel> UserList { get; set; }

        public int Pagination { get; set; }
    }
}

