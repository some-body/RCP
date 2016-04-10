using Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkersFrontend.Controllers
{
    public class ExamController : Controller
    {
        private DistributedQueryProvider _examQueryProvider;
        private ApiQueryProvider<ICollection<CourseDto>, Course> _courseQueryProvider;

        public ExamController()
        {
            var backendUrl = Properties.Resources.ExamBackendURL;
            _examQueryProvider = new DistributedQueryProvider(backendUrl);
        }
        // GET: Exam
        public ActionResult Index()
        {
            return View();
        }
    }
}