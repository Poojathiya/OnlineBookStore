using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OnlineBookStore.Models
{
    public partial class LibraryDbContext : DbContext
    {
        public LibraryDbContext()
        {
        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BooksDetail> BooksDetails { get; set; }
        public virtual DbSet<Booksupplier> Booksuppliers { get; set; }
        public virtual DbSet<Mycart> Mycarts { get; set; }
        public virtual DbSet<NewRegistration> NewRegistrations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=LibraryDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BooksDetail>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__BooksDet__3DE0C20785703D80");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BookImage)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Genre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Stock).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Sup)
                    .WithMany(p => p.BooksDetails)
                    .HasForeignKey(d => d.SupId)
                    .HasConstraintName("FK__BooksDeta__SupId__412EB0B6");
            });

            modelBuilder.Entity<Booksupplier>(entity =>
            {
                entity.HasKey(e => e.SupId)
                    .HasName("PK__Booksupp__4D238596E7F808EC");

                entity.Property(e => e.MailId)
                    .HasMaxLength(100)
                    .HasColumnName("Mail_id");

                entity.Property(e => e.SupAddress)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SupName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mycart>(entity =>
            {
                entity.HasKey(e => e.CartId)
                    .HasName("PK__Mycart__51BCD7B76D0370BB");

                entity.ToTable("Mycart");

                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.BookName).HasMaxLength(50);

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Mycarts)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Mycart__BookId__628FA481");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Mycarts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Mycart__UserId__619B8048");
            });

            modelBuilder.Entity<NewRegistration>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__NewRegis__1788CC4CE601E093");

                entity.ToTable("NewRegistration");

                entity.Property(e => e.Address)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MailId)
                    .HasMaxLength(100)
                    .HasColumnName("Mail_id");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Ph_number");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
