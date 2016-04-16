using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using Distributed;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Admin")]
    public class TeachersController : RCPController
    {
        private ApiQueryProvider<ICollection<SystemUser>, SystemUser> _teachersQueryProvider;
        const string PasswordPlaceholder = "***";


        public TeachersController()
        {
            var backendUrl = Properties.Resources.SystemUsersBackendURL;
            _teachersQueryProvider = new ApiQueryProvider<ICollection<SystemUser>, SystemUser>(backendUrl, "SystemUsers");
        }

        public ActionResult Index()
        {
            var teachers = _teachersQueryProvider.Get()
                .Where(e => e.Role == "Teacher");

            var tableData = new TableViewModel
            {
                ColumnsNames = new string[] { "Логин" },
                Rows = teachers.Select(t => new Row
                {
                    Id = t.Id.Value,
                    Columns = new string[] { t.Login }
                }).ToList(),
                EditAction = "/Teachers/Edit/", // new ActionLink("Edit", "Teachers"),
                DeleteAction = "Teachers/Delete/", //new ActionLink("Delete", "Teachers")
                AddAction = "Teachers/Add" //new ActionLink("Delete", "Teachers")
            };

            var model = new TeachersViewModel
            {
                TableData = tableData
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var teacher = _teachersQueryProvider.Get(id);
            if (teacher.Role != "Teacher")
                return new HttpStatusCodeResult(401);

            ViewBag.Title = "Редактирование учителя";
            return View(new TeacherEditViewModel
            {
                Id = teacher.Id.Value,
                Login = teacher.Login,
                Password = PasswordPlaceholder
            });
        }

        public ActionResult Add()
        {
            ViewBag.Title = "Создание учителя";
            return View("Edit", new TeacherEditViewModel
            {
                Id = null,
                Login = "",
                Password = ""
            });
        }

        [HttpPost]
        public JsonResult Save(TeacherEditViewModel model)
        {
            if (!ModelState.IsValid)
                return Json("Проверьте правильность заполненных полей");

            var teacher = new SystemUser
            {
                Id = model.Id,
                Login = model.Login,
                PasswordHash = model.Password != PasswordPlaceholder
                    ? model.Password
                    : null,
                Role = "Teacher"
            };

            var result = _teachersQueryProvider.Post(teacher);
            var msg = result.Success
                ? "Успех"
                : result.Message;

            return Json(new
            {
                returnTo = Url.Action("Index"),
                success = result.Success,
                msg
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _teachersQueryProvider.Delete(id);

            var msg = result.Success
                ? "Успех"
                : result.Message;

            return Json(new
            {
                success = result.Success,
                msg
            });
        }
    }
}