using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodOrdering.Models
{
    public class CustomerDetails
    {
        public int id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }
    }
}
