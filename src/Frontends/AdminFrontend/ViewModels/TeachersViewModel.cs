using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminFrontend.ViewModels
{
    public class TeachersViewModel
    {
        public ICollection<SystemUser> Teachers { get; set; }
    }
}