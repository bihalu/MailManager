using System.ComponentModel.DataAnnotations.Schema;

namespace MailManager.Data
{
    public class Account
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("domain")]
        public string Domainname { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("quota")]
        public int Quota { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }

        [Column("sendonly")]
        public bool Sendonly { get; set; }
    }
}
