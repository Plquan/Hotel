﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int RoomTypeId { get; set; }
		public int RoomNumber { get; set; }
        public string Status { get; set; }
        public RoomType RoomType { get; set; }
    }
}