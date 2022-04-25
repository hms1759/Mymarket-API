using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Validation
{
  public  interface IRegexValidation
    {
        bool PasswordValidation(string password);
        bool EmailValidation(string password);

    }
}
