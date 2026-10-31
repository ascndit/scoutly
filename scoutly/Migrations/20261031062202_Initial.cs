using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace scoutly.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "error_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    request_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    error_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    stack_trace = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("error_logs_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("permissions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("roles_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "skill_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("skill_categories_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_settings",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("system_settings_pkey", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_permissions_pkey", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "fk_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                    table.ForeignKey(
                        name: "users_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "skills",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("skills_pkey", x => x.id);
                    table.ForeignKey(
                        name: "skills_category_id_fkey",
                        column: x => x.category_id,
                        principalTable: "skill_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    owner_user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    industry = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    contact_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    contact_phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("companies_pkey", x => x.id);
                    table.ForeignKey(
                        name: "companies_owner_user_id_fkey",
                        column: x => x.owner_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Info'::character varying"),
                    target_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("notifications_pkey", x => x.id);
                    table.ForeignKey(
                        name: "notifications_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    device_info = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("refresh_tokens_pkey", x => x.id);
                    table.ForeignKey(
                        name: "refresh_tokens_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specialist_profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    headline = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    telegram = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    job_search_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'ActivelySearching'::character varying"),
                    is_ready_to_relocate = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    prefer_remote = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    expected_salary = table.Column<int>(type: "integer", nullable: true),
                    about_me = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("specialist_profiles_pkey", x => x.id);
                    table.ForeignKey(
                        name: "specialist_profiles_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vacancies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    company_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    responsibilities = table.Column<string>(type: "text", nullable: true),
                    requirements = table.Column<string>(type: "text", nullable: true),
                    terms = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    work_format = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Office'::character varying"),
                    employment_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'FullTime'::character varying"),
                    salary_min = table.Column<int>(type: "integer", nullable: true),
                    salary_max = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Published'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("vacancies_pkey", x => x.id);
                    table.ForeignKey(
                        name: "vacancies_company_id_fkey",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "educations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    specialist_profile_id = table.Column<int>(type: "integer", nullable: false),
                    institution_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    faculty = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    degree = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    start_year = table.Column<int>(type: "integer", nullable: true),
                    end_year = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("educations_pkey", x => x.id);
                    table.ForeignKey(
                        name: "educations_specialist_profile_id_fkey",
                        column: x => x.specialist_profile_id,
                        principalTable: "specialist_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specialist_skills",
                columns: table => new
                {
                    specialist_profile_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    experience_years = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    skill_level = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValueSql: "'Middle'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("specialist_skills_pkey", x => new { x.specialist_profile_id, x.skill_id });
                    table.ForeignKey(
                        name: "specialist_skills_skill_id_fkey",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "specialist_skills_specialist_profile_id_fkey",
                        column: x => x.specialist_profile_id,
                        principalTable: "specialist_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_experiences",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    specialist_profile_id = table.Column<int>(type: "integer", nullable: false),
                    company_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("work_experiences_pkey", x => x.id);
                    table.ForeignKey(
                        name: "work_experiences_specialist_profile_id_fkey",
                        column: x => x.specialist_profile_id,
                        principalTable: "specialist_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    vacancy_id = table.Column<int>(type: "integer", nullable: false),
                    specialist_profile_id = table.Column<int>(type: "integer", nullable: false),
                    cover_letter = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'New'::character varying"),
                    applied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("applications_pkey", x => x.id);
                    table.ForeignKey(
                        name: "applications_specialist_profile_id_fkey",
                        column: x => x.specialist_profile_id,
                        principalTable: "specialist_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "applications_vacancy_id_fkey",
                        column: x => x.vacancy_id,
                        principalTable: "vacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recruiter_candidates",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    recruiter_user_id = table.Column<int>(type: "integer", nullable: false),
                    specialist_profile_id = table.Column<int>(type: "integer", nullable: false),
                    vacancy_id = table.Column<int>(type: "integer", nullable: true),
                    match_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    stage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "'Scouted'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("recruiter_candidates_pkey", x => x.id);
                    table.ForeignKey(
                        name: "recruiter_candidates_recruiter_user_id_fkey",
                        column: x => x.recruiter_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "recruiter_candidates_specialist_profile_id_fkey",
                        column: x => x.specialist_profile_id,
                        principalTable: "specialist_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "recruiter_candidates_vacancy_id_fkey",
                        column: x => x.vacancy_id,
                        principalTable: "vacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "vacancy_skills",
                columns: table => new
                {
                    vacancy_id = table.Column<int>(type: "integer", nullable: false),
                    skill_id = table.Column<int>(type: "integer", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    min_experience_years = table.Column<decimal>(type: "numeric(4,1)", precision: 4, scale: 1, nullable: true),
                    weight = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false, defaultValue: 1.00m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("vacancy_skills_pkey", x => new { x.vacancy_id, x.skill_id });
                    table.ForeignKey(
                        name: "vacancy_skills_skill_id_fkey",
                        column: x => x.skill_id,
                        principalTable: "skills",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "vacancy_skills_vacancy_id_fkey",
                        column: x => x.vacancy_id,
                        principalTable: "vacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "interaction_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    recruiter_candidate_id = table.Column<int>(type: "integer", nullable: false),
                    author_user_id = table.Column<int>(type: "integer", nullable: false),
                    action_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("interaction_history_pkey", x => x.id);
                    table.ForeignKey(
                        name: "interaction_history_author_user_id_fkey",
                        column: x => x.author_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "interaction_history_recruiter_candidate_id_fkey",
                        column: x => x.recruiter_candidate_id,
                        principalTable: "recruiter_candidates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_applications_specialist_profile_id",
                table: "applications",
                column: "specialist_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_applications_vacancy_id",
                table: "applications",
                column: "vacancy_id");

            migrationBuilder.CreateIndex(
                name: "ix_applications_vacancy_id_specialist_profile_id",
                table: "applications",
                columns: new[] { "vacancy_id", "specialist_profile_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_companies_owner_user_id",
                table: "companies",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_educations_specialist_profile_id",
                table: "educations",
                column: "specialist_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_interaction_history_author_user_id",
                table: "interaction_history",
                column: "author_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_interaction_history_recruiter_candidate_id",
                table: "interaction_history",
                column: "recruiter_candidate_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_recruiter_candidates_recruiter_user_id",
                table: "recruiter_candidates",
                column: "recruiter_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_recruiter_candidates_recruiter_user_id_specialist_profile_i",
                table: "recruiter_candidates",
                columns: new[] { "recruiter_user_id", "specialist_profile_id", "vacancy_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_recruiter_candidates_specialist_profile_id",
                table: "recruiter_candidates",
                column: "specialist_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_recruiter_candidates_vacancy_id",
                table: "recruiter_candidates",
                column: "vacancy_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_token",
                table: "refresh_tokens",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_skills_category_id",
                table: "skills",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_specialist_profiles_city",
                table: "specialist_profiles",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "ix_specialist_profiles_job_search_status",
                table: "specialist_profiles",
                column: "job_search_status");

            migrationBuilder.CreateIndex(
                name: "ix_specialist_profiles_user_id",
                table: "specialist_profiles",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_specialist_skills_skill_id",
                table: "specialist_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_vacancies_city",
                table: "vacancies",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "ix_vacancies_company_id",
                table: "vacancies",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_vacancies_status",
                table: "vacancies",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_vacancy_skills_skill_id",
                table: "vacancy_skills",
                column: "skill_id");

            migrationBuilder.CreateIndex(
                name: "ix_work_experiences_specialist_profile_id",
                table: "work_experiences",
                column: "specialist_profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "educations");

            migrationBuilder.DropTable(
                name: "error_logs");

            migrationBuilder.DropTable(
                name: "interaction_history");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "specialist_skills");

            migrationBuilder.DropTable(
                name: "system_settings");

            migrationBuilder.DropTable(
                name: "vacancy_skills");

            migrationBuilder.DropTable(
                name: "work_experiences");

            migrationBuilder.DropTable(
                name: "recruiter_candidates");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "skills");

            migrationBuilder.DropTable(
                name: "specialist_profiles");

            migrationBuilder.DropTable(
                name: "vacancies");

            migrationBuilder.DropTable(
                name: "skill_categories");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
