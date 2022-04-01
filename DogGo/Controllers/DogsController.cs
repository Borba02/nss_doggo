using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGo.Repositories;
using DogGo.Models;
using System.Collections.Generic;
using DogGo.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;
        private readonly IOwnerRepository _ownerRepository;
        public DogsController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepo = dogRepository;
            _ownerRepository = ownerRepository;
        }
        // GET: DogController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            
            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();
            if (dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // GET: DogController/Create
        [Authorize]
        public ActionResult Create()
        {
            List<Owner> owners = _ownerRepository.GetAllOwners();

            DogFormViewModel model = new DogFormViewModel
            {
                Dog = new Dog(),
                Owners = owners
            };
            return View(model);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                dog.OwnerId = GetCurrentUserId();
                _dogRepo.AddDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            List<Owner> owners = _ownerRepository.GetAllOwners();
            int ownerId = GetCurrentUserId();
            if (dog.OwnerId != ownerId)
            {
                return NotFound();
            }

            DogFormViewModel model = new DogFormViewModel
            {
                Dog = dog,
                Owners = owners
            };
            if (dog == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {

            Dog dog = _dogRepo.GetDogById(id);
            int ownerId = GetCurrentUserId();
            if (dog.OwnerId != ownerId)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
