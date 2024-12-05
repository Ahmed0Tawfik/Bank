using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Bank
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<LoanRequest> LoanRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().Property(a => a.FirstName).IsRequired()
                .HasMaxLength(50);

            builder.Entity<Account>().Property(a => a.LastName).IsRequired()
                .HasMaxLength(50);

            builder.Entity<Account>().Property(a => a.UniqueId)
                .ValueGeneratedOnAdd();

            builder.Entity<Account>().HasIndex(Account => Account.UniqueId)
                .IsUnique()
                .HasDatabaseName("IX_Accounts_UniqueId");

            builder.Entity<Account>().HasMany(builder => builder.Transactions)
                .WithOne(transaction => transaction.Account)
                .HasForeignKey(transaction => transaction.AccountId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" });

            var adminId = Guid.NewGuid().ToString();
            var adminUser = new Account
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Hash the password
            var passwordHasher = new PasswordHasher<Account>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@1234");

            builder.Entity<Account>().HasData(adminUser);

            // Assign role to user
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminId,
                RoleId = "1"
            });
        }
    }
   
}
