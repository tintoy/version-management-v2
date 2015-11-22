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
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.Release", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<int>("ProductId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DD.Cloud.VersionManagement.DataAccess.Models.Release", b =>
                {
                    b.HasOne("DD.Cloud.VersionManagement.DataAccess.Models.Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });
        }
    }
}
