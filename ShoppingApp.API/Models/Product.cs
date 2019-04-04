using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShoppingApp.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayImageUrl { get; set; }
        public int QuantityAvailable { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public Category CategoryName { get; set; }
        public int CategoryId { get; set; }
        public Merchant Merchant { get; set; }
        public int MerchantId { get; set; }
        public double Price { get; set; }
        public bool PickUpAvailable { get; set; }
        public Store StoreLocation { get; set; }
        public int StoreId { get; set; }
        public DateTime DateStocked { get; set; }
        public DateTime DateSold { get; set; }
        public ICollection<Feature> Features { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Photo> Photos { get; set; }   
        public Product()
        {
            this.Features = new Collection<Feature>();
            this.Reviews = new Collection<Review>();
            this.Photos = new Collection<Photo>();
        }
    }
}