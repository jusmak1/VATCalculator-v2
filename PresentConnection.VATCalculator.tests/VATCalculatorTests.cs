using NSubstitute;
using System;
using Xunit;

namespace PresentConnection.VATCalculator.tests
{
    public class VATCalculatorTests
    {

        private VATCalculator _VATCalculator;
        private Customer _customer;
        private ServiceProvider _serviceProvider;
        
        private void SubstituteInstances()
        {
            _VATCalculator = Substitute.For<VATCalculator>();
            _customer = Substitute.For<Customer>();
            _serviceProvider = Substitute.For<ServiceProvider>();

            _VATCalculator.EUService = Substitute.For<IEUService>();
            _VATCalculator.VATCountryService = Substitute.For<IVATCountryService>();

        }

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(1000)]
        public void ShouldReturnZeroIfServiceProviderIsNotVATPayerAndCustomerIsVATPayer(int amount)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(false);
            _customer.IsVATPayer().Returns(true);

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal(0, VAT);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(1000)]
        public void ShouldReturnZeroIfServiceProviderIsNotVATPayerAndCustomerIsNotVATPayer(int amount)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(false);
            _customer.IsVATPayer().Returns(false);

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal(0, VAT);
        }


        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(1000)]
        public void ShouldReturnZeroIfServiceProviderIsNotVATPayerAndBothLiveInTheSameCountry(int amount)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(false);
            _serviceProvider.CountryCode = "LT";
            _customer.CountryCode = "LT";

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal(0, VAT);
        }


        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(1000)]
        public void ShouldReturnZeroIfServiceProviderIsNotVATPayerAndBothLiveInTheDifferentCountry(int amount)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(false);
            _serviceProvider.CountryCode = "LT";
            _customer.CountryCode = "USA";

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal(0, VAT);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(50)]
        [InlineData(1000)]
        public void ShouldReturnZeroIfServiceProviderIsVATPayerAndClientIsOutsideEU(int amount)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(true);
            _serviceProvider.CountryCode = "LT";
            _customer.CountryCode = "USA";
            _VATCalculator.EUService.IsCountryInEU("USA").Returns(false);

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal(0, VAT);
        }

        [Theory]
        [InlineData(10, 21)]
        [InlineData(50, 21)]
        [InlineData(1000, 21)]
        public void ShouldReturnPercentOfAmountIfServiceProviderIsVATPayerAndClientIsInEUAndClientIsNotVATPayerAndBothLiveInDifferentCountries(int amount, int percent)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(true);
            _customer.IsVATPayer().Returns(false);
            _serviceProvider.CountryCode = "LT";
            _customer.CountryCode = "EE";
            _VATCalculator.EUService.IsCountryInEU("EE").Returns(true);
            _VATCalculator.VATCountryService.GetVATByCountryCode("LT").Returns(percent);

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal((percent / 100) * amount, VAT);
        }

        [Theory]
        [InlineData(10, 21)]
        [InlineData(50, 21)]
        [InlineData(1000, 21)]
        public void ShouldReturnZeroIfServiceProviderIsVATPayerAndClientIsInEUAndClientIsVATPayerAndBothLiveInDifferentCountries(int amount, int percent)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(true);
            _customer.IsVATPayer().Returns(true);
            _serviceProvider.CountryCode = "LT";
            _customer.CountryCode = "EE";
            _VATCalculator.EUService.IsCountryInEU("EE").Returns(true);
            _VATCalculator.VATCountryService.GetVATByCountryCode("LT").Returns(percent);

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal(0, VAT);
        }

        [Theory]
        [InlineData(10, 21)]
        [InlineData(50, 21)]
        [InlineData(1000, 21)]
        public void ShouldReturnZeroIfServiceProviderIsVATPayerAndClientIsInEUAndClientIsVATPayerAndBothLiveInSameCountry(int amount, int percent)
        {
            //Arange
            SubstituteInstances();
            _serviceProvider.IsVATPayer().Returns(true);
            _customer.IsVATPayer().Returns(true);
            _serviceProvider.CountryCode = "LT";
            _customer.CountryCode = "LT";
            _VATCalculator.EUService.IsCountryInEU("LT").Returns(true);
            _VATCalculator.VATCountryService.GetVATByCountryCode("LT").Returns(percent);

            //Act
            var VAT = _VATCalculator.CalculateVAT(_customer, _serviceProvider, amount);

            //Assert
            Assert.Equal((percent / 100) * amount, VAT);
        }

    }
}
