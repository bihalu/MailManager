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

            if (_dataContext.Domains.Count() > 0)
            {
                foreach (var domain in _dataContext.Domains)
                {
                    domains.Add(GetDomainInfo(domain.Id));
                }
            }
            return View(domains);
        }

        private DomainViewModel GetDomainInfo(int id)
        {
            var domain = _dataContext.Domains.Find(id);

            if (null == domain) throw new Exception($"Domäne mit der id={id} nicht gefunden!");

            var userCount = _dataContext.Accounts.Where(account => account.Domainname == domain.Domainname).Count();

            var mailDirectory = _configuration["Mail:Directory"];
            var driveInfo = new DriveInfo(mailDirectory.Substring(0, 1));
            var domainDirectory = new DirectoryInfo(Path.Combine(mailDirectory, domain.Domainname));
            var directorySize = GetDirectorySize(domainDirectory, true);

            return new DomainViewModel()
            {
                Id = domain.Id,
                Domainname = domain.Domainname,
                UserCount = userCount,
                DirectorySize = directorySize,
                FreeSpace = driveInfo.AvailableFreeSpace
            };
        }

        private long GetDirectorySize(DirectoryInfo dir, bool includeSubDir)
        {
            long size = 0;

            try
            {
                size = dir.EnumerateFiles().Sum(f => f.Length);

                if (includeSubDir)
                {
                    size += dir.EnumerateDirectories().Sum(d => GetDirectorySize(d, true));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error GetDirectorySize: " + e.Message);
            }

            return size;
        }

        [Authorize(Policy = "IsAdmin")]
        public IActionResult Create([FromForm] DomainViewModel domain)
        {
            if(string.IsNullOrEmpty(domain.Domainname))
            {
                domain.Message = "Bitte neuen Domänennamen eingeben";

                return View(domain);
            }

            var existingDomain = _dataContext.Domains.Where(d => d.Domainname == domain.Domainname).FirstOrDefault();

            if(null != existingDomain)
            {
                domain.Message = $"Domäne {domain.Domainname} existiert schon";

                return View(domain);
            }

            _dataContext.Domains.Add(new Domain() { Domainname = domain.Domainname });
            _dataContext.SaveChanges();

            return RedirectToAction("Index", "Domain");
        }

        [Authorize(Policy = "IsAdmin")]
        public IActionResult Delete([FromQuery] int id, [FromForm] DomainViewModel domain)
        {
            if(domain.Id == 0)
            {
                domain = GetDomainInfo(id);
            }

            if(domain.UserCount > 0)
            {
                domain.Message = $"Die Domäne {domain.Domainname} hat noch Benutzer!";

                return View(domain);
            }

            if(domain.ConfirmDelete != domain.Domainname)
            {
                domain.Message = $"Der Domänen Name {domain.Domainname} stimmt nicht";

                return View(domain);
            }

            _dataContext.Domains.Remove(_dataContext.Domains.Find(domain.Id));
            _dataContext.SaveChanges();

            return RedirectToAction("Index", "Domain");
        }
    }
}
