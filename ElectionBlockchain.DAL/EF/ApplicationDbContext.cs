using ElectionBlockchain.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace ElectionBlockchain.DAL.EF;
public class ApplicationDbContext : DbContext
{
   // table properties
   public virtual DbSet<Node>? Nodes { get; set; }
   public virtual DbSet<Citizen> Citizens { get; set; }
   public virtual DbSet<Candidate> Candidates { get; set; }
   public virtual DbSet<Block> Blocks { get; set; }
   public virtual DbSet<Vote> Votes { get; set; } //accepted votes
   public virtual DbSet<VoteQueue> VotesQueue { get; set; } //for storing incoming votes

   public virtual DbSet<CitizenPrivateKey> CitizenPrivateKeys { get; set; } //the real app shouldn't store private keys in the database
   //CitizenPrivateKeys only for future testing and for the comfort of code presentation
   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
   : base(options) 
   {
      try
      {
         var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
         if (databaseCreator != null)
         {
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();

         }
      }
      catch (Exception ex)
      {
         Console.WriteLine(ex.Message);
      }

   }
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseLazyLoadingProxies();
   }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      //    default DeleteBehaviors:
      //many-to-one: .OnDelete(DeleteBehavior.Cascade); 
      //one-to-one: .OnDelete(DeleteBehavior.ClientSetNull); 

      //automatically
      //modelBuilder.Entity<Block>()
      //   .HasKey(b => b.Id);

      modelBuilder.Entity<Block>()
         .HasMany(b => b.Votes)
         .WithOne(v => v.Block)
         .HasForeignKey(v => v.BlockId)
         //.OnDelete(DeleteBehavior.Cascade);  //For testing. For real app it should be DeleteBehavior.Restrict
         .OnDelete(DeleteBehavior.Restrict);  // prevents block deletion if votes exist
         //.IsRequired(false); // Allow null values for BlockId in Vote table

      modelBuilder.Entity<Candidate>()
         .Property(c => c.Name)
         .IsRequired();

      modelBuilder.Entity<Candidate>()
         .Property(c => c.Surname)
         .IsRequired();

      modelBuilder.Entity<Candidate>()
         .HasMany(c => c.Votes)
         .WithOne(v => v.Candidate)
         .HasForeignKey(v => v.CandidateId); //Cascade
         //.OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Citizen>()
         .HasKey(c => new { c.DocumentId} );
      //.HasKey(c => new { c.DocumentId, c.PublicKey }); - cant make foreign key from Vote.CitizenDocumentId to Citizen.DocumentId

      modelBuilder.Entity<Citizen>()
         .Property(c => c.DocumentId)
         .HasMaxLength(6);

      modelBuilder.Entity<Vote>()
         .HasKey(v => v.CitizenDocumentId);

      modelBuilder.Entity<Vote>()
         .HasOne(v => v.Citizen)
         .WithOne(c => c.Vote)
         .HasForeignKey<Citizen>(c => c.VoteId)
         .IsRequired(false); // Allow null values for VoteId in Citizen table
                             //.OnDelete(DeleteBehavior.Cascade);

      
      modelBuilder.Entity<Vote>()
          .HasOne(v => v.Citizen)
          .WithOne(c => c.Vote)
          .HasForeignKey<Citizen>(c => c.VoteId)
          .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Citizen if VoteId is set

      modelBuilder.Entity<Citizen>()
          .HasOne(c => c.Vote)
          .WithOne(v => v.Citizen)
          .HasForeignKey<Vote>(v => v.CitizenDocumentId)
          .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<Vote>()
         .HasOne(v => v.Candidate)
         .WithMany(c => c.Votes)
         .HasForeignKey(v => v.CandidateId)
         .OnDelete(DeleteBehavior.Cascade); 

      modelBuilder.Entity<VoteQueue>()
         .HasKey(vq => vq.CitizenDocumentId);

      modelBuilder.Entity<CitizenPrivateKey>()
         .HasKey(cpk => cpk.DocumentId);

   }
}