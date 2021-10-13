using System;
using System.Collections.Generic;

namespace Invoice.Module.ApiModels.VatPayers
{
    public class EntityList
    {
        public List<Entity> Subjects { get; set; }
        public string RequestDateTime { get; set; }
        public string RequestId { get; set; }
    }
}
