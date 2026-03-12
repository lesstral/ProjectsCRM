using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class ListDTO<T>
    {
        public int TotalItemCount { get; set; }
        public List<T> Items { get; set; }
    }
}
