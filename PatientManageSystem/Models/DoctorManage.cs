//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PatientManageSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DoctorManage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DoctorManage()
        {
            this.AppointmentManages = new HashSet<AppointmentManage>();
        }
        [Required]
        public int DrId { get; set; }
        [Required]
        public string DrName { get; set; }
        [Required]
        public string DrPassword { get; set; }
        [Required]
        public string Specialzaition { get; set; }
        [Required]
        public string DrEmail { get; set; }
        [Required]
        public string DrContactNumber { get; set; }
        [Required]
        public decimal DrExperience { get; set; }
        [Required]
        public string DrGender { get; set; }
        [Required]
        public Nullable<bool> IsActive { get; set; }
        [Required]
        public string Cretedby { get; set; }
        [Required]
        public string Modifiedby { get; set; }
        [Required]
        public Nullable<System.DateTime> Creteddate { get; set; }
        [Required]
        public Nullable<System.DateTime> Modifieddate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppointmentManage> AppointmentManages { get; set; }
    }
}
