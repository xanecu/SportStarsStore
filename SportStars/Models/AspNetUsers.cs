using System.ComponentModel.DataAnnotations;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//public string Id { get; set; }
//[EmailAddress(ErrorMessage = "Invalid email")]
//public string Email { get; set; }

//public bool EmailConfirmed { get; set; }
//public string PasswordHash { get; set; }
//public string SecurityStamp { get; set; }
//[RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid phone number")]
//public string PhoneNumber { get; set; }

//public bool PhoneNumberConfirmed { get; set; }
//public bool TwoFactorEnabled { get; set; }
//public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
//public bool LockoutEnabled { get; set; }
//[RegularExpression(@"^[0-9]\d*$", ErrorMessage = "Invalid integer")]
//public int AccessFailedCount { get; set; }
//[DataType(DataType.Text), MaxLength(256, ErrorMessage = "Out of lenght")]
//public string UserName { get; set; }
//[DataType(DataType.Text), MaxLength(250, ErrorMessage = "Out of lenght")]
//public string images_path { get; set; }
//[DataType(DataType.Text), MaxLength(20, ErrorMessage = "Out of lenght")]
//public string user_status { get; set; }
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportStars.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUsers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUsers()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaims>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogins>();
            this.orders = new HashSet<orders>();
            this.AspNetRoles = new HashSet<AspNetRoles>();
        }
    
        public string Id { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
        
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        [RegularExpression(@"^[0-9]\d*$", ErrorMessage = "Invalid integer")]
        public int AccessFailedCount { get; set; }
        [DataType(DataType.Text), MaxLength(256, ErrorMessage = "Out of lenght")]
        public string UserName { get; set; }
        [DataType(DataType.Text), MaxLength(250, ErrorMessage = "Out of lenght")]
        public string images_path { get; set; }
        [DataType(DataType.Text), MaxLength(20, ErrorMessage = "Out of lenght")]
        public string user_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orders> orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; }
    }
}
