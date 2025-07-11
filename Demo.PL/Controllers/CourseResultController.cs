using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    
    public class CourseResultController : Controller
    {
        private readonly ICourseResultRepository _courseResultRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ITrainerRepository _traineeRepository;

        public CourseResultController(
            ICourseResultRepository courseResultRepository,
            ICourseRepository courseRepository,
            ITrainerRepository traineeRepository
            )
        {
            _courseResultRepository = courseResultRepository;
            _courseRepository = courseRepository;
            _traineeRepository = traineeRepository;
        }
        [HttpGet]

        public async Task<IActionResult> GetCoursesForTrainee(int traineeId)
        {
            var trainee = await _traineeRepository.GetTraineeWithDepartmentAsync(traineeId); 

            if (trainee == null || trainee.dept_id == null)
                return Json(new List<object>());

            var courses = await _courseRepository.GetCoursesByDepartmentIdAsync(trainee.dept_id.Value);

            var result = courses.Select(c => new { id = c.Id, name = c.Name }).ToList();

            return Json(result);
        }

        public async Task<IActionResult> Index()
        {
            var itemsList = await _courseResultRepository.GetAllAsync();
            return View(itemsList);
        }
        public async Task<IActionResult> Results(int id)
        {
            var itemsList = await _courseResultRepository.GetCourseResultAsync(id);
            ViewBag.TraineeName = await _traineeRepository.GetAllAsync();
            ViewBag.CourseName = await _courseRepository.GetAllAsync();
            return View(itemsList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add()
        {
            await Helper();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add(CourseResultViewModel CourseResultVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await Helper();
                    return View();
                }
                var courseResult = new CourseResult
                {
                    Degree = CourseResultVM.Degree,
                    crs_id = CourseResultVM.crs_id,
                    trainee_id = CourseResultVM.trainee_id
                 
                };
                await _courseResultRepository.AddCourseResultAsync(courseResult);
                TempData["successMessageForAddingResult"] = "Result Added successfully!";
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            await _courseResultRepository.DeleteCourseResultAsync(id);
            TempData["successMessageForDeleteResult"] = "Result deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var courseResult = await _courseResultRepository.GetCourseResultAsync(id);
            if (courseResult == null)
            {
                return NotFound();
            }
            var courseResultVM = new CourseResultViewModel
            {
                Id = courseResult.Id,
                Degree = courseResult.Degree,
                crs_id = courseResult.crs_id,
                trainee_id = courseResult.trainee_id
            };
            await Helper();
            return View(courseResultVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(CourseResultViewModel courseResultVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await Helper();
                    return View();
                }
                var courseResult = new CourseResult
                {
                    Id = courseResultVM.Id,
                    Degree = courseResultVM.Degree,
                    crs_id = courseResultVM.crs_id,
                    trainee_id = courseResultVM.trainee_id
                };
                await _courseResultRepository.UpdateCourseResultAsync(courseResult);
                TempData["successMessageForUpdateResult"] = "Result updated successfully!";
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
            var searchResults = await _courseResultRepository.SearchCourseResultAsync(searchString);
            return View(searchResults);
        }


        private async Task Helper()
        {
            ViewBag.TraineeName = await _traineeRepository.GetAllAsync();
            ViewBag.CourseName = await _courseRepository.GetAllAsync();
        }
    }
}
