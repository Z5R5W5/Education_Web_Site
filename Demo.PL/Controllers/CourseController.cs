using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public CourseController(
            ICourseRepository courseRepository,
            IDepartmentRepository departmentRepository
            )
        {
            _courseRepository = courseRepository;
            _departmentRepository = departmentRepository;
        }

        public IActionResult CheckGradeAsync(int MinDegree, int Grade)
        {
            if (MinDegree <= Grade)
            {
                return Json(true);
            }
            return Json(false);
        }
        public async Task<IActionResult> GetCoursePerDepartment(int deptID)
        {
            var courses = await _courseRepository.GetCoursePerDepartmentAsync(deptID);
            return Json(courses);
        }
        public async Task<IActionResult> Index()
        {
            var GetAllCourses = await _courseRepository.GetAllAsync();
            return View(GetAllCourses);
        }
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepository.GetCourseAsync(id);
            CourseViewModel courseVM = new()
            {
                Id = course.Id,
                Name = course.Name!,
                Grade = course.Grade,
                MinDegree = course.MinDegree,
                dept_id = course.dept_id
            };
            ViewBag.DeptName = await _departmentRepository.GetDepartmentAsync(course.dept_id);
            return View(courseVM);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(CourseViewModel courseVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
                    return View();
                }
                var course = new Course
                {
                    Id = courseVM.Id,
                    Name = courseVM.Name,
                    Grade = courseVM.Grade,
                    MinDegree = courseVM.MinDegree,
                    dept_id = courseVM.dept_id
                };

                await _courseRepository.AddCourseAsync(course);
                TempData["successMessageForAddingCourse"] = "Course Added successfully!";
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
            await _courseRepository.DeleteCourseAsync(id);
            TempData["successMessageForDeleteCourse"] = "Course deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepository.GetCourseAsync(id);
            ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
            CourseViewModel courseVM = new()
            {
                Name = course.Name!,
                Grade = course.Grade,
                MinDegree = course.MinDegree,
                dept_id = course.dept_id,
            };
            return View(courseVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CourseViewModel courseVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
                    return View();
                }
                var course = new Course
                {
                    Id = id,
                    Name = courseVM.Name,
                    Grade = courseVM.Grade,
                    MinDegree = courseVM.MinDegree,
                    dept_id = courseVM.dept_id
                };
                await _courseRepository.UpdateCourseAsync(course);
                TempData["successMessageForUpdateCourse"] = "Course updated successfully!";
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var Search = await _courseRepository.SearchCourseAsync(searchString);
            ViewBag.Search = searchString;
            return View(Search);
        }

    }
}
