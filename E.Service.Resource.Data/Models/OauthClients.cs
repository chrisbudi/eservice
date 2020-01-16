using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class OauthClients
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Secret { get; set; }
        public string Redirect { get; set; }
        public bool PersonalAccessClient { get; set; }
        public bool PasswordClient { get; set; }
        public bool Revoked { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}