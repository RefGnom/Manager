﻿// <auto-generated />
using System;
using ManagerService.Server.Layers.DbLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ManagerService.Server.Layers.DbLayer.Migrations
{
    [DbContext(typeof(ManagerDbContext))]
    partial class ManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ManagerService.Server.Layers.DbLayer.Dbos.TimerDbo", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("PingTimeout")
                        .HasColumnType("interval");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "Name");

                    b.ToTable("Timers");
                });

            modelBuilder.Entity("ManagerService.Server.Layers.DbLayer.Dbos.TimerSessionDbo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsOver")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("StopTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TimerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("TimerSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
