using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DD.Cloud.VersionManagement.DataAccess;

namespace VersionManagement.Migrations
{
    [DbContext(typeof(VersionManagementEntities))]
    [Migration("20160616234626_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901");

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ProductData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<string>("SpecialVersion");

                    b.Property<int>("VersionRangeId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("VersionRangeId");

                    b.ToTable("Release");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseVersionData", b =>
                {
                    b.Property<string>("CommitId");

                    b.Property<int>("ReleaseId");

                    b.Property<int>("FromVersionRangeId");

                    b.Property<string>("SpecialVersion")
                        .IsRequired();

                    b.Property<int>("VersionBuild");

                    b.Property<int>("VersionMajor");

                    b.Property<int>("VersionMinor");

                    b.Property<int>("VersionRevision");

                    b.HasKey("CommitId", "ReleaseId");

                    b.HasIndex("FromVersionRangeId");

                    b.HasIndex("ReleaseId");

                    b.ToTable("ReleaseVersion");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.VersionRangeData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EndVersionBuild");

                    b.Property<int>("EndVersionMajor");

                    b.Property<int>("EndVersionMinor");

                    b.Property<int>("EndVersionRevision");

                    b.Property<int>("IncrementBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("NextVersionBuild");

                    b.Property<int>("NextVersionMajor");

                    b.Property<int>("NextVersionMinor");

                    b.Property<int>("NextVersionRevision");

                    b.Property<int>("StartVersionBuild");

                    b.Property<int>("StartVersionMajor");

                    b.Property<int>("StartVersionMinor");

                    b.Property<int>("StartVersionRevision");

                    b.HasKey("Id");

                    b.ToTable("VersionRange");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseData", b =>
                {
                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.ProductData")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.VersionRangeData")
                        .WithMany()
                        .HasForeignKey("VersionRangeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseVersionData", b =>
                {
                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.VersionRangeData")
                        .WithMany()
                        .HasForeignKey("FromVersionRangeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseData")
                        .WithMany()
                        .HasForeignKey("ReleaseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
