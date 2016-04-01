using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DD.Cloud.VersionManagement.DataAccess;

namespace versionmanagement.Migrations
{
    [DbContext(typeof(VersionManagementEntities))]
    partial class VersionManagementEntitiesModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("ProductId");

                    b.Property<string>("SpecialVersion");

                    b.Property<int>("VersionRangeId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseVersion", b =>
                {
                    b.Property<string>("CommitId");

                    b.Property<int>("ReleaseId");

                    b.Property<string>("SpecialVersion")
                        .IsRequired();

                    b.Property<int>("VersionBuild");

                    b.Property<int>("VersionMajor");

                    b.Property<int>("VersionMinor");

                    b.Property<int>("VersionRevision");

                    b.HasKey("CommitId", "ReleaseId");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.VersionRange", b =>
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
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.Release", b =>
                {
                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.VersionRange")
                        .WithMany()
                        .HasForeignKey("VersionRangeId");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.ReleaseVersion", b =>
                {
                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.Release")
                        .WithMany()
                        .HasForeignKey("ReleaseId");
                });
        }
    }
}
