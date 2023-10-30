using ApplicationCore.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Entities.Supporting
{
    public class ChangeLog : SimpleEntity
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string Data { get; set; }

        public DateTime ChangedDate { get; set; }

        public string ChangedBy { get; set; }
    }
}
