using System.ComponentModel.DataAnnotations.Schema;

namespace MailManager.Data
{
    public class TlsPolicy
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("domain")]
        public string Domainname { get; set; }

        [Column("policy")]
        public string Policyrule { get; set; }

        [Column("params")]
        public string Params { get; set; }
    }
}
