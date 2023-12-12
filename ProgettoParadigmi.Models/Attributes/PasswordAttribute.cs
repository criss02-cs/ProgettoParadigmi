using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProgettoParadigmi.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PasswordAttribute(int minLength) : ValidationAttribute
    {
        public int MinLength { get; } = minLength;
        public override bool IsValid(object? value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
                return false;
            if (password.Length < 8)
                return false;
            const string pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-_]).*$";
            var regex = new Regex(pattern);
            return regex.IsMatch(password);
        }
    }
}
