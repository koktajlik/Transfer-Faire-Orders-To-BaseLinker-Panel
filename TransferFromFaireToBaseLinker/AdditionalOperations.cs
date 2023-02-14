using System;
using System.Globalization;

namespace TransferFromFaireToBaseLinker;

public static class AdditionalOperations
{
    /// <summary>
    /// Convert country code in ISO 3166-1 alpha-3 to ISO 3166-1 alpha-2
    /// </summary>
    /// <param name="countryCode">Country code in ISO 3166-1 alpha-3</param>
    /// <returns>Country code in ISO 3166-1 alpha-2 or Empty string for invalid three-letter country code</returns>
    public static string Alpha3ToAlpha2(string countryCode)
    {
        if (countryCode.Length != 3)
            throw new ArgumentException("Invalid country code length. Country code must be three-letter.");

        countryCode = countryCode.ToUpper();
        var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        foreach (var culture in cultures)
        {
            var region = new RegionInfo(culture.LCID);
            if (region.ThreeLetterISORegionName.ToUpper() == countryCode) return region.TwoLetterISORegionName;
        }

        return string.Empty;
    }

    /// <summary>
    /// Convert Faire date to Unix timestamp
    /// </summary>
    /// <param name="faireDate">Date from Faire (ISO 8601 without colons and dashes)</param>
    /// <returns>Unix timestamp</returns>
    public static int ConvertFaireDateToUnix(string faireDate)
    {
        var dateParser = DateTime.ParseExact(faireDate, "yyyyMMddTHHmmss.fffZ", CultureInfo.InvariantCulture);
        return (int)((DateTimeOffset)dateParser).ToUnixTimeSeconds();
    }
}