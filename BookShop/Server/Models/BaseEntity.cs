using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Server.Models
{
    public class BaseEntity
    {
        public Guid Id { get; init; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
