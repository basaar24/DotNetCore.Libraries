using DotNetCore.API.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DotNetCore.API.Common.Models
{
    public class DotNetCoreTypeBase
    {
        public void Validate()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, context, results, true);

            if (!isValid)
                throw new DotNetCoreAPICommonException();
        }

        //https://www.c-sharpcorner.com/UploadFile/f7a3ed/fields-filtering-in-asp-net-web-api/
        [NotMapped]
        private List<string> serializableProperties { get; set; }

        public List<string> GetSerializableProperties() => serializableProperties;

        public void SetSerializableProperties(string fields)
        {
            if (!string.IsNullOrEmpty(fields))
            {
                var returnFields = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                serializableProperties = returnFields.ToList();
                return;
            }
            var members = this.GetType().GetMembers();

            serializableProperties = new List<string>();
            serializableProperties.AddRange(members.Select(x => x.Name).ToList());
        }
    }
}
