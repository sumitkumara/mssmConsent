using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MssmConsent.Models;

namespace MssmConsent.Data
{
    public class MssmConsentContext : DbContext
    {
        public MssmConsentContext (DbContextOptions<MssmConsentContext> options)
            : base(options)
        {
        }

        public DbSet<Consent> Consent { get; set; }
        public DbSet<ConsentSection> ConsentSection { get; set; }
        public DbSet<Title> Title { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Language> Language { get; set; }
    }
}
