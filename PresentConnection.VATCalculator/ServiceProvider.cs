using System;
using System.Collections.Generic;
using System.Text;

namespace PresentConnection.VATCalculator
{
    public class ServiceProvider : IVATPayer
    {
        public string CountryCode { get; set; }

        public string VAT { get; set; }
        public virtual bool IsVATPayer()
        {
            return !string.IsNullOrEmpty(VAT);
        }
    }
}
