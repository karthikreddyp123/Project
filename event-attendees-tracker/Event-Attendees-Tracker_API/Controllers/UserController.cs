//System namespace Imports
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Diagnostics;
using System;
using System.Collections.Generic;

//Custom Namespace Imports
using Event_Attendees_Tracker_API.Models;
using Event_Attendees_Tracker_BAL.User_Actions;


namespace Event_Attendees_Tracker_API.Controllers
{

    public class UserController : ApiController
    {
        public HttpResponseMessage CreateEvent(EventModel requestEventData)
        {
            try
            {
                var response = Events.AddEvent(requestEventData.Name, requestEventData.Description, requestEventData.Venue, requestEventData.PosterImagePath, requestEventData.StartTime, requestEventData.EndTime, requestEventData.EventDate, requestEventData.AttendeesDataTable);
                if (!response) return Request.CreateResponse(HttpStatusCode.BadRequest, "Check the Data");
            }


            catch(Exception ex)
            {
                Debug.Print(ex.Message);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            

            return Request.CreateResponse(HttpStatusCode.Created, new { Message = "Successfully Created!"});           
        }
    }
}
