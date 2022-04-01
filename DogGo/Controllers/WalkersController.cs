using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogGo.Repositories;
using System.Collections.Generic;
using DogGo.Models;
using DogGo.Interfaces;
using DogGo.Models.ViewModels;
using System;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalksRepository _walksRepository;
        private readonly INeighborhoodRepository _neighborhoodRepository;
        private readonly IOwnerRepository _ownerRepository;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalksRepository walksRepository, INeighborhoodRepository neighborhoodRepository, IOwnerRepository ownerRepository)
        {
            _walkerRepo = walkerRepository;
            _walksRepository = walksRepository;
            _neighborhoodRepository = neighborhoodRepository;
            _ownerRepository = ownerRepository;
        }
        // GET: Walkers
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            Owner owner = _ownerRepository.GetOwnerById(ownerId);
            if (ownerId == 0)
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                return View(walkers);

            } else
            {
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);
                return View(walkers);
            }

        }

        private int GetCurrentUserId()
        {
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (id == null)
                { id = "0"; }
                return int.Parse(id);
            }
        }

        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walks = _walksRepository.GetWalksByWalkerId(walker.Id);

            if (walker == null)
            {
                return NotFound();
            }
            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Walker = walker,
                Walks = walks
            };

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetAll();

            WalkerFormViewModel vm = new WalkerFormViewModel()
            {
                Walker = new Walker(),
                Neighborhoods = neighborhoods
            };
            return View(vm);
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walker walker)
        {
            try
            {
                _walkerRepo.AddWalker(walker);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetAll();
            if (walker == null)
            { return NotFound(); }
            WalkerFormViewModel vm = new WalkerFormViewModel()
            {
                Walker = walker,
                Neighborhoods = neighborhoods
            };
            return View(vm);
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walker walker)
        {
            try
            {
                _walkerRepo.UpdateWalker(walker);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            Neighborhood neighborhood = _neighborhoodRepository.GetNeighborhoodById(walker.NeighborhoodId);
            WalkerDeleteViewModel vm = new WalkerDeleteViewModel()
            {
                Neighborhood = neighborhood,
                Walker = walker
            };
            return View(vm);
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walker walker)
        {
            try
            {
                _walkerRepo.DeleteWalker(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }
    }
}
