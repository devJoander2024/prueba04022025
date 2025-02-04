using Microsoft.AspNetCore.Mvc;
using WebApplication2.Controllers;
using WebApplication2.Dtos;
using WebApplication2.Services;

namespace WebApplication2TestProject
{
    public class UnitTestCountries
    {
        private readonly CountriesServices _service;

        [Fact]
        public void TestGetAllCountries()
        {
            // Arrange
            CountriesController controller = new CountriesController(_service);

            // Act
            var result = controller.GetAll() as Object;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void TestGetCountryByCode()
        {
            // Arrange
            CountriesController controller = new CountriesController(_service);

            string isoCode = "US";

            // Act
            Task<Countries> result = controller.GetByCode(isoCode);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(result.Result.name, "Washington");

        }
    }
}