 [Authorize(Roles = "Admin")]
    public class TimeOffEventController : Controller
    {
        //Generate User's first and last name in navigation bar
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    string fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
                    ViewData.Add("FullName", fullName);
                    ViewData.Add("ClockedInStatus", user.GetStatus());
                }
            }
            base.OnActionExecuted(filterContext);
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string filter = "")
        {
            List<SelectListItem> FullNames = new List<SelectListItem>();
            
            var positions = db.Users.Select(y => y.Position).Distinct().ToList();
            var departments = db.Users.Select(z => z.Department).Distinct().ToList();

            var p = positions.Select((r, index) => new SelectListItem { Text = r, Value = index.ToString() });
            var d = departments.Select((r, index) => new SelectListItem { Text = r, Value = index.ToString() });
            ViewBag.Filter = filter;
            ViewBag.Positions = p;
            ViewBag.Departments = d;

            foreach (ApplicationUser person in db.Users)
            {
                string FullName = person.FirstName + " " + person.LastName;
                string ID = person.Id;
                FullNames.Add(new SelectListItem() { Text = FullName, Value = ID });

            }
            ViewBag.Id = new SelectList(FullNames, "Value", "Text");
            //ViewBag.Id = new SelectList(db.Users, "ID", "FirstName");
            return View("Index");
        }

        public PartialViewResult IndexUserList(string filter)
        {
            // if no filter set, return all users
            if (filter == null || filter == "")
            {
                ViewBag.filtertype = "";
                return PartialView(db.Users.ToList());
            }
            // get all users
            var allusers = db.Users.ToList();
            var filtered = new List<ApplicationUser>();

            // set filtercase
            var filtercase = "";
            if (filter.Substring(0, 3) == "dpt") filtercase = "Dept";
            else if (filter.Substring(0, 3) == "pos") filtercase = "Position";
            else filtercase = filter;

            switch (filtercase)
            {
                case "Clocked In":
                    foreach (var user in allusers)
                    {
                        if (user.GetStatus() == "Clocked In") filtered.Add(user);
                    }
                    break;

                case "Clocked Out":
                    foreach (var user in allusers)
                    {
                        if (user.GetStatus() == "Clocked Out") filtered.Add(user);
                    }
                    break;

                case "Full time":
                    filtered = db.Users.Where(x => x.Fulltime == true).ToList();
                    break;

                case "Part time":
                    filtered = db.Users.Where(x => x.Fulltime == false).ToList();
                    break;

                case "Dept":
                    filtered = db.Users.Where(x => x.Department == filter.Substring(4)).ToList();
                    break;

                case "Position":
                    filtered = db.Users.Where(x => x.Position == filter.Substring(4)).ToList();
                    break;

                default:
                    filtered = allusers;
                    break;
            }

            return PartialView(filtered);
        }


        // GET: Employer/TimeOffEvent
        public PartialViewResult ProcessedIndex(string sortOrder, string data, string filter = "")
        {
                        
            //Keeping track of current sort
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Last Name" ? "name_desc" : "Last Name";
            ViewBag.LengthSortParm = sortOrder == "Length of Event" ? "lengthOfEvent_desc" : "Length of Event";
            ViewBag.SubmittedSortParm = sortOrder == "Submitted" ? "submitted_desc" : "Submitted";



            // if no filter set, return all events
            if (filter == null || filter == "")
            {
                var unfiltered = new List<TimeOffViewModel>();
                ViewBag.filtertype = "";
                foreach (var record in db.TimeOffEvents.ToList())
                {
                    unfiltered.Add(new TimeOffViewModel(record));
                }
                return PartialView("_Index", unfiltered);
            }

            var filtered = new List<TimeOffEvent>();


            // set filtercase
            var filtercase = "";
            if (filter.Substring(0, 3) == "dpt") filtercase = "Dept";
            else if (filter.Substring(0, 3) == "pos") filtercase = "Position";
            else if (filter.Substring(0, 2) == "Id") filtercase = "Id";
            else filtercase = filter;

            switch (filtercase)
            {
                case "Clocked In":
                    filtered = db.TimeOffEvents.Where(x => x.User.Status == "Clocked In").ToList();
                    break;
                    
                case "Clocked Out":
                    filtered = db.TimeOffEvents.Where(x => x.User.Status == "Clocked Out").ToList();
                    break;

                case "Full time":
                    filtered = db.TimeOffEvents.Where(x => x.User.Fulltime == true).ToList();
                    break;

                case "Part time":
                    filtered = db.TimeOffEvents.Where(x => x.User.Fulltime == false).ToList();
                    break;

                case "Dept":
                    filtered = db.TimeOffEvents.Where(x => x.User.Department == filter.Substring(4)).ToList();
                    break;

                case "Position":
                    filtered = db.TimeOffEvents.Where(x => x.User.Position == filter.Substring(4)).ToList();
                    break;

                case "Id":
                    filtered = db.TimeOffEvents.Where(x => x.User.Id == filter.Substring(3)).ToList();
                    break;

                default:
                    filtered = db.TimeOffEvents.ToList();
                    break;
            }

            List<TimeOffViewModel> timeOffList = new List<TimeOffViewModel>();

            //var timeOffEvents = new List<TimeOffEvent>();

            

            if (data == "index")
            {
                filtered = filtered.Where(x => x.ApproverId == null).OrderBy(a => a.Start).ToList();
            }
            else if (data == "processsed")
            {
                filtered = filtered.Where(x => x.ApproverId != null).OrderBy(a => a.Start).ToList();
            }

            foreach (var timeOffEvent in filtered)
            {
                TimeOffViewModel timeOff = new TimeOffViewModel(timeOffEvent);
                timeOffList.Add(timeOff);
            }

            
            IEnumerable<TimeOffViewModel> timeOffEventsEnumerable;


            switch (sortOrder)
            {
                case "date_desc":
                    timeOffEventsEnumerable = timeOffList.OrderByDescending(t => t.Start).ToList();
                    break;
                case "Last Name":
                    timeOffEventsEnumerable = timeOffList.OrderBy(t => t.LastName).ToList();
                    break;
                case "name_desc":
                    timeOffEventsEnumerable  = timeOffList.OrderByDescending(t => t.LastName).ToList();
                    break;
                case "Length of Event":
                    timeOffEventsEnumerable = timeOffList.OrderBy(t => t.RequestLength).ToList();
                    break;
                case "lengthOfEvent_desc":
                    timeOffEventsEnumerable = timeOffList.OrderByDescending(t => t.RequestLength).ToList();
                    break;
                case "Submitted":
                    timeOffEventsEnumerable = timeOffList.OrderBy(t => t.Submitted).ToList();
                    break;
                case "submitted_desc":
                    timeOffEventsEnumerable = timeOffList.OrderByDescending(t => t.Submitted).ToList();
                    break;
                default:
                    timeOffEventsEnumerable = timeOffList.OrderBy(t => t.Start).ToList();
                    break;
            }
            return PartialView("_Index", timeOffEventsEnumerable);
        }