using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Haooyou.Ticket.WebApp.Utility.Json
{
    public class IncludePropertiesContractResolver : DefaultContractResolver
    {
        private readonly IEnumerable<string> lstInclude;

        public IncludePropertiesContractResolver(params string[] includedProperties)
        {
            lstInclude = includedProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).ToList().FindAll(p => lstInclude.Any(x => x.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}