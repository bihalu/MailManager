using MailManager.Data;
using MailManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MailManager.Controllers
{
    [Authorize]
    public class DomainController : Controller
    {
        private readonly DataContext _dataContext;

        private IConfiguration _configuration;

        public DomainController(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var domains = new List<DomainViewModel>();

            if (_dataContext.Domains.Any())
            {
                foreach (var d in _dataContext.Domains)
                {
                    var userCount = _dataContext.Accounts.Where(a => a.Domainname == d.Domainname).Count(); 
                    
                    var mailDirectory = _configuration["Mail:Directory"];
                    var driveInfo = new DriveInfo(mailDirectory.Substring(0, 1));
                    var domainDirectory = new DirectoryInfo(Path.Combine(mailDirectory, d.Domainname));
                    var directorySize = DirectorySize(domainDirectory, true);

                    var domain = new DomainViewModel()
                    {
                        Id = d.Id,
                        Domainname = d.Domainname,
                        UserCount = userCount,
                        DirectorySize = directorySize,
                        FreeSpace = driveInfo.AvailableFreeSpace                        
                    };

                    domains.Add(domain);
                }
            }
            return View(domains);
        }

        private long DirectorySize(DirectoryInfo dir, bool includeSubDir)
        {
            long size = 0;

            try
            {
                size = dir.EnumerateFiles().Sum(f => f.Length);

                if (includeSubDir)
                {
                    size += dir.EnumerateDirectories().Sum(d => DirectorySize(d, true));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error Get DirectorySize: " + e.Message);
            }

            return size;
        }

        [Authorize(Policy = "IsAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
