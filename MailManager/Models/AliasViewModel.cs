using MailManager.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace MailManager.Models
{
    public class AliasViewModel
    {
        public int Id { get; set; }

        public string SourceUsername { get; set; }

        public int SourceDomainname { get; set; }

        public string DestinationUsername { get; set; }

        public int DestinationDomainname { get; set; }

        public bool Enabled { get; set; }

        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public List<Domain> DomainnameList { get; set; }
    }
}