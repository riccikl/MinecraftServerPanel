using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MinecraftServerPanel.Pages
{
    public class IndexModel : PageModel
    {
        public bool ServerStatus { get; set; }
        
        public void OnGet()
        {
            ServerStatus = true;
        }
    }
}