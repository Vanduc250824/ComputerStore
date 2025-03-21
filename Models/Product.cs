﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Computer_Store.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ProductPromotions = new HashSet<ProductPromotion>();
        }
    
        public int ProductID { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        public Nullable<int> CategoryID { get; set; }

        [Display(Name = "Giá")]
        public decimal Price { get; set; }
        [Display(Name = "số lượng tồn kho")]
        public int StockQuantity { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name ="Hình ảnh sản phẩm")]
        public string ImageUrl { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }
    }
}
