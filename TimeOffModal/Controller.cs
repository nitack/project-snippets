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