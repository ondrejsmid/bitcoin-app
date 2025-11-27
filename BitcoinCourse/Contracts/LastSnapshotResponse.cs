using System.Collections.Generic;

namespace Contracts
{
    public class LastSnapshotResponse
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public List<BtcRecord> Data { get; set; }

        public LastSnapshotResponse()
        {
            Data = new List<BtcRecord>();
        }
    }
}
