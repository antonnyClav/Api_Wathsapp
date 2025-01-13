using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class ResponseDTO <T> where T : class
    {
        public Status status { get; set; }
        public T? data { get; set; }
        public string? messages { get; set; }
        public List<string>? errors { get; set; } = new List<string>();
    }
}
