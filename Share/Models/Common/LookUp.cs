using System.Collections.Generic;

namespace Share.Models.Common
{
    public class LookUp
    {
        public List<KeyValueDescription> CategoryOptionsBy { get; set; }
        public List<KeyValueDescription> ConditionOptionsBy { get; set; }
        public List<KeyValueDescription> MileOptionsBy { get; set; }
        public List<KeyValueDescription> SortOptionsBy { get; set; }
    }
}
