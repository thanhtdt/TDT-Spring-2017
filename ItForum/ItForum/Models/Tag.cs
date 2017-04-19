﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItForum.Models
{
    public class Tag
    {
        private string _name;

        [Key]
        public string Name
        {
            get => _name;
            set => _name = value.ToLower();
        }

        public string Description { get; set; }

        public List<ContainerTag> ContainerTags { get; set; }

        public List<UserTag> UserTags { get; set; }

        public bool Equals(Tag obj)
        {
            return Name == obj.Name;
        }
    }
}