using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TrainerController(
            ITrainerRepository trainerRepository,
            IDepartmentRepository departmentRepository,
            ICourseRepository courseRepository,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _trainerRepository = trainerRepository;
            _departmentRepository = departmentRepository;
            _courseRepository = courseRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var trainers = await _trainerRepository.GetAllAsync();
            return View(trainers);
        }
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _trainerRepository.GetTraineeAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            TrainerViewModel trainerView = new()
            {
                Id = trainer.Id,
                Name = trainer.Name!,
                Address = trainer.Address,
                PhoneNumber = trainer.PhoneNumber,
                Level = trainer.Level,
                ImageUrl = trainer.ImageUrl,
                dept_id = trainer.dept_id

            };
            ViewBag.DeptName = await _departmentRepository.GetDepartmentAsync(trainer.dept_id);
            return View(trainerView);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Add()
        {
            ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]

        public async Task<IActionResult> Add(TrainerViewModel trainerVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
                        return View();
                    }
                    #region  Image 
                    trainerVM.ImageUrl = "Images/TraineePhotos/" + Guid.NewGuid().ToString();
                    trainerVM.ImageUrl += trainerVM.File.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, trainerVM.ImageUrl);
                    //await traineevm.File.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    using (FileStream fs = new FileStream(serverFolder, FileMode.Create))
                    {
                        trainerVM.File?.CopyTo(fs);
                    }
                    ;

                    #endregion
                    var trainee = new Trainee
                    {
                        Name = trainerVM.Name,
                        Address = trainerVM.Address,
                        PhoneNumber = trainerVM.PhoneNumber,
                        Level = trainerVM.Level,
                        ImageUrl = trainerVM.ImageUrl,
                        dept_id = trainerVM.dept_id
                    };
                    await _trainerRepository.AddTraineeAsync(trainee);
                    TempData["successMessageForAddingTrainee"] = "Trainee Added successfully!";
                }
                catch
                {
                    return View();
                }
                
            }
            return RedirectToAction(nameof(Index));
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]

        public async Task<IActionResult> Delete(int id)
        {
            string oldFileName = (await _trainerRepository.GetTraineeAsync(id)).ImageUrl!;
            string fullOldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldFileName);
            System.IO.File.Delete(fullOldPath);
            await _trainerRepository.DeleteTraineeAsync(id);
            TempData["successMessageForDeleteTrainee"] = "Trainee deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]

        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _trainerRepository.GetTraineeAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            TrainerViewModel trainerVM = new()
            {
                Id = trainer.Id,
                Name = trainer.Name!,
                Address = trainer.Address,
                PhoneNumber = trainer.PhoneNumber,
                Level = trainer.Level,
                ImageUrl = trainer.ImageUrl,
                dept_id = trainer.dept_id
            };
            ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
            return View(trainerVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]

        public async Task<IActionResult> Edit(TrainerViewModel traineeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DeptDropDownList = await _departmentRepository.GetDepartmentsAsync();
                return View();
            }
            #region  Image
            var oldUrl = traineeVM.ImageUrl;
            traineeVM.ImageUrl = "Images/TraineePhotos/";
            traineeVM.ImageUrl = traineeVM.File?.FileName;
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, traineeVM.ImageUrl!);
            if (traineeVM.ImageUrl != oldUrl)
            {
                traineeVM.ImageUrl = "Images/TraineePhotos/" + Guid.NewGuid().ToString();
                traineeVM.ImageUrl += traineeVM.File?.FileName;
                string fullPath2 = Path.Combine(_webHostEnvironment.WebRootPath, traineeVM.ImageUrl!);
                string oldFileName = (await _trainerRepository.GetTraineeAsync(traineeVM.Id)).ImageUrl!;
                string fullOldPath = Path.Combine(_webHostEnvironment.WebRootPath, oldFileName);
                if (fullPath2 != fullOldPath)
                {
                    using (FileStream fs = new FileStream(fullPath2, FileMode.Create))
                    {
                        System.IO.File.Delete(fullOldPath);
                        traineeVM.File?.CopyTo(fs);
                    }
                }
            }

            #endregion
            var trainee = new Trainee
            {
                Id = traineeVM.Id,
                Name = traineeVM.Name,
                Address = traineeVM.Address,
                PhoneNumber = traineeVM.PhoneNumber,
                Level = traineeVM.Level,
                ImageUrl = traineeVM.ImageUrl,
                dept_id = traineeVM.dept_id
            };
            await _trainerRepository.UpdateTraineeAsync(trainee);
            TempData["successMessageForEditTrainee"] = "Trainee Updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Instructor")]

        public async Task<IActionResult> Search(string searchString)
        {
            var Search = await _trainerRepository.SearchTraineeAsync(searchString);
            return View(Search);
        }
    }
}
