using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Domain.DTOs.File
{
    public class S3ResponseDTO
    {
        public string url { get; set; }
        public string method { get; set; }
        public DateTime expired { get; set; }
    }
}
