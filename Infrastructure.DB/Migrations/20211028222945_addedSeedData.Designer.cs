// <auto-generated />
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
    [Migration("20211028222945_addedSeedData")]
    partial class addedSeedData
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

                    b.Property<int?>("ExcecutedById")
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

                    b.HasIndex("ExcecutedById");

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

                    b.Property<int?>("IntakeById")
                        .HasColumnType("int");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("bit");

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

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
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Preposition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email")
                        .HasName("AlternateKey_Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Domain.Models.Treatment", b =>
                {
                    b.HasBaseType("Core.Domain.Models.Appointment");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DossierId1")
                        .HasColumnType("int");

                    b.Property<string>("Particulatities")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TreatmentCodeId")
                        .HasColumnType("int");

                    b.HasIndex("DossierId1");

                    b.HasDiscriminator().HasValue(true);
                });

            modelBuilder.Entity("Core.Domain.Models.Patient", b =>
                {
                    b.HasBaseType("Core.Domain.Models.User");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatientNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Patients");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2021, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(2966),
                            Email = "Danmaarkaas2@gmail.com",
                            FirstName = "Paula",
                            LastName = "PatientenBerg",
                            Preposition = "van der",
                            BirthDay = new DateTime(1965, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(3627),
                            Gender = 1,
                            PatientNumber = "dfb87071-7eef-4e81-91ab-461d5e2c5fad",
                            PhoneNumber = "0636303815",
                            PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2021, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(4474),
                            Email = "Danmaarkaas3@gmail.com",
                            FirstName = "Pavlov",
                            LastName = "PatientStan",
                            Preposition = "",
                            BirthDay = new DateTime(2000, 10, 29, 0, 29, 45, 47, DateTimeKind.Local).AddTicks(4487),
                            Gender = 0,
                            PatientNumber = "5b01effe-2016-4597-94b5-a537db81b354",
                            PhoneNumber = "0636303816",
                            PictureUrl = "506cf9b3-c437-46bd-944e-3dfcb1d17e8b_9.jpg"
                        });
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2021, 10, 29, 0, 29, 45, 28, DateTimeKind.Local).AddTicks(5369),
                            Email = "Danmaarkaas1@gmail.com",
                            FirstName = "Dirk",
                            LastName = "DoctorMan",
                            Preposition = "De",
                            end = new TimeSpan(0, 0, 0, 0, 0),
                            start = new TimeSpan(0, 0, 0, 0, 0),
                            BigNumber = "29292929929",
                            EmployeeNumber = "0636303815",
                            PhoneNumber = "0636303815"
                        });
                });

            modelBuilder.Entity("Core.Domain.Models.Student", b =>
                {
                    b.HasBaseType("Core.Domain.Models.Staff");

                    b.Property<string>("StudentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2021, 10, 29, 0, 29, 45, 49, DateTimeKind.Local).AddTicks(9073),
                            Email = "Danmaarkaas@gmail.com",
                            FirstName = "Stefan",
                            LastName = "Student",
                            Preposition = "De",
                            end = new TimeSpan(0, 0, 0, 0, 0),
                            start = new TimeSpan(0, 0, 0, 0, 0),
                            StudentNumber = "2153494"
                        });
                });

            modelBuilder.Entity("Core.Domain.Models.Appointment", b =>
                {
                    b.HasOne("Core.Domain.Models.Dossier", "Dossier")
                        .WithMany("Appointments")
                        .HasForeignKey("DossierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Domain.Models.Staff", "ExcecutedBy")
                        .WithMany("TreatmentsDone")
                        .HasForeignKey("ExcecutedById")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Dossier");

                    b.Navigation("ExcecutedBy");
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
