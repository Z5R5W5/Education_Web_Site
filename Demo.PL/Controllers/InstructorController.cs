using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InstructorController(
            IInstructorRepository instructorRepository,
            IDepartmentRepository departmentRepository,
            ICourseRepository courseRepository,
            IWebHostEnvironment webHostEnvironment

            )
        {
            _instructorRepository = instructorRepository;
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var instructors = await _instructorRepository.GetAllAsync();
            return View(instructors);

        }
        public async Task<IActionResult> Details(int id)
        {
            var instructor = await _instructorRepository.GetInstructorAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            InstructorViewModel instructorView = new()
            {
                Id = instructor.Id,
                Name = instructor.Name!,
                Address = instructor.Address,
                PhoneNumber = instructor.PhoneNumber,
                ImageUrl = instructor.ImageUrl,
                crs_id = instructor.crs_id,
                dept_id = instructor.dept_id
            };
            ViewBag.DeptName = await _departmentRepository.GetDepartmentAsync(instructorView.dept_id);
            ViewBag.CourseName = await _courseRepository.GetCourseAsync(instructorView.crs_id);
            return View(instructorView);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await Helper();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(InstructorViewModel instructorVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await Helper();
                    return View();
                }

                instructorVM.ImageUrl = "Images/InstructorPhotos/" + Guid.NewGuid().ToString();
                instructorVM.ImageUrl += instructorVM.File!.FileName;
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, instructorVM.ImageUrl);
                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    instructorVM.File?.CopyTo(fs);
                }
                ;
                var instructor = new Instructor
                {
                    Name = instructorVM.Name,
                    Address = instructorVM.Address,
                    PhoneNumber = instructorVM.PhoneNumber,
                    ImageUrl = instructorVM.ImageUrl,
                    crs_id = instructorVM.crs_id,
                    dept_id = instructorVM.dept_id
                };

                await _instructorRepository.AddInstructorAsync(instructor);
                TempData["successMessageForAddingInstructor"] = "Instructor Added successfully!";
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            string oldFileName = (await _instructorRepository.GetInstructorAsync(id)).ImageUrl!;
            string fullOldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldFileName);
            System.IO.File.Delete(fullOldPath);
            await _instructorRepository.DeleteInstructorAsync(id);
            TempData["successMessageForDeleteInstructor"] = "Instructor deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            await Helper();
            var instructor = await _instructorRepository.GetInstructorAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            InstructorViewModel instructorVM = new()
            {
                Id = instructor.Id,
                Name = instructor.Name!,
                Address = instructor.Address,
                PhoneNumber = instructor.PhoneNumber,
                ImageUrl = instructor.ImageUrl,
                crs_id = instructor.crs_id,
                dept_id = instructor.dept_id
            };
            return View(instructorVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(InstructorViewModel instructorVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await Helper();
                    return View();
                }
                var instructor = await _instructorRepository.GetInstructorAsync(instructorVM.Id);
                if (instructor == null)
                {
                    return NotFound();
                }
                if (instructorVM.File != null)
                {
                    string oldFileName = instructor.ImageUrl!;
                    string fullOldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldFileName);
                    System.IO.File.Delete(fullOldPath);
                    instructorVM.ImageUrl = "Images/InstructorPhotos/" + Guid.NewGuid().ToString();
                    instructorVM.ImageUrl += instructorVM.File!.FileName;
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, instructorVM.ImageUrl);
                    using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                    {
                        instructorVM.File?.CopyTo(fs);
                    }
                }
                else
                {
                    instructorVM.ImageUrl = instructor.ImageUrl;
                }
                instructor.Name = instructorVM.Name;
                instructor.Address = instructorVM.Address;
                instructor.PhoneNumber = instructorVM.PhoneNumber;
                instructor.ImageUrl = instructorVM.ImageUrl;
                instructor.crs_id = instructorVM.crs_id;
                instructor.dept_id = instructorVM.dept_id;
                await _instructorRepository.UpdateInstructorAsync(instructor);
                TempData["successMessageForUpdateInstructor"] = "Instructor updated successfully!";
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Search(string searchString)
        {
            var Search = await _instructorRepository.SearchInstructorAsync(searchString);
            return View(Search);
        }

        [Authorize(Roles = "Admin")]
        private async Task Helper()
        {
            ViewBag.DeptName = await _departmentRepository.GetDepartmentsAsync();
            ViewBag.CourseName = await _courseRepository.GetAllAsync();
        }
    }
}
