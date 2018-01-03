using System.ComponentModel.DataAnnotations.Schema;

namespace MailManager.Data
{
    public class Alias
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("source_username")]
        public string SourceUsername { get; set; }

        [Column("source_domain")]
        public string SourceDomainname { get; set; }

        [Column("destination_username")]
        public string DestinationUsername { get; set; }

        [Column("destination_domain")]
        public string DestinationDomainname { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }
    }
}
