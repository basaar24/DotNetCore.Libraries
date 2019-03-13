using System;

namespace DotNetCore.API.Common.DotNetCoreSieve
{
    public class FieldTerm : IFieldTerm
    {
        public FieldTerm() { }

        private string _field;

        public string Field
        {
            set
            {
                _field = value;
            }
        }

        public string Name => _field;
    }
}
