namespace MailManager.Models
{
    public class DomainViewModel
    {
        public int Id { get; set; }

        public string Domainname { get; set; }

        public string ConfirmDelete { get; set; }

        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public int UserCount { get; set; }

        public long DirectorySize { get; set; }

        public long FreeSpace { get; set; }

        public int PercentageUsed
        {
            get
            {
                return (int)((float)(DirectorySize == 0 ? 1 : DirectorySize) / FreeSpace * 100);
            }
        }

        public int PercentageFree
        {
            get
            {
                return 100 - PercentageUsed;
            }
        }

        public string MemoryUsage
        {
            get
            {
                string used = $"({DirectorySize} bytes belegt, ";
                long sizeGb = DirectorySize / 1024 / 1024 / 1024;
                if(sizeGb < 1024 && sizeGb > 0) used = $"({sizeGb} GB belegt, ";
                long sizeMb = DirectorySize / 1024 / 1024;
                if (sizeMb < 1024 && sizeMb > 0) used = $"({sizeMb} MB belegt, ";
                long sizeKb = DirectorySize / 1024;
                if (sizeKb < 1024 && sizeKb > 0) used = $"({sizeKb} kB belegt, ";

                string free = $"{FreeSpace} bytes frei)";
                sizeGb = FreeSpace / 1024 / 1024 / 1024;
                if (sizeGb < 1024 && sizeGb > 0) free = $"{sizeGb} GB frei)";
                sizeMb = FreeSpace / 1024 / 1024;
                if (sizeMb < 1024 && sizeMb > 0) free = $"{sizeMb} MB frei)";
                sizeKb = FreeSpace / 1024;
                if (sizeKb < 1024 && sizeKb > 0) free = $"{sizeKb} kB frei)";

                return used + free;
            }
        }
    }
}