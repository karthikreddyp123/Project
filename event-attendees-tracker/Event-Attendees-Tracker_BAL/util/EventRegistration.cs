
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Attendees_Tracker_DAL.DBOperations;
using System.Diagnostics;

namespace Event_Attendees_Tracker_BAL.util
{
    class EventRegistration
    {
        public static int InsertTblRegisteredStudents(DataTable StudentRegistrationData, int EventID, string EventName)
        {
            EventRegistrationDAL eventRegistration = new EventRegistrationDAL();
            int studentsInsertReturnValue = eventRegistration.InsertTblRegisteredStudents(StudentRegistrationData);
            if (studentsInsertReturnValue > 0)
            {
                var AttenddesInsertList = eventRegistration.InsertTblEventAttendees(EventID);
                if (AttenddesInsertList.Count > 0)
                {
                    MailSend.SendRegistrationMail(AttenddesInsertList,EventName);
                    Debug.Print("Succesfully added data into Attendees Table");
                }
                else
                {
                    Debug.Print("Could not insert data into Attendees Table");
                }
            }
            else
            {
                Debug.Print("Could not insert data into Registration Table");
            }
            return studentsInsertReturnValue;
        }
    }
}
