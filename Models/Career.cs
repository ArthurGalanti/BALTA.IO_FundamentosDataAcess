using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BALTA.IO_FundamentosDataAcess.Models
{
    public class Career
    {
        public Career()
        {
            Items = new List<CareerItem>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public IList<CareerItem> Items { get; set; }
    }
}