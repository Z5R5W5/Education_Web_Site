using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ITrainerRepository _traineeRepository;

        public DepartmentController(
            IDepartmentRepository departmentRepository,
            ITrainerRepository traineeRepository
            )
        {
            _departmentRepository = departmentRepository;
            _traineeRepository = traineeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _departmentRepository.GetDepartmentsAsync();
            return View(departments);
        }
        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentRepository.GetDepartmentAsync(id);
            DepartmentViewModel DepartmentVM = new()
            {
                Id = department.Id,
                Name = department.Name,
                DeptManager = department.DeptManager
            };
            return View(DepartmentVM);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(DepartmentViewModel departmentVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var department = new Department
                {
                    Id = departmentVM.Id,
                    Name = departmentVM.Name,
                    DeptManager = departmentVM.DeptManager
                };
                await _departmentRepository.AddDepartmentAsync(department);
                TempData["successMessageForAddingDepartment"] = "Department Added successfully!";
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
            await _departmentRepository.DeleteDepartmentAsync(id);
            TempData["successMessageForDeleteDepartment"] = "Department deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentRepository.GetDepartmentAsync(id);
            DepartmentViewModel DepartmentVM = new()
            {
                Id = department.Id,
                Name = department.Name,
                DeptManager = department.DeptManager
            };
            return View(DepartmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var department = new Department
                {
                    Id = departmentVM.Id,
                    Name = departmentVM.Name,
                    DeptManager = departmentVM.DeptManager
                };
                await _departmentRepository.UpdateDepartmentAsync(department);
                TempData["successMessageForUpdateDepartment"] = "Department updated successfully!";
            }
            catch
            {
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Search(string searchString)
        {
            var Search = await _departmentRepository.SearchDepartmentAsync(searchString);
            return View(Search);
        }



    }
}
