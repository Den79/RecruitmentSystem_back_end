﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecruitmentSystemAPI.Models;

namespace RecruitmentSystemAPI.Migrations
{
    [DbContext(typeof(RecruitmentSystemContext))]
    [Migration("20200430035843_AddChargeAmountToLabourerJob")]
    partial class AddChargeAmountToLabourerJob
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            ConcurrencyStamp = "1",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "2",
                            ConcurrencyStamp = "2",
                            Name = "Company",
                            NormalizedName = "COMPANY"
                        },
                        new
                        {
                            Id = "3",
                            ConcurrencyStamp = "3",
                            Name = "Labourer",
                            NormalizedName = "LABOURER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Province")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<float>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.CompanyUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UserId");

                    b.ToTable("CompanyUsers");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Province")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<float>("Rating");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Weekdays");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.JobSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("JobId");

                    b.Property<int>("NumberOfLabourersNeeded");

                    b.Property<int>("SkillId");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("SkillId");

                    b.ToTable("JobSkills");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.Labourer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<int>("Availability");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<string>("PersonalId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .HasMaxLength(10);

                    b.Property<string>("Province")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<float>("QualityRating")
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<float>("SafetyRating")
                        .HasMaxLength(450)
                        .IsUnicode(false);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Labourers");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.LabourerJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("ChargeAmount")
                        .HasColumnType("decimal(18, 0)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("JobId");

                    b.Property<int?>("JobRating");

                    b.Property<int>("LabourerId");

                    b.Property<int?>("QualityRating");

                    b.Property<int?>("SafetyRating");

                    b.Property<int>("SkillId");

                    b.Property<decimal>("WageAmount")
                        .HasColumnType("decimal(18, 0)");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("LabourerId");

                    b.HasIndex("SkillId");

                    b.ToTable("LabourerJobs");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.LabourerSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LabourerId");

                    b.Property<int>("SkillId");

                    b.HasKey("Id");

                    b.HasIndex("LabourerId");

                    b.HasIndex("SkillId");

                    b.ToTable("LabourerSkills");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("ChargeAmount")
                        .HasColumnType("decimal(18, 0)");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false);

                    b.Property<decimal>("PayAmount")
                        .HasColumnType("decimal(18, 0)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.SystemUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RecruitmentSystemAPI.Models.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.SystemUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.CompanyUser", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.Company", "Company")
                        .WithMany("CompanyUsers")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_CompanyUsers_Companies");

                    b.HasOne("RecruitmentSystemAPI.Models.SystemUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.Job", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.Company", "Company")
                        .WithMany("Jobs")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_Jobs_Companies");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.JobSkill", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.Job", "Job")
                        .WithMany("JobSkills")
                        .HasForeignKey("JobId")
                        .HasConstraintName("FK_JobSkills_Jobs");

                    b.HasOne("RecruitmentSystemAPI.Models.Skill", "Skill")
                        .WithMany("JobSkills")
                        .HasForeignKey("SkillId")
                        .HasConstraintName("FK_JobSkills_Skills");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.Labourer", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.SystemUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.LabourerJob", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.Job", "Job")
                        .WithMany("LabourerJobs")
                        .HasForeignKey("JobId")
                        .HasConstraintName("FK_LabourerJob_Jobs");

                    b.HasOne("RecruitmentSystemAPI.Models.Labourer", "Labourer")
                        .WithMany("LabourerJobs")
                        .HasForeignKey("LabourerId")
                        .HasConstraintName("FK_LabourerJob_Labourers");

                    b.HasOne("RecruitmentSystemAPI.Models.Skill", "Skill")
                        .WithMany("LabourerJobs")
                        .HasForeignKey("SkillId")
                        .HasConstraintName("FK_LabourerJob_Skills");
                });

            modelBuilder.Entity("RecruitmentSystemAPI.Models.LabourerSkill", b =>
                {
                    b.HasOne("RecruitmentSystemAPI.Models.Labourer", "Labourer")
                        .WithMany("LabourerSkills")
                        .HasForeignKey("LabourerId")
                        .HasConstraintName("FK_LabourerSkills_Labourers");

                    b.HasOne("RecruitmentSystemAPI.Models.Skill", "Skill")
                        .WithMany("LabourerSkills")
                        .HasForeignKey("SkillId")
                        .HasConstraintName("FK_LabourerSkills_Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
