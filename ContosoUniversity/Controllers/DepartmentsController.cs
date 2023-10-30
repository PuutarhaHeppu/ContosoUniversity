using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;
        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }
        //index get

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Administrator)
                .ToListAsync();
            return View(departments);
        }

        //details get

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Department WHERE departmentID ={0}";
            var departments = await _context.Departments
                .FromSqlRaw(query, id)
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (departments == null)
            {
                return NotFound();
            }

            return View(departments);
        }
        //Create Get
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }
        //create post
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> Create([Bind("Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            ModelState.Remove("Courses");
            ModelState.Remove("Adminstrator");
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "Fullname", department.InstructorID);
            return RedirectToAction(nameof(Index));
        }
        // Edit Get
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }
        //edit post 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            ModelState.Remove("Courses");
            ModelState.Remove("Administrators");
            ModelState.Remove("RowVersion");
            if (id == null)
            {
                return NotFound();
            }
            var departmentToUpdate = await _context.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (departmentToUpdate == null)
            {
                Department deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty, "Unable to save changes, The department has been deleted by another user.");
                ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", deletedDepartment.InstructorID);
                return View(deletedDepartment);
            }
            _context.Entry(departmentToUpdate)
                .Property("RowVersion")
                .OriginalValue = rowVersion;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate, "",
                s => s.Name,
                s => s.StartDate,
                s => s.Budget,
                s => s.InstructorID
                ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes, The department has been deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();
                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"current value: {databaseValues.Name}");
                        }
                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"current value: {databaseValues.Budget}");
                        }
                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", $"current value: {databaseValues.StartDate}");
                        }
                        if (databaseValues.InstructorID != clientValues.InstructorID)
                        {
                            ModelState.AddModelError("InstructorID", $"current value: {databaseValues.InstructorID}");
                        }
                        ModelState.AddModelError(string.Empty, "The record that you have attempted to edit"
                            + "was modified by another user after you got the original value."
                            + "The editing operation was cancelled and the current values in the database"
                            + "Have been displayed. If you still require to eidt this record. click"
                            + "the save button again. otherwise click the back to list hyperlink");
                        departmentToUpdate.RowVersion = databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);
            return View(departmentToUpdate);
        }
        //Delete Get
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record that you have attempted to edit"
                + "was modified by another user after you got the original value."
                + "The editing operation was cancelled and the current values in the database"
                + "Have been displayed. If you still require to eidt this record. click"
                + "the save button again. otherwise click the back to list hyperlink";
            }
            return View(department);
        }
        //delete post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {
                if (await _context.Departments.AnyAsync(m => m.DepartmentID == department.DepartmentID))
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction(nameof(Delete),
                    new { concurrencyError = true, id = department.DepartmentID });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}