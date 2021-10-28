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
    [Migration("20211027210835_appointemnts1")]
    partial class appointemnts1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Domain.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DossierId")
                        .HasColumnType("int");

                    b.Property<int>("Room")
                        .HasColumnType("int");

                    b.Property<DateTime>("TreatmentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TreatmentEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isTreatment")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DossierId");

                    b.ToTable("Appointments");

                    b.HasDiscriminator<bool>("isTreatment").HasValue(false);
                });

            modelBuilder.Entity("Core.Domain.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommentBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
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

            modelBuilder.Entity("Core.Domain.Models.Dossier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DiagnoseCodeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DismissionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("HeadPractitionerId")
                        .HasColumnType("int");

                    b.Property<string>("Housenumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IntakeById")
                        .HasColumnType("int");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("bit");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SupervisedById")
                        .HasColumnType("int");

                    b.Property<int?>("TreatmentPlanId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("HeadPractitionerId");

                    b.HasIndex("IntakeById");

                    b.HasIndex("PatientId");

                    b.HasIndex("SupervisedById");

                    b.HasIndex("TreatmentPlanId");

                    b.ToTable("Dossiers");
                });

            modelBuilder.Entity("Core.Domain.Models.TreatmentPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("TimePerSessionInMinutes")
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

            modelBuilder.Entity("Core.Domain.Models.Treatment", b =>
                {
                    b.HasBaseType("Core.Domain.Models.Appointment");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DossierId1")
                        .HasColumnType("int");

                    b.Property<int?>("ExcecutedById")
                        .HasColumnType("int");

                    b.Property<string>("Particulatities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TreatmentCodeId")
                        .HasColumnType("int");

                    b.HasIndex("DossierId1");

                    b.HasIndex("ExcecutedById");

                    b.HasDiscriminator().HasValue(true);
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

            modelBuilder.Entity("Core.Domain.Models.Staff", b =>
                {
                    b.HasBaseType("Core.Domain.Models.User");

                    b.Property<TimeSpan>("end")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("start")
                        .HasColumnType("time");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("Core.Domain.Models.Doctor", b =>
                {
                    b.HasBaseType("Core.Domain.Models.Staff");

                    b.Property<string>("BigNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Core.Domain.Models.Student", b =>
                {
                    b.HasBaseType("Core.Domain.Models.Staff");

                    b.Property<string>("StudentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Core.Domain.Models.Appointment", b =>
                {
                    b.HasOne("Core.Domain.Models.Dossier", "Dossier")
                        .WithMany("Appointments")
                        .HasForeignKey("DossierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dossier");
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
                    b.HasOne("Core.Domain.Models.User", "HeadPractitioner")
                        .WithMany("HeadPractisionerOf")
                        .HasForeignKey("HeadPractitionerId");

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

                    b.HasOne("Core.Domain.Models.TreatmentPlan", "TreatmentPlan")
                        .WithMany()
                        .HasForeignKey("TreatmentPlanId");

                    b.Navigation("HeadPractitioner");

                    b.Navigation("IntakeBy");

                    b.Navigation("Patient");

                    b.Navigation("SupervisedBy");

                    b.Navigation("TreatmentPlan");
                });

            modelBuilder.Entity("Core.Domain.Models.Treatment", b =>
                {
                    b.HasOne("Core.Domain.Models.Dossier", null)
                        .WithMany("Treatments")
                        .HasForeignKey("DossierId1");

                    b.HasOne("Core.Domain.Models.Staff", "ExcecutedBy")
                        .WithMany("TreatmentsDone")
                        .HasForeignKey("ExcecutedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ExcecutedBy");
                });

            modelBuilder.Entity("Core.Domain.Models.Patient", b =>
                {
                    b.HasOne("Core.Domain.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Patient", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Staff", b =>
                {
                    b.HasOne("Core.Domain.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Staff", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Doctor", b =>
                {
                    b.HasOne("Core.Domain.Models.Staff", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Doctor", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Student", b =>
                {
                    b.HasOne("Core.Domain.Models.Staff", null)
                        .WithOne()
                        .HasForeignKey("Core.Domain.Models.Student", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Domain.Models.Dossier", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Comments");

                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("Core.Domain.Models.User", b =>
                {
                    b.Navigation("CommentsCreated");

                    b.Navigation("HeadPractisionerOf");

                    b.Navigation("IntakesDone");

                    b.Navigation("IntakesSupervised");
                });

            modelBuilder.Entity("Core.Domain.Models.Patient", b =>
                {
                    b.Navigation("Dossiers");
                });

            modelBuilder.Entity("Core.Domain.Models.Staff", b =>
                {
                    b.Navigation("TreatmentsDone");
                });
#pragma warning restore 612, 618
        }
    }
}
