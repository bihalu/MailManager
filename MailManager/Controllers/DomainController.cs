using MailManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MailManager.Controllers
{
    [Authorize]
    public class DomainController : Controller
    {
        private readonly DataContext _dataContext;

        public DomainController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            var domains = new List<Policy>();

            if (_dataContext.Domains.Any())
            {
                foreach (var domain in _dataContext.Domains)
                {
                    Policy policy = _dataContext.Policies.Where(d => d.Domainname == domain.Domainname).FirstOrDefault();

                    if (null == policy)
                    {
                        policy = new Policy()
                        {
                            Domainname = domain.Domainname,
                            Policyrule = string.Empty,
                            Params = string.Empty
                        };
                    }

                    domains.Add(policy);
                }
            }
            return View(domains);
        }
    }
}
