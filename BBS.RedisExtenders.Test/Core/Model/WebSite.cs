using System;
using System.Collections.Generic;
using BBS.RedisExtenders.Test.Core.Extenders;
using BBS.RedisExtenders.Test.Core.Types;

namespace BBS.RedisExtenders.Test.Core.Model
{
    public class WebSite
    {
        private string _domainName;

        public long SiteId { get; set; }

        public string DomainName
        {
            get { return _domainName; }
            set
            {
                if (value != null)
                {
                    if (!value.IsValidDomainName())
                        throw new Exception($"Domain name '{value}' is invalid.");
                    _domainName = value;
                }
            }
        }

        public string Description { get; set; }
        public DateTime DateCreated { get; } = DateTime.UtcNow;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public TSiteStatus Status { get; set; } = TSiteStatus.Created;
        public string MailDomainName { get; set; } = "none";
        public Dictionary<string, string> Details { get; } = new Dictionary<string, string>();


        public void Update(WebSite details)
        {
            if (!string.IsNullOrEmpty(details.DomainName))
                DomainName = details.DomainName;
            if (!string.IsNullOrEmpty(details.Description))
                Description = details.Description;
            LastUpdated = DateTime.UtcNow;
            Status = details.Status;
            if (!string.IsNullOrEmpty(details.MailDomainName))
                MailDomainName = details.MailDomainName;
            foreach (var key in details.Details.Keys)
                if (Details.ContainsKey(key))
                    Details[key] = details.Details[key];
                else
                    Details.Add(key, details.Details[key]);
        }
    }
}