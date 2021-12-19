using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Blazor.Server.Dtos
{
    public class UploadFileDto
    {
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
