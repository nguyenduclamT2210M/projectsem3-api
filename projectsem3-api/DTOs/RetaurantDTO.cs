﻿using projectsem3_api.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectsem3_api.DTOs
{
    public class RestaurantDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime JoinDate { get; set; }

        public string Description { get; set; }

        public string DeliveryHours { get; set; }

        public string MinimumDelivery { get; set; }

        public string Thumbnail { get; set; }

        public string Banner { get; set; }

        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }

        public int CatId { get; set; }
        [ForeignKey("CatId")]
        public Category Category { get; set; }

        public int AdminId { get; set; }
        [ForeignKey("AdminId")]
        public Admin Admin { get; set; }
    }
}
