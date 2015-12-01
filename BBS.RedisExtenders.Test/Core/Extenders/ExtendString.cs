using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BBS.RedisExtenders.Test.Core.Extenders
{
    public static class ExtendString
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(this string phone)
        {
            try
            {
                var tst =
                    new Regex(
                        @"^\({0,1}((0|\+61)(2|4|3|7|8)){0,1}\){0,1}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{1}(\ |-){0,1}[0-9]{3}$");
                return tst.IsMatch(phone);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidDomainName(this string domain)
        {
            try
            {
                var tst = new Regex(@"^(([a-z]|[a-z][a-z0-9\-]*[a-z0-9])\.)*([a-z]|[a-z][a-z0-9\-]*[a-z0-9])$",
                    RegexOptions.IgnoreCase);
                return tst.IsMatch(domain);
            }
            catch
            {
                return false;
            }
        }

        public static string Fmt(this string fmt, params object[] args)
        {
            return string.Format(fmt, args);
        }
    }
}