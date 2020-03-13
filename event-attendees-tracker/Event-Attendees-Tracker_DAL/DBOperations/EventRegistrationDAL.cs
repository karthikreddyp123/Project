using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Event_Attendees_Tracker_DAL.Instances;
using Event_Attendees_Tracker_DAL.Models;
using System.Diagnostics;

namespace Event_Attendees_Tracker_DAL.DBOperations
{
    public class EventRegistrationDAL
    {
        static Event_Attendees_Tracker_DAL.Database_Context.EAT_DBContext _eatDBContext = DBInstance.getDBInstance();
        public int InsertTblRegisteredStudents(DataTable StudentsData)
        {
            try
            {
                foreach (DataRow student in StudentsData.Rows)
                {
                    String CollegeName = student.ItemArray[5].ToString();
                    _eatDBContext.RegisteredStudents.Add(new RegisteredStudents
                    {
                        FirstName = student.ItemArray[0].ToString(),
                        LastName = student.ItemArray[1].ToString(),
                        ContactNumber = student.ItemArray[2].ToString(),
                        EmailID = student.ItemArray[3].ToString(),
                        StudentRollNumber = student.ItemArray[4].ToString(),
                        CollegeDetails = _eatDBContext.Master_CollegeDetails.Where(m => m.CollegeName.Equals(CollegeName)).FirstOrDefault(),
                        CreatedBy = 1,//Update with user sessionID
                        CreatedDate = DateTime.Now
                    });
                    
                }
                return _eatDBContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Debug.Print(ex.Message);
                return 0;
            }
             


        }
        public List<String> InsertTblEventAttendees(int EventID)
        {
            List<String> EmailList = new List<string>();
            foreach (var student in _eatDBContext.RegisteredStudents.ToList())
            {
                String EmailID = student.EmailID;
                EmailList.Add(EmailID);
                _eatDBContext.EventAttendees.Add(
                new EventAttendees
                {
                    RegisteredStudents = _eatDBContext.RegisteredStudents.Where(m => m.EmailID == EmailID).FirstOrDefault(),
                    EventDetails = _eatDBContext.EventDetails.Where(m => m.ID == EventID).FirstOrDefault(),
                    QRString = "",
                    isPresent = false,
                    MailSent = false
                });

            }
            if (_eatDBContext.SaveChanges() > 0)
            {
                return EmailList;
            }
            return null;
        }
    }
}
