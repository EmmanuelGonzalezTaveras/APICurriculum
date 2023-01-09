using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var rolAdminId = "ab9b9283-721d-4311-9333-f9c8989996a1";
            var usuarioAdminId = "20528816-cbe7-47cb-a6f2-1d9e3d4ad718";

            var rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var username = "emmanuel.g.t@hotmail.com";
            var usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                UserName = username,
                NormalizedUserName = username,
                Email = username,
                NormalizedEmail = username,
                PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
            };

            modelBuilder.Entity<IdentityUser>().HasData(usuarioAdmin);
            modelBuilder.Entity<IdentityRole>().HasData(rolAdmin);

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 1,
                    ClaimType = ClaimTypes.Role,
                    UserId = usuarioAdminId,
                    ClaimValue = "Admin"
                });

        }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////para hacer la llave compuesta por dos columnas
            //modelBuilder.Entity<ClientesPrestamos>()
            //    .HasKey(x => new { x.ClienteId, x.PrestamoID });

            modelBuilder.Entity<PrestamosCarteras>()
                .HasKey(x => new { x.PrestamoId, x.CarteraId });


            // modelBuilder.Entity<Log>().ToTable(nameof(Log), t => t.ExcludeFromMigrations());
            SeedData(modelBuilder);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Email> Emails { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Cartera> Carteras { get; set; }
        public DbSet<PrestamosCarteras> PrestamosCarteras { get; set; }

        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Seguimiento> Seguimientos { get; set; }


        [NotMapped]
        public DbSet<Log> Logs { get; set; }


    }
}
