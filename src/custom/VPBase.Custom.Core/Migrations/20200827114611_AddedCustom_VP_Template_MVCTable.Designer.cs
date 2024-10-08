﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using VPBase.Custom.Core.Data;

namespace VPBase.Custom.Core.Migrations
{
    [DbContext(typeof(CustomDatabaseManager))]
    [Migration("20200827114611_AddedCustom_VP_Template_MVCTable")]
    partial class AddedCustom_VP_Template_MVCTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VPBase.Custom.Data.Entities.Custom_VP_Template_Mvc", b =>
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

                    b.ToTable("Custom_VP_Template_Mvcs");
                });
#pragma warning restore 612, 618
        }
    }
}
