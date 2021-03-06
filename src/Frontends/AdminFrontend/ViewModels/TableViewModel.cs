﻿using System.Collections.Generic;

namespace AdminFrontend.ViewModels
{
    public class Row
    {
        public int Id { get; set; }
        public string[] Columns { get; set; }
    }

    //public class ActionLink
    //{
    //    public ActionLink(string actionName, string controllerName)
    //    {
    //        ActionName = actionName;
    //        ControllerName = controllerName;
    //    }

    //    public string ActionName { get; set; }
    //    public string ControllerName { get; set; }
    //}


    public class TableViewModel
    {
        public string[] ColumnsNames { get; set; }
        public ICollection<Row> Rows { get; set; }
        public string InfoAction { get; set; }
        public string EditAction { get; set; }
        public string DeleteAction { get; set; }
        public string AddAction { get; set; }
    }
}