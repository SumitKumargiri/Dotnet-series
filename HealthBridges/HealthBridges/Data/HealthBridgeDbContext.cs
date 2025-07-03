using HealthBridges.Models;
using Microsoft.EntityFrameworkCore;

public class HealthBridgeDbContext : DbContext
{
    public HealthBridgeDbContext(DbContextOptions<HealthBridgeDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Nurse> Nurses { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<PatientDocument> PatientDocuments { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User-Patient relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Patient)
            .WithOne(p => p.User)
            .HasForeignKey<Patient>(p => p.UserId);

        // User-Admin relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Admin)
            .WithOne(a => a.User)
            .HasForeignKey<Admin>(a => a.UserId);

        // User-Doctor relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Doctor)
            .WithOne(d => d.User)
            .HasForeignKey<Doctor>(d => d.UserId);

        // User-Nurse relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Nurse)
            .WithOne(n => n.User)
            .HasForeignKey<Nurse>(n => n.UserId);


        // User-Appointments relationship (One-to-Many)
        modelBuilder.Entity<Appointment>()
       .HasOne(a => a.User)
       .WithMany(u => u.Appointments)
       .HasForeignKey(a => a.UserId);

        //modelBuilder.Entity<User>()
        //    .HasOne(u => u.PatientDocument)
        //    .WithOne(pd => pd.User)
        //    .HasForeignKey<PatientDocument>(pd => pd.UserId);

        base.OnModelCreating(modelBuilder);
    }
}
