using System;
using System.Collections.Generic;
using System.Text;

namespace PresentConnection.VATCalculator
{
    public class VATCalculator : IVATCalculator
    {

        public IEUService EUService;

        public IVATCountryService VATCountryService;

        public double CalculateVAT(Customer customer, ServiceProvider serviceProvider, double amount)
        {
            if (!serviceProvider.IsVATPayer()) return 0;

            if (!EUService.IsCountryInEU(customer.CountryCode)) return 0;

            var CalculatedVATInCustomerCountry = (VATCountryService.GetVATByCountryCode(customer.CountryCode) / 100) * amount;

            if (customer.CountryCode != serviceProvider.CountryCode)
            {
                if (customer.IsVATPayer()) return 0;
                return CalculatedVATInCustomerCountry;
            }
            else return CalculatedVATInCustomerCountry;
        }
    }
}
