using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gis.DAL.Models
{
    public class BaseEntity
    {
        [Key]
        [Column("OBJECTID")]
        public int Objectid { get; set; }

    }
}
