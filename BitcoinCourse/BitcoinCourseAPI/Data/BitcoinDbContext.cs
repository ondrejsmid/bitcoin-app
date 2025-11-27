using Microsoft.EntityFrameworkCore;
using BitcoinCourseAPI.Models;

namespace BitcoinCourseAPI.Data
{
    public class BitcoinDbContext : DbContext
    {
        public BitcoinDbContext(DbContextOptions<BitcoinDbContext> options) : base(options)
        {
        }

        public DbSet<Snapshot> Snapshots { get; set; } = null!;
        public DbSet<SnapshotRow> SnapshotRows { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Snapshot>(eb =>
            {
                eb.ToTable("snapshot");
                eb.HasKey(e => e.Id);
                eb.Property(e => e.Id).HasColumnName("id");
                eb.Property(e => e.Note).HasColumnName("note").HasMaxLength(255);
            });

            modelBuilder.Entity<SnapshotRow>(eb =>
            {
                eb.ToTable("snapshot_row");
                eb.HasKey(e => e.Id);
                eb.Property(e => e.Id).HasColumnName("id");
                eb.Property(e => e.SnapshotId).HasColumnName("snapshot_id");
                eb.Property(e => e.BtcFieldName).HasColumnName("btc_fieldname").HasMaxLength(255).IsRequired();
                eb.Property(e => e.BtcFieldValue).HasColumnName("btc_fieldvalue").HasMaxLength(255).IsRequired();

                eb.HasOne(e => e.Snapshot)
                    .WithMany(s => s.Rows)
                    .HasForeignKey(e => e.SnapshotId)
                    .HasConstraintName("FK_snapshotrow_snapshot")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
