﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderFoodApi.Entity
{
    public class ChiTietDonHang
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChiTietDonHangId { get; set; }

        public int DonHangId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(DonHangId))]
        public DonHang DonHang { get; set; }

        public int MonAnId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(MonAnId))]
        public MonAn MonAn { get; set; }

        public int DonGia { get; set; }

        public int SoLuong { get; set; }
    }
}