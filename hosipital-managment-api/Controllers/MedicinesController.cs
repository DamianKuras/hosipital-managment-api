using hosipital_managment_api.Data;
using hosipital_managment_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hosipital_managment_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public MedicinesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ret = await _dbContext.Medicines.ToListAsync();
            return new JsonResult(ret);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Medicine medicine)
        {
            _dbContext.Medicines.Add(medicine);
            await _dbContext.SaveChangesAsync();

            return new JsonResult(medicine.Id);
        }
    }
}
