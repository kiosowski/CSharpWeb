﻿using System;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"{nameof(input)} cannot be null");
            }
            return Char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
