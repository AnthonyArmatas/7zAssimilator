using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7zAssimilator.Models
{
    public class MassFileZipConvertModel
    {
        public int CompressedCount { get; set; }
        public int ErrorCount { get; set; }
        public bool Successful { get; set; }
    }
}
