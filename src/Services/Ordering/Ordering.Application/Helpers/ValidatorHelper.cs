using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Helpers;

public class ValidatorHelper
{
    public static bool EmailValidator(string email)
    {
        var emailValidator = new EmailAddressAttribute();
        return emailValidator.IsValid(email);
    }
}
