﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Shared.PresentationModels
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Roles { get; set; }
    }
}
