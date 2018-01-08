namespace MailManager.Models
{
    public class DomainViewModel
    {
        public int Id { get; set; }

        public string Domainname { get; set; }

        public int UserCount { get; set; }

        public long DirectorySize { get; set; }

        public long FreeSpace { get; set; }
    }
}