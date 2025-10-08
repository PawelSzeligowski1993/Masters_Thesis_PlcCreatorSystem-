using Microsoft.EntityFrameworkCore;
using PlcCreatorSystem_API.Models;
using PlcCreatorSystem_Utility;
using static PlcCreatorSystem_Utility.SD;

namespace PlcCreatorSystem_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<PLC> PLCs { get; set; }
        public DbSet<HMI> HMIs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PLC>()
                .HasOne(p => p.LocalUser)
                .WithMany()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HMI>()
                .HasOne(h => h.LocalUser)
                .WithMany()
                .HasForeignKey(h => h.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.LocalUser)
                .WithMany()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LocalUser>().HasData
                (
                    new LocalUser()
                    {
                        Id = 1,
                        UserName = "PawelAdmin",
                        Name = "Pawel",
                        Password = "Pawel123",
                        Role = "admin"
                    },
                    new LocalUser()
                    {
                        Id = 2,
                        UserName = "JanAdmin",
                        Name = "Jan",
                        Password = "Jan123",
                        Role = "admin"
                    },
                    new LocalUser()
                    {
                        Id = 3,
                        UserName = "AdamEngineer",
                        Name = "Adam",
                        Password = "Adam123",
                        Role = "engineer"
                    },
                    new LocalUser()
                    {
                        Id = 4,
                        UserName = "RobertCustom",
                        Name = "Robert",
                        Password = "Robert123",
                        Role = "custom"
                    }
                );



            modelBuilder.Entity<PLC>().HasData(

                new PLC()
                {
                    Id = 1,
                    Name = "PLC1",
                    Subnet_X1 = "Network1_PLC1",
                    IP_X1 = "10.101.10.11",
                    Subnet_X2 = "Network2_PLC1",
                    IP_X2 = "10.100.10.10",
                    Identyfier = "6ES7 516-3AN02-0AB0/V2.9",
                    Details = "PLC1 - This is TEST",
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new PLC()
                {
                    Id = 2,
                    Name = "PLC2",
                    Subnet_X1 = "Network1_PLC2",
                    IP_X1 = "10.102.10.21",
                    Subnet_X2 = "Network2_PLC2",
                    IP_X2 = "10.100.10.20",
                    Identyfier = "6ES7 516-3AN02-0AB0/V2.9",
                    Details = "PLC2 - This is TEST",
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new PLC()
                {
                    Id = 3,
                    Name = "PLC3",
                    Subnet_X1 = "Network1_PLC3",
                    IP_X1 = "10.103.10.31",
                    Subnet_X2 = "Network2_PLC3",
                    IP_X2 = "10.100.10.30",
                    Identyfier = "6ES7 516-3AN02-0AB0/V2.9",
                    Details = "PLC3 - This is TEST, fill database PLC3, and Project3",
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                });
            modelBuilder.Entity<HMI>().HasData(
                new HMI()
                {
                    Id = 1,
                    Name = "HMI1",
                    IP = "10.101.10.100",
                    Identyfier = "6AV2 124-0UC02-0AX0/17.0.0.0",
                    Details = "HMI1 - TEST",
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new HMI()
                {
                    Id = 2,
                    Name = "HMI2",
                    IP = "10.102.10.100",
                    Identyfier = "6AV2 124-0UC02-0AX0/17.0.0.0",
                    Details = "HMI2 - TEST",
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new HMI()
                {
                    Id = 3,
                    Name = "HMI3",
                    IP = "10.103.10.100",
                    Identyfier = "6AV2 124-0UC02-0AX0/17.0.0.0",
                    Details = "HMI3 - TEST, fill database HMI3, and Project3",
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                });
            modelBuilder.Entity<Project>().HasData(
                new Project()
                {
                    Id = 1,
                    Name = "Project1",
                    PlcID = 3,
                    //PLC = new PLC()
                    //{
                    //    Id = 3,
                    //    Name = "PLC3",
                    //    Subnet_X1 = "Network1_PLC3",
                    //    IP_X1 = "10.103.10.31",
                    //    Subnet_X2 = "Network2_PLC3",
                    //    IP_X2 = "10.100.10.30",
                    //    Identyfier = "6ES7 516-3AN02-0AB0/V2.9",
                    //    Details = "PLC3 - This is TEST, fill database PLC3, and Project3",
                    //    CreatedDate = DateTime.Now,
                    //    UpdatedDate = DateTime.Now
                    //},
                    HmiID = 3,
                    //HMI = new HMI()
                    //{
                    //    Id = 3,
                    //    Name = "HMI3",
                    //    IP = "10.103.10.100",
                    //    Identyfier = "6AV2 124-0UC02-0AX0/17.0.0.0",
                    //    Details = "HMI3 - TEST, fill database HMI3, and Project3",
                    //    CreatedDate = DateTime.Now,
                    //},
                    CustomerDetails = "Firma Krzak",
                    Status = SD.ProjectStatus.waiting_to_check,
                    UserID = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                });
        }
    }
}
