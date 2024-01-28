using System.Text.RegularExpressions;
using System.Globalization;

namespace VirtualLibrary.Validators;

public class MyValidators {
    public static string InvalidInterval { get; } = "Invalid interval.";
    public static string InvalidUrl { get; } = "Invalid Url.";
    public static string InvalidEmail { get; } = "Invalid Email.";
    public static string InvalidAge { get; } = "Please introduce a valid age (No younger than 15 years old).";
    public static string InvalidIsbn { get; } = "Invalid Isbn.";
    public static string InvalidQualification { get; } = "Invalid Qualification: Must be between 1 and 5.";
    public static string InvalidEmpty { get; } = "Invalid parameters, some required parameters are empty.";
    public static string InvalidPagesCount { get; } = "Invalid pages count.";
    public static bool IsValidEmail(string email) {
        if (email == null || email == "") return false;
        try {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
            string DomainMapper(Match match) {
                var idn = new IdnMapping();
                string domainName = idn.GetAscii(match.Groups[2].Value);
                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException ex) {
            return false;
        }
        catch (ArgumentException ex) {
            return false;
        }
        try {
            return Regex.IsMatch(
                email,
                // Basic email regex, extend as needed
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200)
            );
        }
        catch (RegexMatchTimeoutException ex) {
            return false;
        }
    }

    public static bool IsValidUri(string uri) {
        return Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out _);
    }

    public static bool IsValidInterval(int offset, int limit) {
        return offset >= 0 && limit > 0;
    }

    public static bool IsValidAge(DateTime birthDate) {
        DateTime now = DateTime.Today;
        int age = now.Year - birthDate.Year;
        if (age < 15) return false;
        return true;
    }
    public static bool IsValidIsbn(string isbn) {
        string[] split1 = isbn.Split('-');
        long sum = 0;
        int exp = 0;
        for (int i = split1.Length -2; i >= 0 ; i--) {
            if (!long.TryParse(split1[i], out long semi)) return false;
            sum += semi * (long)Math.Pow(10, exp);
            exp += split1[i].Length;
        }
        if (exp != 10 && exp != 13) {
            return false;
        }
        else if (exp == 10) {
            sum += 978 * (long)Math.Pow(10, 10);
        }
        int remainder = (int)(sum % 10);
        int checksum = 0;
        int count = 0;
        while(sum > 0) {
            sum /= 10;
            checksum += (int)(sum % 10) * (count % 2 == 0 ? 3 : 1);
            count += 1;
        }
        return (checksum % 10 + remainder) % 10 == 0;
    }

    public static bool IsValidQualification(int qualification) {
        return qualification > 0 && qualification <= 5;
    }

    public static bool IsNotEmpty(string str) {
        return str != null && str != "";
    }

    public static bool IsValidPageCount(int pages) {
        return pages > 10;
    }
}