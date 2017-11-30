﻿using System.Text.RegularExpressions;
using System;

namespace Identifiers.Czech
{
    public sealed class IdentificationNumberPattern : IPattern<IdentificationNumber>
    {
        /// <summary>
        /// A regular expressin that defines standard form of the ICO.
        /// </summary>
        private static readonly Regex standardForm = new Regex("^([0-9]{7})([0-9])$");

        /// <summary>
        /// Pattern for 8 digit IČO (Identifikační Číslo Osoby).
        /// </summary>
        /// <example><c>00006947</c></example>
        public static IdentificationNumberPattern StandardPattern { get; } = new IdentificationNumberPattern(); 

        private IdentificationNumberPattern()
        {
        }

        public ParseResult<IdentificationNumber> Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var match = standardForm.Match(text);
            if (!match.Success)
            {
                return ParseResult<IdentificationNumber>.ForException(new FormatException($"Unable to parse identifier number '{text}'. Expecting a text of 8 digits."));
            }

            var number = match.Groups[1].ConvertToLong();
            var checkDigit = match.Groups[2].ConvertToNumber();

            var identificationNumber = new IdentificationNumber(number, checkDigit);
            return ParseResult<IdentificationNumber>.ForValue(identificationNumber);
        }

        /// <summary>
        /// Format identification number in a <see cref="IdentificationNumberPattern.StandardPattern"/>.
        /// </summary>
        /// <param name="value">Identification number.</param>
        /// <returns>Identification number formatted to string.</returns>
        public string Format(IdentificationNumber value)
        {
            return value.ToString("S", null);
        }
    }
}
