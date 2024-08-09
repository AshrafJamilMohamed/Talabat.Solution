using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductSpecParams
    {
        public int? BrandId { get; set; }
        public string? Sort { get; set; }
        public int? CategoryId { get; set; }

        private int pageSize;

        private string? search;

        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
 
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }

        
        public int PageIndex { get; set; } = 1;
    }
}
