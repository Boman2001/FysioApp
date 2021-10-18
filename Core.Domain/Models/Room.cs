using System.Collections.Generic;
using Core.Domain.Enums;

namespace Core.Domain.Models
{
    public class Room : Entity
    {
        public string RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public virtual IEnumerable<Treatment> Treatments { get; set; }

    }
}