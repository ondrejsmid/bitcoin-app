using System.Collections.Generic;

namespace Contracts
{
    public class SaveSnapshotRequest
    {
        public string Note { get; set; } = string.Empty;
        public List<BtcRecord> Records { get; set; } = new List<BtcRecord>();
    }
}
