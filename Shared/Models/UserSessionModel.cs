using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp1.Shared.Models
{
    public class UserSessionModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimestamp { get; set; }
    }
}
