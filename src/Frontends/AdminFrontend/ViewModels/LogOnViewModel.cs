﻿namespace AdminFrontend.ViewModels
{
    public class LogOnViewModel
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; } = false;
        public string ReturnUrl { get; set; } = "";
    }
}