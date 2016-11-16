using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class PatientsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private PatientService service;

        public PatientsController()
        {
            service = new PatientService(db);
        }

        public DataSourceResult Get([System.Web.Http.ModelBinding.ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request)
        {
            return service.GetAllMyPatients().ToDataSourceResult(request);
        }

        public HttpResponseMessage Post(Patient patient)
        {
            if (ModelState.IsValid)
            {
                service.Create(patient);

                var response = Request.CreateResponse(HttpStatusCode.Created, new DataSourceResult { Data = new[] { patient }, Total = 1 });
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = patient.Id }));
                return response;
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage);

                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }

        public HttpResponseMessage Put(string id, Patient patient)
        {
            if (ModelState.IsValid && id == patient.Id)
            {
                try
                {
                    service.Update(id, patient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }


    }
}