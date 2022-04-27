using Shared.Dapper;
using Shared.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketMe.Share.Validation
{
   public class RegexValidation : Service<Validation> ,IRegexValidation
    {
        public RegexValidation(IUnitOfWork uow) : base(uow)
        {

        }
        public bool EmailValidation(string email)
        {
            var validEmail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (!validEmail.IsMatch(email))
            {
                this.Results.Add(new ValidationResult("Invalid Email Address"));
                return false;
            }
            return true;
        }

        public bool PasswordValidation(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum6Chars = new Regex(@".{6,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(password))
            {
                this.Results.Add(new ValidationResult("Password should contain At least one lower case letter"));
                return false;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                this.Results.Add(new ValidationResult("Password should contain At least one upper case letter"));
                return false;
            }

            else if (!hasNumber.IsMatch(password))
            {
                this.Results.Add(new ValidationResult("Password should contain At least one numeric value"));
                return false;
            }
            else if (!hasMinimum6Chars.IsMatch(password))
            {
                this.Results.Add(new ValidationResult("Password should be At least 6 character"));
                return false;
            }

            else if (!hasSymbols.IsMatch(password))
            {
                this.Results.Add(new ValidationResult("Password should contain At least one special case characters"));
                return false;
            }
            else
            {
                return true;
            }

         
        }

    }
}
