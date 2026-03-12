using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs
{
    public class QueryDTO
    {
        [Required]
        public int Page { get; set; } = 1;

        [Range(0, 100)]
        [Required]
        public int ItemsPerPage { get; set; } = 20;

        [MaxLength(50)]
        public string? SortBy { get; set; }
        public bool Descending { get; set; } = false;
    }
}
