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

        [HttpGet]
        [Authorize(Policy="IsAdmin")]
        public IActionResult Edit([FromQuery] string name)
        {
            Policy policy = _dataContext.Policies.Where(d => d.Domainname == name).FirstOrDefault();

            if (null == policy)
            {
                policy = new Policy()
                {
                    Id = -1,
                    Domainname = name,
                    Policyrule = string.Empty,
                    Params = string.Empty
                };
            }

            return View(policy);
        }

        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public IActionResult Edit([FromForm] Policy input)
        {
            if(input.Id < 0)
            {
                var policy = new Policy()
                {
                    Domainname = input.Domainname,
                    Policyrule = input.Policyrule,
                    Params = input.Params
                };

                _dataContext.Policies.Add(policy);
                _dataContext.SaveChanges();

                return RedirectToAction("Index", "Domain");
            }
            else
            {
                var policy = _dataContext.Policies.Find(input.Id);
                if(policy.Policyrule != input.Policyrule || policy.Params != input.Params)
                {
                    policy.Policyrule = input.Policyrule;
                    policy.Params = input.Params;
                    _dataContext.SaveChanges();

                    return RedirectToAction("Index", "Domain");
                }
            }

            return View(input);
        }

        [Authorize(Policy = "IsAdmin")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
