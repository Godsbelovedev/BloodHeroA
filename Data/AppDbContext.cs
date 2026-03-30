using BloodHeroA.Models.Entities;
using BloodHeroA.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BloodHeroA.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<ReleasedBlood> ReleasedBloods { get; set; }
        public DbSet<RecipientOrganization> RecipientOrganizations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonationRequest> DonationRequests { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonorOrganization> DonorOrganizations { get; set; }
        public DbSet<BloodTestResult> BloodTestResults { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }
        public DbSet<BloodStorage> BloodStorages { get; set; }
        public DbSet<BankingOrganization> BankingOrganizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Donor>()
           .HasOne(d => d.User)
           .WithOne(u => u.Donor)
           .HasForeignKey<Donor>(d => d.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipientOrganization>()
           .HasOne(d => d.User)
           .WithOne(u => u.RecipientOrganization)
           .HasForeignKey<RecipientOrganization>(d => d.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BankingOrganization>()
            .HasOne(d => d.User)
            .WithOne(u => u.BankingOrganization)
            .HasForeignKey<BankingOrganization>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DonorOrganization>()
            .HasOne(d => d.User)
            .WithOne(u => u.DonorOrganization)
            .HasForeignKey<DonorOrganization>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
           .HasOne(n => n.Sender)
           .WithMany(u => u.SentNotifications)
           .HasForeignKey(n => n.SenderId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
            .HasOne(n => n.Receiver)
            .WithMany(u => u.ReceivedNotifications)
            .HasForeignKey(n => n.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

           // modelBuilder.Entity<Notification>()
           //.HasOne(n => n.Receiver)
           //.WithMany(u => u.ReceivedNotifications)
           //.HasForeignKey(n => n.ReceiverId);

            modelBuilder.Entity<ReleasedBlood>()
           .HasOne(n => n.DonationRequest)
           .WithMany(u => u.ReleasedBloods)
           .HasForeignKey(n => n.DonationRequestId);


            modelBuilder.Entity<ReleasedBlood>()
           .HasOne(n => n.BankingOrganization)
           .WithMany(u => u.ReleasedBloods)
           .HasForeignKey(n => n.BankingOrganizationId);

            modelBuilder.Entity<ReleasedBlood>()
          .HasOne(n => n.RecipientOrganization)
          .WithMany(u => u.ReleasedBloods)
          .HasForeignKey(n => n.RecipientOrganizationId);

           

            modelBuilder.Entity<DonationRequest>()
            .HasOne(dr => dr.RecipientOrganization)
            .WithMany(ro => ro.DonationRequests)
            .HasForeignKey(dr => dr.RecipientOrganizationId);

            modelBuilder.Entity<DonationRequest>()
           .HasOne(dr => dr.BankingOrganization)
           .WithMany(ro => ro.DonationRequests)
           .HasForeignKey(dr => dr.BankingOrganizationId);

            

            modelBuilder.Entity<Donor>()
           .HasOne(d => d.DonorOrganization)
           .WithMany(d => d.Donors)
           .HasForeignKey(r => r.DonorOrganizationId);

            

         //   modelBuilder.Entity<DonationRequest>()
         // .HasOne(d => d.BankingOrganization)
         // .WithMany(d => d.DonationRequests)
         // .HasForeignKey(r => r.BankingOrganizationId);

         //   modelBuilder.Entity<DonationRequest>()
         //.HasOne(d => d.RecipientOrganization)
         //.WithMany(d => d.DonationRequests)
         //.HasForeignKey(r => r.RecipientOrganizationId);

            modelBuilder.Entity<Donation>()
         .HasOne(d => d.BankingOrganization)
         .WithMany(d => d.Donations)
         .HasForeignKey(r => r.BankingOrganizationId);

            modelBuilder.Entity<Donation>()
          .HasOne(d => d.DonorOrganization)
          .WithMany(d => d.Donations)
          .HasForeignKey(r => r.DonorOrganizationId);

            modelBuilder.Entity<Donation>()
       .HasOne(d => d.Donor)
       .WithMany(d => d.Donations)
       .HasForeignKey(d => d.DonorId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BloodTestResult>()
           .HasOne(d => d.Donation)
           .WithOne(d => d.BloodTestResult)
           .HasForeignKey<BloodTestResult>(r => r.DonationId);

          //modelBuilder.Entity<BloodTestResult>()
          //.HasOne(d => d.Donor)
          //.WithOne(d => d.BloodTestResult)
          //.HasForeignKey<BloodTestResult>(r => r.DonorId);

         modelBuilder.Entity<BloodTestResult>()
        .HasOne(d => d.BankingOrganization)
        .WithMany(d => d.BloodTestResults)
        .HasForeignKey(r => r.BankingOrganizationId);

        // modelBuilder.Entity<BloodStorage>()
        //.HasOne(d => d.Donor)
        //.WithOne(d => d.BloodStorage)
        //.HasForeignKey<BloodStorage>(r => r.DonorId);

        modelBuilder.Entity<BloodStorage>()
       .HasOne(d => d.Donation)
       .WithOne(d => d.BloodStorage)
       .HasForeignKey<BloodStorage>(r => r.DonationId);

        modelBuilder.Entity<BloodStorage>()
       .HasOne(d => d.BankingOrganization)
       .WithMany(d => d.BloodStorages)
       .HasForeignKey(r => r.BankingOrganizationId);

            // modelBuilder.Entity<BloodInventory>()
            //.HasOne(d => d.BloodStorage)
            //.WithOne(d => d.BloodInventory)
            //.HasForeignKey<BloodInventory>(r => r.BloodStorageId);
 

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.Parse("dd38778b-eab3-4107-82f3-81e2c9d0f4d9"),
                Email = "admin@bloodhero.com",
                IsAvailable = true,
                Role = Role.Admin,
                HashPassWord = "$2a$11$6oa/w00m3.g4NkrAqjNH9eORe/7vKmeLs4Cpn2xvJLGE3zq4TKlmG", //Admin@123
                FullName = "BloodHero Admin",
                PhoneNumber = "07043138331",

            });
        }

    }
}