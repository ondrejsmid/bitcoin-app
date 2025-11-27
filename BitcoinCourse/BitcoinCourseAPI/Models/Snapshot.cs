using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BitcoinCourseAPI.Models
{
    public class Snapshot
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }

        public List<SnapshotRow> Rows { get; set; } = new();
    }

    public class SnapshotRow
    {
        public int Id { get; set; }

        public int SnapshotId { get; set; }

        [Required]
        [MaxLength(255)]
        public string BtcFieldName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string BtcFieldValue { get; set; } = string.Empty;

        public Snapshot? Snapshot { get; set; }
    }
}
