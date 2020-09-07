using System;
using System.Collections.Generic;
using System.Text;

namespace PresentConnection.VATCalculator
{
    public interface IVATCountryService
    {
       int GetVATByCountryCode(string countryCode);

    }
}
