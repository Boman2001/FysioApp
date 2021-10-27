using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationServices.ExtensionMethods;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class AppointmentService : Service<Appointment>, IService<Appointment>
    {

        private readonly IService<Dossier> _dossierService;
        private readonly IRepository<TreatmentPlan> _treatmentPlanRepository;
        
        public AppointmentService(IRepository<Appointment> repository, IService<Dossier> dossierService, IRepository<TreatmentPlan> treatmentPlanRepository) : base(repository)
        {
            _dossierService = dossierService;
            _treatmentPlanRepository = treatmentPlanRepository;
        }

        public new async Task<Appointment> Add(Appointment model)
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
            
            if (inBuisinessHours(model.TreatmentDate) && inBuisinessHours(model.TreatmentEndDate))
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

        private bool inBuisinessHours(DateTime toCheck)
        {
            TimeSpan start = TimeSpan.Parse("9:00"); 
            TimeSpan end = TimeSpan.Parse("17:00");
            TimeSpan CheckTimespan =  toCheck.TimeOfDay;

            return (CheckTimespan < end && CheckTimespan > start && toCheck.DayOfWeek != DayOfWeek.Saturday &&
                    toCheck.DayOfWeek != DayOfWeek.Sunday);
        }
    }
}