

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Core.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewComponents
{
    [ViewComponent(Name = "Counter")]
    public class CounterComponenet : ViewComponent{
        private readonly IAppointmentRepository _repository;
        private readonly IRepository<Treatment> _Trepository;

        public CounterComponenet(IAppointmentRepository repository, IRepository<Treatment> trepository)
        {
            _repository = repository;
            _Trepository = trepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = await _repository.Get(0);
            var items = _repository.Get(appointment => appointment.TreatmentDate.Date == DateTime.Today);
            var items1 = _Trepository.Get(appointment => appointment.TreatmentDate.Date == DateTime.Today);
            return View(items.Count() + items1.Count());
        }
    }
}