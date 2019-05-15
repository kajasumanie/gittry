using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;
using cosmossetry.Models;
using Microsoft.AspNetCore.Mvc;

namespace cosmossetry.Controllers
{
    public class HomeController : Controller
    {

        string EndpointUrl;
        private string PrimaryKey;
        private DocumentClient client;
        public HomeController()
        {
            EndpointUrl = "https://sbeehlab.documents.azure.com:443/";
            PrimaryKey = "F4tQtOfR8AWs6oy7E4ZjvQ3mr3R3OBxjqbL4EWQzoZQgKrZbyj1wT6ffdk6UjIjCQCwVRjp7vJcVQvwh8oVSJg==";


            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);



        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public async Task<ActionResult> Employees()
        {
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "HRDB" });

            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("HRDB"),
                new DocumentCollection { Id = "EmployeesCollection" });


            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };


            IQueryable<Employee> employeeQuery = this.client.CreateDocumentQuery<Employee>(
                    UriFactory.CreateDocumentCollectionUri("HRDB", "EmployeesCollection"), queryOptions)
                    .Where(f => f.Salary >= 100);

            return View(employeeQuery);
        }

        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee(Employee employee)


       {

            await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("HRDB", "EmployeesCollection"), employee);

            return RedirectToAction("Employees");
        }

        public async Task<ActionResult> DeleteEmployee(string documentId)
        {
            await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri("HRDB", "EmployeesCollection", documentId));
            return RedirectToAction("Employees");
        }
    }
}