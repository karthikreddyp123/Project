//System Namespace Import
using System;
using RestSharp;
using System.Diagnostics;
using System.Web.Mvc;

//Custom Namespace Imports
using Event_Attendees_Tracker.Middlewares;
using Event_Attendees_Tracker.Modals;

using Newtonsoft.Json;

namespace Event_Attendees_Tracker.Controllers
{
    public class UserController : Controller
    {
        RestClient client = new RestClient("https://localhost:44360/");

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public RedirectToRouteResult Login(FormCollection formData)
        {
            var request = new RestRequest("api/Login");
            request.Method = Method.POST;
            request.AddJsonBody(new { Email = formData.Get("username"), Password = formData.Get("password") });
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = new JsonDeserializer(response.Content);

                //Remove it
                Debug.Print(content.GetString("RoleName"));
                Debug.Print(content.GetInt("UserID").ToString());

                return RedirectToAction(content.GetString("RoleName"));
            }
            return RedirectToAction("Login");
            //return RedirectToAction("Organizer");

        }

        public ActionResult Index()
        {
            return View("Contact");
        }

        public ActionResult Delete(int id)
        {
            return View("ModifyView");
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return View("Admin");
        }

        public ActionResult Edit()
        {
            ViewBag.Readonly = true;

            return View("CreateEvent");
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            return View("Admin");
        }
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult CreateEvent()
        {
            ViewBag.Readonly = false;
            return View();
        }
        [HttpPost]
        public RedirectToRouteResult CreateEvent(EventModel responseEventModel)
        {
            var excelFilePath = "";
            var imageFilePath = "";

            //Save Excel File
            if (responseEventModel.excelFile.ContentLength > 0 && responseEventModel.excelFile.ContentType.Contains("spreadsheetml"))
            {
                //Excel File

                //Remove it
                Debug.Print(responseEventModel.excelFile.FileName);

                excelFilePath = System.Web.HttpContext.Current.Server.MapPath($@"~/StudentExcel/{DateTime.Now.ToFileTime()}{responseEventModel.excelFile.FileName}");
                responseEventModel.excelFile.SaveAs(excelFilePath);
            }

            //Save Poster Image
            if (responseEventModel.posterImage.ContentLength > 0 && responseEventModel.posterImage.ContentType.Contains("image"))
            {
                //Poster Image File

                //Remove it
                Debug.Print(responseEventModel.posterImage.FileName);

                imageFilePath = System.Web.HttpContext.Current.Server.MapPath($@"~/PosterImage/{DateTime.Now.ToFileTime()}{responseEventModel.posterImage.FileName}");
                responseEventModel.posterImage.SaveAs(imageFilePath);
            }

            //Get the Datatable After Parsing
            var parsedDataTable = new ParseExcel().InsertTblRegisteredStudents(excelFilePath);


            //To Delete the file
            if (System.IO.File.Exists(excelFilePath))
            {
                System.IO.File.Delete(excelFilePath);
            }


            //Request Config
            var request = new RestRequest("api/User/CreateEvent");
            request.Method = Method.POST;

            //Adding JSON Body

            //TODO:
            //Add Volunteer Reference

            var requestedData = new
            {
                Name = responseEventModel.name,
                Venue = responseEventModel.venue,
                Description = responseEventModel.description,
                EventDate = Convert.ToDateTime(responseEventModel.eventDate),
                StartTime = responseEventModel.startTime,
                EndTime = responseEventModel.endTime,
                PosterImagePath = imageFilePath,
                AttendeesDataTable = parsedDataTable
            };

            request.AddJsonBody(JsonConvert.SerializeObject(requestedData, Formatting.Indented));

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Debug.Print(response.Content);

            }
            return RedirectToAction("CreateEvent");

        }
        public ActionResult CreateOrganizer()
        {
            return View();
        }
        public ActionResult ModifyEvent()
        {
            return View();
        }
        public ActionResult Volunteer()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Organizer()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult dashboard()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}