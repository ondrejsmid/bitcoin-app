using System.Collections.Generic;

namespace Contracts
{
    public class SnapshotRowDto
    {
        public string BtcFieldName { get; set; } = string.Empty;
        public string BtcFieldValue { get; set; } = string.Empty;
    }

    public class SnapshotDto
    {
        public int Id { get; set; }
        public string? Note { get; set; }
        public List<SnapshotRowDto> Rows { get; set; } = new();
    }
}
