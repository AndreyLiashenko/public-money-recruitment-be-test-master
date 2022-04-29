using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Booking;
using VacationRental.Entities.DTO.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class BookingControllerTests
    {
        private readonly HttpClient _client;

        public BookingControllerTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetNotExistsBooking_ThenAGetReturnsNotFound()
        {
            int id = -1;
            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{id}"))
            {
                Assert.False(getBookingResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, getBookingResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 0
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 3,
                 Start = DateTime.Now.Date
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBookingWithNegativeNights_ThenAPostReturnsBadRequest()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 0
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = -3,
                Start = DateTime.Now.Date
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.False(postBookingResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postBookingResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBookingWithNotExistsRental_ThenAPostReturnsNotFound()
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = -1,
                Nights = 3,
                Start = DateTime.Now.Date
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.False(postBookingResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, postBookingResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBookingWithDateLessThenNow_ThenAPostReturnsBadRequest()
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = 1,
                Nights = 3,
                Start = DateTime.Now.Date.AddDays(-1)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.False(postBookingResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postBookingResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBookingWhithoutPreparationTime_ThenAPostReturnsBadRequestThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = DateTime.Now.Date
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.Now.Date.AddDays(1)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.False(postBooking2Response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postBooking2Response.StatusCode);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBookingWhithPreparationTime_ThenAPostReturnsBadRequestThereIsOverbooking()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1,
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            int nights = 3;
            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = nights,
                Start = DateTime.Now.Date
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = DateTime.Now.Date.AddDays(nights)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.False(postBooking2Response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postBooking2Response.StatusCode);
            }
        }
    }
}
