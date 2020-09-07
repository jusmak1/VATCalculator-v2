using System;

namespace PresentConnection.VATCalculator
{
    public class Customer : IVATPayer
    {
        public string CountryCode { get; set; }

        public string VAT { get; set; }

        public virtual bool IsVATPayer()
        {
            return !String.IsNullOrEmpty(VAT);
        }
    }
}
