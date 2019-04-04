using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.DTOS
{
    public class ProductToReturnDto
    {
         public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityAvailable { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string MerchantName { get; set; }
        public double Price { get; set; }

        public int MerchantId { get; set; }
        public bool PickUpAvailable { get; set; }
        public int StoreId { get; set; }
        public DateTime DateStocked { get; set; }
        public ICollection<FeatureToReturn> Features { get; set; }
    }
}