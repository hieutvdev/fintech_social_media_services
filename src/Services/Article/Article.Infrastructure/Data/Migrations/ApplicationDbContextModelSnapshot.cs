﻿// <auto-generated />
using System;
using Article.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Article.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Article.Domain.Models.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MainImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("PublishAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleCategory", b =>
                {
                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("ArticleId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ArticleCategories");
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleRequestPublish", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticleRequestPublishes");
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleTag", b =>
                {
                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.HasKey("ArticleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTags");
                });

            modelBuilder.Entity("Article.Domain.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("CategoryStatus")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Article.Domain.Models.ProcessingStep", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ArticleRequestPublishId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HandlerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArticleRequestPublishId");

                    b.ToTable("ProcessingSteps");
                });

            modelBuilder.Entity("Article.Domain.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleCategory", b =>
                {
                    b.HasOne("Article.Domain.Models.Article", null)
                        .WithMany("ArticleCategories")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Article.Domain.Models.Category", null)
                        .WithMany("ArticleCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleRequestPublish", b =>
                {
                    b.HasOne("Article.Domain.Models.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleTag", b =>
                {
                    b.HasOne("Article.Domain.Models.Article", null)
                        .WithMany("ArticleTags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Article.Domain.Models.Tag", null)
                        .WithMany("ArticleTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Article.Domain.Models.ProcessingStep", b =>
                {
                    b.HasOne("Article.Domain.Models.ArticleRequestPublish", null)
                        .WithMany("ProcessingSteps")
                        .HasForeignKey("ArticleRequestPublishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Article.Domain.Models.Article", b =>
                {
                    b.Navigation("ArticleCategories");

                    b.Navigation("ArticleTags");
                });

            modelBuilder.Entity("Article.Domain.Models.ArticleRequestPublish", b =>
                {
                    b.Navigation("ProcessingSteps");
                });

            modelBuilder.Entity("Article.Domain.Models.Category", b =>
                {
                    b.Navigation("ArticleCategories");
                });

            modelBuilder.Entity("Article.Domain.Models.Tag", b =>
                {
                    b.Navigation("ArticleTags");
                });
#pragma warning restore 612, 618
        }
    }
}
