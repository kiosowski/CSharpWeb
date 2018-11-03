using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public class User : BaseModel<string>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}
