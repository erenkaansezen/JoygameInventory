﻿using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{


public class ProductEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        public string? ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Barkod gereklidir.")]
        public string? ProductBarkod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama gereklidir.")]

        public string? Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seri numarası gereklidir.")]
        public string? SerialNumber { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public string ProductModel { get; set; } = string.Empty;
        public DateTime ProductAddDate { get; set; }

        public string? Status { get; set; } = null!;
        public int Ram { get; set; }
        public string Storage { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string GraphicsCard { get; set; } = string.Empty;


        public int UserId { get; set; }

        public int? SelectedUserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçin.")]
        public int SelectedCategoryId { get; set; }

        public int MaintenanceId { get; set; } // Maintenance ID
        public string? MaintenanceDescription { get; set; } // Servis açıklaması
        public string? ServiceTitle { get; set; }

        public string? ServiceAdress { get; set; }

        public List<JoyStaff> JoyStaffs { get; set; }

   
        public List<Category> Categories { get; set; } 


        public IEnumerable<InventoryAssigment>? InventoryAssigments { get; set; }
        public IEnumerable<AssigmentHistory>? AssigmentHistorys { get; set; }
        public Maintenance Maintenance { get; set; }

        public IEnumerable<MaintenanceHistory>? MaintenanceHistorys { get; set; }

        public IEnumerable<ProductCategory> ProductCategory { get; set; } = new List<ProductCategory>();
    }



}





