using MailManager.Data;
using MailManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

using System.Linq;

namespace MailManager.Controllers
{
    public class AliasController : Controller
    {
        private readonly DataContext _dataContext;

        public AliasController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var aliases = new List<Alias>();

            if(_dataContext.Aliases.Any())
            {
                aliases = _dataContext.Aliases.ToList();
            }

            return View(aliases);
        }

        [Authorize(Policy = "IsAdmin")]
        public IActionResult Create([FromForm] AliasViewModel alias)
        {
            if(null == alias.DomainnameList)
            {
                alias.DomainnameList = _dataContext.Domains.ToList();
                alias.Enabled = true;
            }

            if (string.IsNullOrEmpty(alias.SourceUsername) ||
                alias.SourceDomainname == 0 ||
                string.IsNullOrEmpty(alias.DestinationUsername) ||
                alias.DestinationDomainname == 0)
            {
                alias.Message = "Bitte Quell- und Ziel Benutzer und Domänennamen eingeben";

                return View(alias);
            }

            var source = alias.DomainnameList.Find(d => d.Id == alias.SourceDomainname);
            var destination = alias.DomainnameList.Find(d => d.Id == alias.DestinationDomainname);

            _dataContext.Aliases.Add(new Alias()
            {
                SourceUsername = alias.SourceUsername,
                SourceDomainname = source.Domainname,
                DestinationUsername = alias.DestinationUsername,
                DestinationDomainname = destination.Domainname,
                Enabled = alias.Enabled
            });

            _dataContext.SaveChanges();

            return RedirectToAction("Index", "Alias");
        }
    }
}
