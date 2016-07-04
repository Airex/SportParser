namespace TelegramBot.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SportParser : DbContext
    {
        public SportParser()
            : base("name=SportParser")
        {
        }

        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Settings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
