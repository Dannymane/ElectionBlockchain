using ElectionBlockchain.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

public class ApplicationDbContext : DbContext
{
   // table properties
   public virtual DbSet<Node>? Nodes { get; set; }
   public virtual DbSet<Citizen> Citizens { get; set; }
   public virtual DbSet<Candidate> Candidates { get; set; }
   public virtual DbSet<Block> Blocks { get; set; }
   public virtual DbSet<Vote> Votes { get; set; } //accepted votes
   public virtual DbSet<VoteQueue> VotesQueue { get; set; } //for storing incoming votes

   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
   : base(options) { }
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
         .HasForeignKey(v => v.CandidateId); 

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

      modelBuilder.Entity<Vote>()
          .HasOne(v => v.Citizen)
          .WithOne(c => c.Vote)
          .HasForeignKey<Citizen>(c => c.VoteId)
          .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Citizen if VoteId is set

      modelBuilder.Entity<Citizen>()
          .HasOne(c => c.Vote)
          .WithOne(v => v.Citizen)
          .HasForeignKey<Vote>(v => v.CitizenDocumentId);

      modelBuilder.Entity<Vote>()
         .HasOne(v => v.Candidate)
         .WithMany(c => c.Votes)
         .HasForeignKey(v => v.CandidateId);

      modelBuilder.Entity<VoteQueue>()
         .HasKey(vq => vq.CitizenDocumentId);

   }
}