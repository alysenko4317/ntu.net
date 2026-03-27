using Microsoft.EntityFrameworkCore.Migrations;

namespace app3.Migrations
{
    public partial class CreateSchoolDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentGroupSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroupSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTypeSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentSubjectTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTypeSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectTypeSet_SubjectTypeSet_ParentSubjectTypeId",
                        column: x => x.ParentSubjectTypeId,
                        principalTable: "SubjectTypeSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<int>(type: "int", nullable: false),
                    SubjectTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectSet_SubjectTypeSet_SubjectTypeId",
                        column: x => x.SubjectTypeId,
                        principalTable: "SubjectTypeSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroupSubject",
                columns: table => new
                {
                    StudentGroupsId = table.Column<int>(type: "int", nullable: false),
                    SubjectsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroupSubject", x => new { x.StudentGroupsId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_StudentGroupSubject_StudentGroupSet_StudentGroupsId",
                        column: x => x.StudentGroupsId,
                        principalTable: "StudentGroupSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentGroupSubject_SubjectSet_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "SubjectSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroupSubject_SubjectsId",
                table: "StudentGroupSubject",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectSet_SubjectTypeId",
                table: "SubjectSet",
                column: "SubjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTypeSet_ParentSubjectTypeId",
                table: "SubjectTypeSet",
                column: "ParentSubjectTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentGroupSubject");

            migrationBuilder.DropTable(
                name: "StudentGroupSet");

            migrationBuilder.DropTable(
                name: "SubjectSet");

            migrationBuilder.DropTable(
                name: "SubjectTypeSet");
        }
    }
}
