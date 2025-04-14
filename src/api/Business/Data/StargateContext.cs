// using Microsoft.EntityFrameworkCore;
// using System.Data;

// namespace StargateAPI.Business.Data
// {
//     public class StargateContext : DbContext
//     {
//         // public IDbConnection Connection => Database.GetDbConnection();
//         // public DbSet<Person> People { get; set; }
//         // public DbSet<AstronautDetail> AstronautDetails { get; set; }
//         // public DbSet<AstronautDuty> AstronautDuties { get; set; }
//         // public DbSet<ProcessLog> ProcessLogs { get; set; }

//         public StargateContext(DbContextOptions<StargateContext> options)
//         : base(options)
//         {
//         }

//         public DbSet<Person> People { get; set; }      // Base type
//         public DbSet<Astronaut> Astronauts { get; set; }  // Derived type
//         public DbSet<AstronautDuty> AstronautDuties { get; set; } // Another entity

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);

//             modelBuilder.Entity<Person>()
//                 .HasIndex(p => p.Name)
//                 .IsUnique();

//             SeedData(modelBuilder);

//             base.OnModelCreating(modelBuilder);
//         }

//         private static void SeedData(ModelBuilder modelBuilder)
//         {
//             //add seed data
//             modelBuilder.Entity<Person>()
//                 .HasData(
//                     new Person
//                     {
//                         Id = 1,
//                         Name = "John Doe"
//                     },
//                     new Person
//                     {
//                         Id = 2,
//                         Name = "Jane Doe"
//                     },
//                     new Person { Id = 3, Name = "Cool Guy" },
//                     new Person { Id = 4, Name = "Not so cool guy" }
//                 );

//             modelBuilder.Entity<AstronautDetail>()
//                 .HasData(
//                     new AstronautDetail
//                     {
//                         Id = 1,
//                         PersonId = 1,
//                         CurrentRank = "1LT",
//                         CurrentDutyTitle = "Commander",
//                         CareerStartDate = DateTime.Now
//                     }
//                 );

//             modelBuilder.Entity<AstronautDuty>()
//                 .HasData(
//                     new AstronautDuty
//                     {
//                         Id = 1,
//                         PersonId = 1,
//                         DutyStartDate = DateTime.Now,
//                         DutyTitle = "Commander",
//                         Rank = "1LT"
//                     }
//                 );
//         }
//     }
// }
using Microsoft.EntityFrameworkCore;

namespace StargateAPI.Business.Data
{
    public class StargateContext : DbContext
    {
        public StargateContext(DbContextOptions<StargateContext> options)
            : base(options)
        {
        }

        // Base + Derived
        public DbSet<Person> People { get; set; }
        public DbSet<Astronaut> Astronauts { get; set; }

        // Duties referencing Astronaut
        public DbSet<AstronautDuty> AstronautDuties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                        .ToTable("Person");

            modelBuilder.Entity<Astronaut>()
                        .ToTable("Astronaut");

            modelBuilder.Entity<AstronautDuty>()
                        .ToTable("AstronautDuty");

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Name)
                .IsUnique();


            //SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        // private static void SeedData(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Person>()
        //         .HasData(
        //             new Person { Id = 1, Name = "John Doe" },
        //             new Person { Id = 2, Name = "Jane Doe" },
        //             new Person { Id = 3, Name = "Cool Guy" },
        //             new Person { Id = 4, Name = "Not so cool guy" }
        //         );

        //     modelBuilder.Entity<Astronaut>()
        //         .HasData(
        //             new Astronaut
        //             {
        //                 Id = 1,
        //                 Rank = "1LT",
        //                 CurrentDutyTitle = "Commander",
        //                 CareerStartDate = DateTime.UtcNow
        //             }
        //         );

        //     modelBuilder.Entity<AstronautDuty>()
        //         .HasData(
        //             new AstronautDuty
        //             {
        //                 Id = 1,
        //                 AstronautId = 1,
        //                 DutyTitle = "Commander",
        //                 Rank = "1LT",
        //                 DutyStartDate = DateTime.UtcNow
        //             }
        //         );
        // }
    }
}
