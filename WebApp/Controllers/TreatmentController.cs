using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.Dossier;
using WebApp.Dtos.Treatment;
using WebApp.Dtos.TreatmentPlan;

namespace WebApp.Controllers
{
    public class TreatmentController : Controller
    {
        private IWebRepository<TreatmentCode> _treatmentCodeRepository;
        private IService<Treatment> _treatmentService;
        private IService<TreatmentPlan> _treatmentPlanService;

        public TreatmentController(IWebRepository<TreatmentCode> treatmentCodeRepository, IService<Treatment> treatmentService, IService<TreatmentPlan> treatmentPlanService)
        {
            _treatmentCodeRepository = treatmentCodeRepository;
            _treatmentService = treatmentService;
            _treatmentPlanService = treatmentPlanService;
        }

        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create(CreateDossierDto dossierDto)
        {
            CreateTreatmentDto viewModel = new CreateTreatmentDto();
            viewModel.DossierDto = dossierDto;
            TreatmentPlan treatmentPlan = await _treatmentPlanService.Get(dossierDto.TreatmentPlanId);
            TreatmentCode treatment = await  _treatmentCodeRepository.Get(treatmentPlan.TreatmentCodeId);
            viewModel.TreatmentCode = treatment;
            return View("Create", viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create(CreateTreatmentDto treatmentDto)
        {
            if (ModelState.IsValid)
            {
                TreatmentCode treatment = await  _treatmentCodeRepository.Get(treatmentDto.TreatmentCodeId);
                await _treatmentService.Add(new Treatment()
                {
                    TreatmentDate = treatmentDto.TreatmentDate,
                    TreatmentCodeId = treatmentDto.TreatmentCodeId,
                    Description = treatmentDto.Description,
                    TreatmentCode = treatment,
                    Room = treatmentDto.Room
                });

                TempData["SuccessMessage"] = "Success";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.error = "Something went wrong, please try again later";
            return PartialView("_Form", treatmentDto);
        }
    }
}