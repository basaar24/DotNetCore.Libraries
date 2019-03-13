using System;
using System.Reflection;
using DotNetCore.API.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetCore.API.Common.Helpers
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == typeof(DotNetCoreTypeBase) || property.DeclaringType.BaseType == typeof(DotNetCoreTypeBase))
            {
                if (property.PropertyName == "serializableProperties")
                {
                    property.ShouldSerialize = instance => { return false; };
                }
                else
                {
                    property.ShouldSerialize = instance =>
                    {
                        var p = (DotNetCoreTypeBase)instance;
                        return p.GetSerializableProperties().Contains(property.PropertyName);
                    };
                }
            }
            return property;
        }
    }
}
