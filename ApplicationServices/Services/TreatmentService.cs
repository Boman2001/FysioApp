using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.ExtensionMethods;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class TreatmentService : Service<Treatment> , IService<Treatment>
    {
        private readonly IService<Dossier> _dossierService;
        private readonly IRepository<TreatmentPlan> _treatmentPlanRepository;

        public TreatmentService(IRepository<Treatment> repository, IService<Dossier> dossierService, IRepository<TreatmentPlan> treatmentPlanRepository) : base(repository)
        {
            _dossierService = dossierService;
            _treatmentPlanRepository = treatmentPlanRepository;
        }

        public new async Task<Treatment> Add(Treatment model)
        {
          var Dossier = await _dossierService.Get(model.Dossier.Id);
          var TreatmentPlan = await _treatmentPlanRepository.Get(model.Dossier.TreatmentPlan.Id);
            model.TreatmentEndDate =
                model.TreatmentDate.AddMinutes(model.Dossier.TreatmentPlan.TimePerSessionInMinutes);
            if (TreatmentPlan.TreatmentsPerWeek <= Dossier.Treatments
                .Where(t => AreFallingInSameWeek(t.TreatmentDate, model.TreatmentDate)).ToList().Count)
            {
                throw new ValidationException("Het maximum aantal afspraken zijn al aangemaakt voor deze week");
            }
            
            if (model.TreatmentCode == null && !model.TreatmentCode.ExplanationRequired || model.Description.Count() == 0)
            {
                throw new ValidationException("Voor dit type behandeling is een beschrijving verplicht");
            }

            if (!(model.ExcecutedBy.InBuisinessHours(model.TreatmentDate) && model.ExcecutedBy.InBuisinessHours(model.TreatmentEndDate)))
            {
                throw new ValidationException("Deze behandelinmg valt buiten de werktijden van uw doctor");
            }
            
            if (!model.ExcecutedBy.IsAvailable(model.TreatmentDate, model.TreatmentEndDate))
            {
                throw new ValidationException("Uw doctor is al bezet op dit moment");
            }

            if (model.TreatmentDate.Date.Ticks <= model.Dossier.RegistrationDate.Ticks && model.TreatmentEndDate.Ticks >= model.Dossier.DismissionDate.Ticks)
            {
                throw new ValidationException("een behandeling kan niet geplanned worden buiten een behandel periode");
            }
            

            return await _repository.Add(model);

        }
        private bool AreFallingInSameWeek(DateTime date1, DateTime date2)
        {
            return date1.AddDays(-(int)date1.DayOfWeek) == date2.AddDays(-(int)date2.DayOfWeek);
        }


    }
}