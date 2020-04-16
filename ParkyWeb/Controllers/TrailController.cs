﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWeb.Controllers
{
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailController(INationalParkRepository nationalParkRepository, ITrailRepository trailRepository)
        {
            _nationalParkRepository = nationalParkRepository;
            _trailRepository = trailRepository;
        }

        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> UpSert(int? id)
        {
            IEnumerable<NationalPark> nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = nationalParks.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                //this will be true for Insert/Create
                return View(objVM);
            }

            //Flow will come here for update
            objVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (objVM.Trail == null)
            {
                return NotFound();
            }
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if(ModelState.IsValid)
            {
                if(obj.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(SD.TrailAPIPath, obj.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(SD.TrailAPIPath + obj.Trail.Id, obj.Trail);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> nationalParks = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);

                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = nationalParks.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };

                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message="Delete Successful"});
            }

            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}