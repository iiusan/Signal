using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Signal.DataLayer.Core
{
    public class Communicator
    {
        protected static dynamic _db;

        public Communicator(dynamic dbContext)
        {
            _db = dbContext;
        }
    }
}
