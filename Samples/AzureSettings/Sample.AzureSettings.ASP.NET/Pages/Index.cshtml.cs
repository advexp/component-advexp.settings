﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sample.AzureSettings.ASP.NET.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }

        public void OnGetAsync()
        {
            Settings.LoadSettings();
            Settings.LoadSettings();

            if (Settings.Secret == null)
            {
                Message = "Cant get KeyVault secret value";
            }
            else
            {
                Message = "KeyVault secrete is: " + Settings.Secret.Value;
            }
        }
    }
}
