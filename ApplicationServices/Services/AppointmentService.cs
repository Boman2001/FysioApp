﻿using System;
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

        public AppointmentService(IRepository<Appointment> repository, IService<Dossier> dossierService,
            IRepository<TreatmentPlan> treatmentPlanRepository) : base(repository)
        {
            _dossierService = dossierService;
            _treatmentPlanRepository = treatmentPlanRepository;
        }

        public new async Task<Appointment> Add(Appointment model)
        {
            var dossier = await _dossierService.Get(model.Dossier.Id);
            var treatmentPlan = await _treatmentPlanRepository.Get(model.Dossier.TreatmentPlan.Id);
            model.TreatmentEndDate =
                model.TreatmentDate.AddMinutes(model.Dossier.TreatmentPlan.TimePerSessionInMinutes);
            if (treatmentPlan.TreatmentsPerWeek <= dossier.Treatments
                .Where(t => AreFallingInSameWeek(t.TreatmentDate, model.TreatmentDate)).ToList().Count)
            {
                throw new ValidationException("Het maximum aantal afspraken zijn al aangemaakt voor deze week");
            }

            if (!(model.ExcecutedBy.InBuisinessHours(model.TreatmentDate) &&
                  model.ExcecutedBy.InBuisinessHours(model.TreatmentEndDate)))
            {
                throw new ValidationException("Deze behandelinmg valt buiten de werktijden van uw doctor");
            }

            if (!model.ExcecutedBy.IsAvailable(model.TreatmentDate, model.TreatmentEndDate))
            {
                throw new ValidationException("Uw doctor is al bezet op dit moment");
            }

            if (model.TreatmentDate.Ticks <= model.Dossier.RegistrationDate.Ticks ||
                model.TreatmentEndDate.Ticks >= model.Dossier.DismissionDate.Ticks)
            {
                throw new ValidationException("een behandeling kan niet geplanned worden buiten een behandel periode");
            }

            if (model.TreatmentDate.Ticks < DateTime.Now.Ticks)
            {
                throw new ValidationException("een behandeling kan alleen in de toekomst geplanned worden");
            }

            return await _repository.Add(model);
        }

        public new async Task<Appointment> Update(Appointment model)
        {
            var dossier = await _dossierService.Get(model.Dossier.Id);
            var treatmentPlan = await _treatmentPlanRepository.Get(model.Dossier.TreatmentPlan.Id);
            model.TreatmentEndDate =
                model.TreatmentDate.AddMinutes(model.Dossier.TreatmentPlan.TimePerSessionInMinutes);
            if (treatmentPlan.TreatmentsPerWeek <= dossier.Treatments
                .Where(t => AreFallingInSameWeek(t.TreatmentDate, model.TreatmentDate)).ToList().Count)
            {
                throw new ValidationException("Het maximum aantal afspraken zijn al aangemaakt voor deze week");
            }

            if (!(model.ExcecutedBy.InBuisinessHours(model.TreatmentDate) &&
                  model.ExcecutedBy.InBuisinessHours(model.TreatmentEndDate)))
            {
                throw new ValidationException("Deze behandelinmg valt buiten de werktijden van uw doctor");
            }

            if (!model.ExcecutedBy.IsAvailable(model.TreatmentDate, model.TreatmentEndDate))
            {
                throw new ValidationException("Uw doctor is al bezet op dit moment");
            }

            if (model.TreatmentDate.Ticks <= model.Dossier.RegistrationDate.Ticks ||
                model.TreatmentEndDate.Ticks >= model.Dossier.DismissionDate.Ticks)
            {
                throw new ValidationException("een behandeling kan niet geplanned worden buiten een behandel periode");
            }

            if (model.TreatmentDate.Ticks < DateTime.Now.Ticks)
            {
                throw new ValidationException("een behandeling kan alleen in de toekomst geplanned worden");
            }
            return await _repository.Update(model);
        }
        public new Task Delete(Appointment model)
        {
            if (DateTime.Now > model.TreatmentDate.AddHours(-24))
            {
                throw new ValidationException(
                    "een behandeling kan niet verwijderd worden binnen 24 uur van het begin van de afspraak");
            }

            return _repository.Delete(model);
        }
        

        private bool AreFallingInSameWeek(DateTime date1, DateTime date2)
        {
            return date1.AddDays(-(int) date1.DayOfWeek) == date2.AddDays(-(int) date2.DayOfWeek);
        }
    }
}