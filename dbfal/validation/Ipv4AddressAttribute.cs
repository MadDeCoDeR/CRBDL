
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dbfal.validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class Ipv4AddressAttribute : DataTypeAttribute
    {
        public Ipv4AddressAttribute() : base(DataType.Custom)
        {
        }

        public override bool IsValid(object? value)
        {
            if (value == null) {
                return false;
            }

            if (!(value is string valueString))
            {
                return false;
            }

            if (string.IsNullOrEmpty(valueString))
            {
                return false;
            }

            if (!valueString.Contains(".")) {
                return false;
            }

            string[] ipParts = valueString.Split('.');

            if (ipParts.Length != 4)
            {
                return false;
            }

            int maxLength = 0;
            foreach (string ipPart in ipParts) {
                int temp = 0;
                if (!int.TryParse(ipPart, out temp))
                {
                    return false;
                }
                if (ipPart.Length > maxLength)
                {
                    maxLength = ipPart.Length;
                }
            }

            return maxLength <= 3;
        }
    }
}
