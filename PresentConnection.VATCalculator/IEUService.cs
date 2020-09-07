using System;
using System.Collections.Generic;
using System.Text;

namespace PresentConnection.VATCalculator
{
    public interface IEUService
    {
        bool IsCountryInEU(string countryCode);
    }
}
