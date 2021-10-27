using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Infrastructure.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTested.AspNetCore.Mvc.Utilities.Extensions;
using WebApp.Dtos.TreatmentPlan;
using WebApp.helpers;

namespace WebApp.Controllers
{
    public class TreatmentPlanController : Controller
    {
        private readonly IWebRepository<TreatmentCode> _treatmentCodeRepository;
        private readonly IRepository<TreatmentPlan> _treatmentPlanRepository;
        private readonly IService<Dossier> _dossierService;

        public TreatmentPlanController(IWebRepository<TreatmentCode> treatmentCodeRepository,
            IRepository<TreatmentPlan> treatmentPlanRepository, IService<Dossier> dossierService)
        {
            _treatmentCodeRepository = treatmentCodeRepository;
            _treatmentPlanRepository = treatmentPlanRepository;
            _dossierService = dossierService;
        }

        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            CreateTreatmentPlanDto viewModel = new CreateTreatmentPlanDto();

            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create(CreateTreatmentPlanDto treatmentPlanDto)
        {
            if (ModelState.IsValid)
            {
                Dossier dossier = this.TempData.Get<Dossier>("dossier");
                TreatmentPlan treatmentplan = await _treatmentPlanRepository.Add(new TreatmentPlan()
                {
                    TreatmentsPerWeek = treatmentPlanDto.TreatmentsPerWeek,
                    TimePerSessionInMinutes = treatmentPlanDto.TimePerSessionInMinutes
                });
                dossier.TreatmentPlan = treatmentplan;

                await _dossierService.Add(dossier);

                TempData["SuccessMessage"] = "Success";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.error = "Something went wrong, please try again later";
            return PartialView("_Create", treatmentPlanDto);
        }
    }
}