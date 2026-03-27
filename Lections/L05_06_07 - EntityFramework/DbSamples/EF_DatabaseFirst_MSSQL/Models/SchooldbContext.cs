using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFDemo_DBFirstApp.Models;

public partial class SchooldbContext : DbContext
{
    public SchooldbContext()
    {
    }

    public SchooldbContext(DbContextOptions<SchooldbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<StudentGroupSet> StudentGroupSets { get; set; }

    public virtual DbSet<SubjectSet> SubjectSets { get; set; }

    public virtual DbSet<SubjectTypeSet> SubjectTypeSets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=schooldb;Trusted_Connection=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentGroupSet>(entity =>
        {
            entity.ToTable("StudentGroupSet");

            entity.HasMany(d => d.Subjects).WithMany(p => p.StudentGroups)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentGroupSubject",
                    r => r.HasOne<SubjectSet>().WithMany().HasForeignKey("SubjectsId"),
                    l => l.HasOne<StudentGroupSet>().WithMany().HasForeignKey("StudentGroupsId"),
                    j =>
                    {
                        j.HasKey("StudentGroupsId", "SubjectsId");
                        j.ToTable("StudentGroupSubject");
                        j.HasIndex(new[] { "SubjectsId" }, "IX_StudentGroupSubject_SubjectsId");
                    });
        });

        modelBuilder.Entity<SubjectSet>(entity =>
        {
            entity.ToTable("SubjectSet");

            entity.HasIndex(e => e.SubjectTypeId, "IX_SubjectSet_SubjectTypeId");

            entity.HasOne(d => d.SubjectType).WithMany(p => p.SubjectSets).HasForeignKey(d => d.SubjectTypeId);
        });

        modelBuilder.Entity<SubjectTypeSet>(entity =>
        {
            entity.ToTable("SubjectTypeSet");

            entity.HasIndex(e => e.ParentSubjectTypeId, "IX_SubjectTypeSet_ParentSubjectTypeId");

            entity.HasOne(d => d.ParentSubjectType).WithMany(p => p.InverseParentSubjectType).HasForeignKey(d => d.ParentSubjectTypeId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
