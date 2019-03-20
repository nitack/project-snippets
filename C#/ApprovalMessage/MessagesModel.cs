namespace ScheduleUsers.Models
{
    public class Message     
     {   
	    public Message(TimeOffEvent e, string s, ApplicationDbContext db) : this()
       	    {

            	Recipient = db.Users.Find(e.User.Id);
            	TimeOffEventID = e;
            	Sender = db.Users.Find(s);
            	DateSent = DateTime.Now;
            	string state = "";
            	if (e.ActiveSchedule == true)
            	{
                    state = "Approved";
            	}
            	else if (e.ActiveSchedule == false)
            	{
                    state = "Denied";
            	}
            	MessageTitle = "Time Off " + state;
            	Content = Sender.FirstName + " has " + state + " your time off request from " + e.Start + " to " + e.End + ". " + e.Note;
            }
     }
}