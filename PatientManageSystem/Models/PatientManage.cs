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

    public partial class PatientManage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PatientManage()
        {
            this.AppointmentManages = new HashSet<AppointmentManage>();
        }
        [Required]
        public int Pid { get; set; }
        [Required]
        public string Pname { get; set; }
        [Required]
        public string Address1 { get; set; }
        [Required]
        public string Address2 { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public System.DateTime DOB { get; set; }
        [Required]
        public string PContactNumber { get; set; }
        [Required]
        public string PEmail { get; set; }
        [Required]
        public string Cretedby { get; set; }
        [Required]
        public string Modifiedby { get; set; }
        [Required]
        public Nullable<System.DateTime> Creteddate { get; set; }
        [Required]
        public Nullable<System.DateTime> Modifieddate { get; set; }
        [Required]
        public int RefCityid { get; set; }
        [Required]

        public virtual CityMaster CityMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppointmentManage> AppointmentManages { get; set; }
    }
}
