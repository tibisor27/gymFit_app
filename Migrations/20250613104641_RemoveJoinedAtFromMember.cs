﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymFit.BE.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJoinedAtFromMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
