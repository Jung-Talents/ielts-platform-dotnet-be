using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IeltsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ielts_tests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_name = table.Column<string>(type: "text", nullable: false),
                    skill = table.Column<string>(type: "text", nullable: false),
                    duration = table.Column<short>(type: "smallint", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Draft"),
                    slug = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_ielts_tests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_type = table.Column<string>(type: "text", nullable: false),
                    instruction = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<short>(type: "smallint", nullable: false),
                    category = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_question_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "temp_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    verification_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_temp_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_email_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    role = table.Column<string>(type: "text", nullable: false, defaultValue: "Student"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_login_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listening_sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<short>(type: "smallint", nullable: false),
                    audio = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_listening_sections", x => x.id);
                    table.ForeignKey(
                        name: "f_k_listening_sections_ielts_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "ielts_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reading_sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_reading_sections", x => x.id);
                    table.ForeignKey(
                        name: "f_k_reading_sections_ielts_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "ielts_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "writing_sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_writing_sections", x => x.id);
                    table.ForeignKey(
                        name: "f_k_writing_sections_ielts_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "ielts_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<short>(type: "smallint", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_questions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_questions__question_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "question_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ielts_test_results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false),
                    total_correct_answers = table.Column<short>(type: "smallint", nullable: false),
                    user_submission = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    detail_analysis = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    time_spent = table.Column<float>(type: "real", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_ielts_test_results", x => x.id);
                    table.ForeignKey(
                        name: "f_k_ielts_test_results__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_ielts_test_results_ielts_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "ielts_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ip_address = table.Column<string>(type: "text", nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "f_k_refresh_tokens__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answer_keys",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    test_id = table.Column<Guid>(type: "uuid", nullable: false),
                    answers = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    clarification = table.Column<string>(type: "text", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_answer_keys", x => x.id);
                    table.ForeignKey(
                        name: "f_k_answer_keys__ielts_tests_test_id",
                        column: x => x.test_id,
                        principalTable: "ielts_tests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_answer_keys__questions_question_id",
                        column: x => x.question_id,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_answer_keys_question_id",
                table: "answer_keys",
                column: "question_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_answer_keys_test_id",
                table: "answer_keys",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "idx_ielts_test_result_test",
                table: "ielts_test_results",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "idx_ielts_test_result_test_user",
                table: "ielts_test_results",
                columns: new[] { "test_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "idx_ielts_test_result_user",
                table: "ielts_test_results",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_ielts_tests_deleted_at",
                table: "ielts_tests",
                column: "deleted_at");

            migrationBuilder.CreateIndex(
                name: "idx_ielts_tests_skill_order",
                table: "ielts_tests",
                columns: new[] { "skill", "order" });

            migrationBuilder.CreateIndex(
                name: "ix_ielts_tests_slug",
                table: "ielts_tests",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ielts_tests_test_name",
                table: "ielts_tests",
                column: "test_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_listening_sections_test_order",
                table: "listening_sections",
                columns: new[] { "test_id", "order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_question_groups_section_id_section_type",
                table: "question_groups",
                columns: new[] { "section_id", "section_type" });

            migrationBuilder.CreateIndex(
                name: "uq_question_groups_section_order",
                table: "question_groups",
                columns: new[] { "section_id", "order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_questions_group_order",
                table: "questions",
                columns: new[] { "group_id", "order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_reading_sections_test_id",
                table: "reading_sections",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id_user_agent",
                table: "refresh_tokens",
                columns: new[] { "user_id", "user_agent" });

            migrationBuilder.CreateIndex(
                name: "idx_temp_user_email",
                table: "temp_users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_writing_sections_test_id",
                table: "writing_sections",
                column: "test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer_keys");

            migrationBuilder.DropTable(
                name: "ielts_test_results");

            migrationBuilder.DropTable(
                name: "listening_sections");

            migrationBuilder.DropTable(
                name: "reading_sections");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "temp_users");

            migrationBuilder.DropTable(
                name: "writing_sections");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "ielts_tests");

            migrationBuilder.DropTable(
                name: "question_groups");
        }
    }
}
