﻿using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class OauthAccessTokens
    {
        public string Id { get; set; }
        public int? UserId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Scopes { get; set; }
        public bool Revoked { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}