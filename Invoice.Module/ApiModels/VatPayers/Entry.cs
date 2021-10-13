using Invoice.Module.ApiModels.VatPayers.Error;
using System.Collections.Generic;

namespace Invoice.Module.ApiModels.VatPayers
{
    public class Entry
    {
        public string Identifier { get; set; }
        public List<Entity> Subjects { get; set; }
        public ApiException Error { get; set; }
    }
}
