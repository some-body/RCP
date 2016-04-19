using AdminFrontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Error404()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Не найдено",
                Message = "Запрашиваемый ресурс не найден"
            });
        }

        public ActionResult Error401()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Нет доступа",
                Message = "Доступ к запрашиваемому ресурсу запрещен"
            });
        }

        public ActionResult Default()
        {
            return View("Error", new ErrorViewModel
            {
                Title = "Ошибка",
                Message = "Во время выполнения запроса произошла ошибка"
            });
        }
    }
}