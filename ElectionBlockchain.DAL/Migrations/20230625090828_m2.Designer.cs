﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ElectionBlockchain.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230625090828_m2")]
    partial class m2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstVerifierSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LeaderSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousBlockHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PreviousBlockId")
                        .HasColumnType("int");

                    b.Property<string>("SecondVerifierSignature")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PreviousBlockId");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Citizen", b =>
                {
                    b.Property<string>("DocumentId")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VoteId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId");

                    b.ToTable("Citizens");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Vote", b =>
                {
                    b.Property<string>("CitizenDocumentId")
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("BlockId")
                        .HasColumnType("int");

                    b.Property<int>("CandidateId")
                        .HasColumnType("int");

                    b.Property<string>("CitizenSignature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CitizenDocumentId");

                    b.HasIndex("BlockId");

                    b.HasIndex("CandidateId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.VoteQueue", b =>
                {
                    b.Property<string>("CitizenDocumentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CandidateId")
                        .HasColumnType("int");

                    b.Property<string>("CitizenSignature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CitizenDocumentId");

                    b.ToTable("VotesQueue");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Block", b =>
                {
                    b.HasOne("ElectionBlockchain.Model.DataModels.Block", "PreviousBlock")
                        .WithMany()
                        .HasForeignKey("PreviousBlockId");

                    b.Navigation("PreviousBlock");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Vote", b =>
                {
                    b.HasOne("ElectionBlockchain.Model.DataModels.Block", "Block")
                        .WithMany("Votes")
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ElectionBlockchain.Model.DataModels.Candidate", "Candidate")
                        .WithMany("Votes")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ElectionBlockchain.Model.DataModels.Citizen", "Citizen")
                        .WithOne("Vote")
                        .HasForeignKey("ElectionBlockchain.Model.DataModels.Vote", "CitizenDocumentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Block");

                    b.Navigation("Candidate");

                    b.Navigation("Citizen");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Block", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Candidate", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("ElectionBlockchain.Model.DataModels.Citizen", b =>
                {
                    b.Navigation("Vote");
                });
#pragma warning restore 612, 618
        }
    }
}
