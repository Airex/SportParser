namespace TelegramBot.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Setting
    {
        public int ID { get; set; }

        public int userId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        public string value { get; set; }

        public virtual User User { get; set; }
    }
}
