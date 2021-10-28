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
        public TreatmentService(IRepository<Treatment> repository) : base(repository)
        {
        }

        public new async Task<Treatment> Add(Treatment model)
        {
            if (model.TreatmentCode == null && !model.TreatmentCode.ExplanationRequired || model.Description.Count() == 0)
            {
                throw new ValidationException("Voor dit type behandeling is een beschrijving verplicht");
            }

            if (DateTime.Now > model.CreatedAt.AddDays(1))
            {
                
            }
            return await _repository.Add(model);

        }
        
        public new async Task<Treatment> Update(Treatment model)
        {
            if (model.TreatmentCode == null && !model.TreatmentCode.ExplanationRequired || model.Description.Count() == 0)
            {
                throw new ValidationException("Voor dit type behandeling is een beschrijving verplicht");
            }

            if (DateTime.Now > model.CreatedAt.AddDays(1))
            {
                throw new ValidationException("een behandeling kan alleen aangepast worden op de dag dat die gemaakt is.");
            }
            return await _repository.Add(model);

        }

        public new async Task Delete(int id)
        {
            Treatment model = await _repository.Get(id);

            await this.Delete(model);
        }

        public new Task Delete(Treatment model)
        {
            if (DateTime.Now > model.CreatedAt.AddDays(1))
            {
                throw new ValidationException("een behandeling kan alleen aangepast worden op de dag dat die gemaakt is.");
            }
            return  _repository.Delete(model);
        }
    }
}