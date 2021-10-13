using System.Collections.Generic;

namespace Invoice.Module.ApiModels.VatPayers
{
    public class EntryList
    {
        public List<Entry> Entries { get; set; }
        public string RequestDateTime { get; set; } 
        public string RequestId { get; set; }
    }
}
