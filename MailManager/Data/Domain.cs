using System.ComponentModel.DataAnnotations.Schema;

namespace MailManager.Data
{
    public class Domain
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("domain")]
        public string Domainname { get; set; }
    }
}
