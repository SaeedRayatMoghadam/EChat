using System.ComponentModel.DataAnnotations;

namespace EChat.DataLayer.Entities.Roles
{
    public class Role : BaseEntity
    {
        [MaxLength(20)]
        public string Title { get; set; }

    }
}