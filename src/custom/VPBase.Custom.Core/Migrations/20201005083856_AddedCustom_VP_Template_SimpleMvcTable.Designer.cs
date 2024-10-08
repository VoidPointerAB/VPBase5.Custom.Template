﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VPBase.Custom.Core.Data;

namespace VPBase.Custom.Core.Migrations
{
    [DbContext(typeof(CustomDatabaseManager))]
    [Migration("20201005083856_AddedCustom_VP_Template_SimpleMvcTable")]
    partial class AddedCustom_VP_Template_SimpleMvcTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VPBase.Custom.Core.Data.Entities.Custom_VP_Template_Mvc", b =>
                {
                    b.Property<string>("Custom_VP_Template_MvcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("VP_Template_MvcId");

                    b.Property<DateTime?>("AnonymizedUtc");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime?>("DeletedUtc");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedUtc");

                    b.Property<string>("TenantId")
                        .IsRequired();

                    b.Property<string>("Title");

                    b.HasKey("Custom_VP_Template_MvcId");

                    b.ToTable("VP_Template_Mvcs");
                });

            modelBuilder.Entity("VPBase.Custom.Core.Data.Entities.Custom_VP_Template_SimpleMvc", b =>
                {
                    b.Property<string>("Custom_VP_Template_SimpleMvcId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Custom_VP_Template_SimpleMvcId");

                    b.Property<DateTime?>("AnonymizedUtc");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime?>("DeletedUtc");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedUtc");

                    b.Property<string>("TenantId")
                        .IsRequired();

                    b.Property<string>("Title");

                    b.HasKey("Custom_VP_Template_SimpleMvcId");

                    b.ToTable("Custom_VP_Template_SimpleMvcs");
                });

            modelBuilder.Entity("VPBase.Custom.Core.Data.Entities.VP_Template_GraphQL", b =>
                {
                    b.Property<string>("VP_Template_GraphQLId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("VP_Template_GraphQLId");

                    b.Property<DateTime?>("AnonymizedUtc");

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<DateTime?>("DeletedUtc");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModifiedUtc");

                    b.Property<string>("TenantId")
                        .IsRequired();

                    b.Property<string>("Title");

                    b.HasKey("VP_Template_GraphQLId");

                    b.ToTable("VP_Template_GraphQLs");
                });
#pragma warning restore 612, 618
        }
    }
}
