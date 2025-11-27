using System.Collections.Generic;

namespace Contracts
{
    public class SnapshotRowDto
    {
        public string BtcFieldName { get; set; }
        public string BtcFieldValue { get; set; }

        public SnapshotRowDto()
        {
            BtcFieldName = string.Empty;
            BtcFieldValue = string.Empty;
        }
    }

    public class SnapshotDto
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public List<SnapshotRowDto> Rows { get; set; }

        public SnapshotDto()
        {
            Note = string.Empty;
            Rows = new List<SnapshotRowDto>();
        }
    }
}
