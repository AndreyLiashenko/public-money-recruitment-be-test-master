using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class RentalControllerTests
    {
        private readonly HttpClient _client;

        public RentalControllerTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetNotExistsRental_ThenAGetReturnsNotFound()
        {
            int id = -1;
            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{id}"))
            {
                Assert.False(getBookingResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, getBookingResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
                Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRentalWithNegativeUnit_ThenAGetReturnsBadRequest()
        {
            var request = new RentalBindingModel
            {
                Units = -25,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.False(postResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRentalWithNegativePreparationTimeInDays_ThenAGetReturnsBadRequest()
        {
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = -1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.False(postResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
            }
        }
    }
}
