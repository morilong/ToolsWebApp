using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json
{
    public class ExcludePropertiesContractResolver : DefaultContractResolver
    {
        private readonly IEnumerable<string> lstExclude;

        public ExcludePropertiesContractResolver(params string[] excludedProperties)
        {
            lstExclude = excludedProperties;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).ToList().FindAll(p => !lstExclude.Any(x => x.Equals(p.PropertyName, StringComparison.OrdinalIgnoreCase)));
        }


    }
}