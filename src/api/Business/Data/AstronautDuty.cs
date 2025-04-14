// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace StargateAPI.Business.Data
// {
//     [Table("AstronautDuty")]
//     public class AstronautDuty
//     {
//         public int Id { get; set; }
//         public int AstronautId { get; set; }
//         public string DutyName { get; set; } = string.Empty;
//         public DateTime DutyStartDate { get; set; }
//         public DateTime? DutyEndDate { get; set; }

//         public Astronaut? Astronaut { get; set; }
//     }

//     public class AstronautDutyConfiguration : IEntityTypeConfiguration<AstronautDuty>
//     {
//         public void Configure(EntityTypeBuilder<AstronautDuty> builder)
//         {
//             builder.ToTable("AstronautDuty");
//             builder.HasKey(d => d.Id);
//             builder.Property(d => d.DutyName)
//                    .IsRequired()
//                    .HasMaxLength(100);
//             builder.HasOne(d => d.Astronaut)
//                    .WithMany(a => a.Duties)
//                    .HasForeignKey(d => d.AstronautId);
//         }
//     }
// }
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data
{
    [Table("AstronautDuty")]
    public class AstronautDuty
    {
        public int Id { get; set; }
        public int AstronautId { get; set; }
        public string DutyTitle { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public DateTime DutyStartDate { get; set; }
        public DateTime? DutyEndDate { get; set; }

        public Astronaut? Astronaut { get; set; }
    }

    public class AstronautDutyConfiguration : IEntityTypeConfiguration<AstronautDuty>
    {
        public void Configure(EntityTypeBuilder<AstronautDuty> builder)
        {
            builder.ToTable("AstronautDuty");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.DutyTitle)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasOne(d => d.Astronaut)
                   .WithMany(a => a.Duties)
                   .HasForeignKey(d => d.AstronautId);
        }
    }
}
