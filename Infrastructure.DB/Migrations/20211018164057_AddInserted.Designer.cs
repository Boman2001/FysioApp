﻿// <auto-generated />
using System;
using Core.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211018164057_AddInserted")]
    partial class AddInserted
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Domain.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommentBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsVisiblePatient")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("isPostedOnId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("isPostedOnId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Core.Domain.Models.DiagnoseCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocationBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pathology")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DiagnoseCodes");
                });

            modelBuilder.Entity("Core.Domain.Models.Dossier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiagnoseCodeId")
                        .HasColumnType("int");

                    b.Property<int?>("IntakeById")
                        .HasColumnType("int");

                    b.Property<bool>("IsByStudent")
                        .HasColumnType("bit");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SupervisedById")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IntakeById");

                    b.HasIndex("PatientId");

                    b.HasIndex("SupervisedById");

                    b.ToTable("Dossiers");
                });

            modelBuilder.Entity("Core.Domain.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoomNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoomType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Core.Domain.Models.Treatment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DossierId")
                        .HasColumnType("int");

                    b.Property<int?>("ExcecutedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExcecutedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Particulatities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("TreatmentCodeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TreatmentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DossierId");

                    b.HasIndex("ExcecutedById");

                    b.HasIndex("RoomId");

                    b.ToTable("Treatment");
                });

            modelBuilder.Entity("Core.Domain.Models.TreatmentCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ExplanationRequired")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TreatmentCodes");
                });

            modelBuilder.Entity("Core.Domain.Models.TreatmentPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<float>("TimePerSession")
                        .HasColumnType("real");

                    b.Property<int>("TreatmentCodeId")
                        .HasColumnType("int");

                    b.Property<int>("TreatmentsPerWeek")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TreatmentPlans");
                });

            modelBuilder.Entity("Core.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Preposition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Domain.Models.Doctor", b =>
                {
                    b.HasBaseType("Core.Domain.Models.User");

                    b.Property<string>("BigNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Core.Domain.Models.Patient", b =>
                {
                    b.HasBaseType("Core.Domain.Models.User");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Core.Domain.Models.Student", b =>
                {
                    b.HasBaseType("Core.Domain.Models.User");

                    b.Property<string>("StudentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Core.Domain.Models.Comment", b =>
                {
                    b.HasOne("Core.Domain.Models.User", "CreatedBy")
                        .WithMany("CommentsCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Domain.Models.Dossier", "isPostedOn")
                        .WithMany("Comments")
                        .HasForeignKey("isPostedOnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CreatedBy");

                    b.Navigation("isPostedOn");
                });

            modelBuilder.Entity("Core.Domain.Models.Dossier", b =>
                {
                    b.HasOne("Core.Domain.Models.User", "IntakeBy")
                        .WithMany("IntakesDone")
                        .HasForeignKey("IntakeById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Core.Domain.Models.Patient", "Patient")
                        .WithMany("Dossiers")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Core.Domain.Models.User", "SupervisedBy")
                        .WithMany("IntakesSupervised")
                        .HasForeignKey("SupervisedById")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("IntakeBy");

                    b.Navigation("Patient");

                    b.Navigation("SupervisedBy");
                });

            modelBuilder.Entity("Core.Domain.Models.Treatment", b =>
                {
                    b.HasOne("Core.Domain.Models.Dossier", "Dossier")
                        .WithMany("Treatments")
                        .HasForeignKey("DossierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Domain.Models.User", "ExcecutedBy")
                        .WithMany("TreatmentsDone")
                        .HasForeignKey("ExcecutedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Domain.Models.TreatmentPlan", "TreatmentPlan")
                        .WithOne("Treatment")
                        .HasForeignKey("Core.Domain.Models.Treatment", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Models.Room", "Room")
                        .WithMany("Treatments")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dossier");

                    b.Navigation("ExcecutedBy");

                    b.Navigation("Room");

                    b.Navigation("TreatmentPlan");
                });

            modelBuilder.Entity("Core.Domain.Models.Doctor", b =>
                {
                    b.HasOne("Core.Domain.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Doctor", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Patient", b =>
                {
                    b.HasOne("Core.Domain.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Patient", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Student", b =>
                {
                    b.HasOne("Core.Domain.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Student", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Dossier", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("Core.Domain.Models.Room", b =>
                {
                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("Core.Domain.Models.TreatmentPlan", b =>
                {
                    b.Navigation("Treatment");
                });

            modelBuilder.Entity("Core.Domain.Models.User", b =>
                {
                    b.Navigation("CommentsCreated");

                    b.Navigation("IntakesDone");

                    b.Navigation("IntakesSupervised");

                    b.Navigation("TreatmentsDone");
                });

            modelBuilder.Entity("Core.Domain.Models.Patient", b =>
                {
                    b.Navigation("Dossiers");
                });
#pragma warning restore 612, 618
        }
    }
}
