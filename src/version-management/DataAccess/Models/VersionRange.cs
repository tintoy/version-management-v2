using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DD.Cloud.VersionManagement.DataAccess.Models
{
    public class VersionRange
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DefaultValue(VersionComponent.Build)]
        public VersionComponent IncrementBy { get; set; } = VersionComponent.Build;

        [Required]
        public int StartVersionMajor { get; set; }

        [Required]
        public int StartVersionMinor { get; set; }

        [Required]
        public int StartVersionBuild { get; set; }

        [Required]
        public int StartVersionRevision { get; set; }

        [Required]
        public int EndVersionMajor { get; set; }

        [Required]
        public int EndVersionMinor { get; set; }

        [Required]
        public int EndVersionBuild { get; set; }

        [Required]
        public int EndVersionRevision { get; set; }

        public ICollection<Release> Releases { get; set; }
    }
}
