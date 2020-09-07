using System;
using System.Collections.Generic;
using System.Text;

namespace PresentConnection.VATCalculator
{
    public interface IVATCalculator
    {
        double CalculateVAT(Customer customer, ServiceProvider serviceProvider, double amount);
    }
}
