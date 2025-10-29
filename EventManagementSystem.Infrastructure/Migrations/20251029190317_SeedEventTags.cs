using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedEventTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EventTags",
                columns: new[] { "EventsId", "TagsId" },
                values: new object[,]
                {
                    { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ba") },
                    { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bb") },
                    { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") },
                    { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ad"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") },
                    { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ae"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") }
                });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "b0a0e3f2-46e9-4dc8-83f3-1234567890af", new DateTime(2025, 10, 31, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "b0a0e3f2-46e9-4dc8-83f3-1234567890af", new DateTime(2025, 10, 31, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "b0a0e3f2-46e9-4dc8-83f3-1234567890af", new DateTime(2025, 11, 6, 14, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ad"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "b0a0e3f2-46e9-4dc8-83f3-1234567890af", new DateTime(2025, 11, 9, 11, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ae"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { "b0a0e3f2-46e9-4dc8-83f3-1234567890af", new DateTime(2025, 11, 12, 16, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventTags",
                keyColumns: new[] { "EventsId", "TagsId" },
                keyValues: new object[] { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ba") });

            migrationBuilder.DeleteData(
                table: "EventTags",
                keyColumns: new[] { "EventsId", "TagsId" },
                keyValues: new object[] { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bb") });

            migrationBuilder.DeleteData(
                table: "EventTags",
                keyColumns: new[] { "EventsId", "TagsId" },
                keyValues: new object[] { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") });

            migrationBuilder.DeleteData(
                table: "EventTags",
                keyColumns: new[] { "EventsId", "TagsId" },
                keyValues: new object[] { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ad"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") });

            migrationBuilder.DeleteData(
                table: "EventTags",
                keyColumns: new[] { "EventsId", "TagsId" },
                keyValues: new object[] { new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ae"), new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890bc") });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890aa"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ab"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ac"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ad"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("b0a0e3f2-46e9-4dc8-83f3-1234567890ae"),
                columns: new[] { "CreatedBy", "CreatedDate" },
                values: new object[] { null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
