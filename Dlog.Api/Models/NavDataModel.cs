using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dlog.Api.Models
{
    public class NavDataModel
    {
        public List<LinkModel> Recents { get; set; }
        public List<LinkModel> Tags { get; set; }
        public List<LinkModel> TimeLine { get; set; }
    }
}
