using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using scoutly.Data.Context;
using scoutly.Data.Entities;
using scoutly.Entities;

namespace scoutly.Features.Vacancies
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacanciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VacanciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //READ ALL: GET /api/vacancies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VacancyResponseDto>>> GetVacancies([FromQuery] string? city, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Vacancies
                .Include(v => v.Company)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(v => v.City != null && v.City.ToLower().Contains(city.ToLower()));
            }

            var vacancies = await query
                .OrderByDescending(v => v.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VacancyResponseDto
                {
                    Id = v.Id,
                    CompanyId = v.CompanyId,
                    CompanyName = v.Company.Name,
                    Title = v.Title,
                    Description = v.Description,
                    City = v.City,
                    WorkFormat = v.WorkFormat,
                    SalaryMin = v.SalaryMin,
                    SalaryMax = v.SalaryMax,
                    Status = v.Status,
                    CreatedAt = v.CreatedAt
                })
                .ToListAsync();

            return Ok(vacancies);
        }

        //READ ONE: GET /api/vacancies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<VacancyResponseDto>> GetVacancy(int id)
        {
            var vacancy = await _context.Vacancies
                .Include(v => v.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vacancy == null)
            {
                return NotFound(new { message = $"Вакансия с ID {id} не найдена" });
            }

            return Ok(new VacancyResponseDto
            {
                Id = vacancy.Id,
                CompanyId = vacancy.CompanyId,
                CompanyName = vacancy.Company.Name,
                Title = vacancy.Title,
                Description = vacancy.Description,
                City = vacancy.City,
                WorkFormat = vacancy.WorkFormat,
                SalaryMin = vacancy.SalaryMin,
                SalaryMax = vacancy.SalaryMax,
                Status = vacancy.Status,
                CreatedAt = vacancy.CreatedAt
            });
        }

        //CREATE: POST /api/vacancies
        [HttpPost]
        public async Task<ActionResult<VacancyResponseDto>> CreateVacancy([FromBody] CreateVacancyDto dto)
        {
            var companyExists = await _context.Companies.AnyAsync(c => c.Id == dto.CompanyId);
            if (!companyExists)
            {
                return BadRequest(new { message = "Указанная компания не существует" });
            }

            var vacancy = new Vacancy
            {
                CompanyId = dto.CompanyId,
                Title = dto.Title,
                Description = dto.Description,
                City = dto.City,
                WorkFormat = dto.WorkFormat,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                Status = "Published",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            foreach (var skillId in dto.SkillIds)
            {
                vacancy.VacancySkills.Add(new VacancySkill
                {
                    SkillId = skillId,
                    IsRequired = true,
                    Weight = 1.0m
                });
            }

            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVacancy), new { id = vacancy.Id }, new VacancyResponseDto
            {
                Id = vacancy.Id,
                CompanyId = vacancy.CompanyId,
                CompanyName = "",
                Title = vacancy.Title,
                Description = vacancy.Description,
                City = vacancy.City,
                WorkFormat = vacancy.WorkFormat,
                SalaryMin = vacancy.SalaryMin,
                SalaryMax = vacancy.SalaryMax,
                Status = vacancy.Status,
                CreatedAt = vacancy.CreatedAt
            });
        }

        //UPDATE: PUT /api/vacancies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVacancy(int id, [FromBody] UpdateVacancyDto dto)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound(new { message = $"Вакансия с ID {id} не найдена" });
            }

            vacancy.Title = dto.Title;
            vacancy.Description = dto.Description;
            vacancy.City = dto.City;
            vacancy.WorkFormat = dto.WorkFormat;
            vacancy.SalaryMin = dto.SalaryMin;
            vacancy.SalaryMax = dto.SalaryMax;
            vacancy.Status = dto.Status;
            vacancy.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //DELETE: DELETE /api/vacancies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVacancy(int id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound(new { message = $"Вакансия с ID {id} не найдена" });
            }

            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}