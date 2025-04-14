using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("Astronaut")]
    public class Astronaut : Person
    {
        public string Rank { get; set; } = string.Empty;
        public string CurrentDutyTitle { get; set; } = string.Empty;
        public DateTime? CareerStartDate { get; set; }
        public DateTime? CareerEndDate { get; set; }

        public ICollection<AstronautDuty>? Duties { get; set; }
    }

    public class AstronautConfiguration : IEntityTypeConfiguration<Astronaut>
    {
        public void Configure(EntityTypeBuilder<Astronaut> builder)
        {
            builder.ToTable("Astronaut");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Rank)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.CurrentDutyTitle)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.CareerStartDate)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
