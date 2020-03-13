using System;
using System.Data;

//Custom namespace imports
using Event_Attendees_Tracker_BAL.Models.ResponseModels;
using Event_Attendees_Tracker_BAL.util;
using Event_Attendees_Tracker_DAL.DBQueries;

namespace Event_Attendees_Tracker_BAL.User_Actions
{
   public class Events
    {
        public static bool AddEvent(string EventName, string Description, string Venue, string posterImagePath, TimeSpan startTime, TimeSpan endTime, DateTime eventDate, DataTable StudentRegistrationData)
        {
            try
            {
                var responseAddEventData = EventQuery.AddEvent(EventName, Description, Venue, posterImagePath, startTime, endTime, eventDate);

                //Save the attendees data
                //Fetch Event ID and Name
                var responseAddStudentRegistrationData = EventRegistration.InsertTblRegisteredStudents(StudentRegistrationData, 2, "CodeInject");
                return responseAddEventData;
            }
            catch(Exception ex)
            {
                return false;
            }
                
        }
    }
}
