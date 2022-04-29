using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Entities.DTO;
using VacationRental.Entities.DTO.Booking;
using VacationRental.Entities.DTO.Calendar;
using VacationRental.Entities.DTO.Rental;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class CalendarControllerTests
    {
        private readonly HttpClient _client;

        public CalendarControllerTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendarWhithoutPreparationTime_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2
            };

            ResourceIdViewModel postRentalResult = new ResourceIdViewModel();
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = DateTime.Now.Date
            };

            ResourceIdViewModel postBooking1Result;
            
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }
            

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.Now.Date.AddDays(1)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            int nights = 5;
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={DateTime.Now.Date:yyyy-MM-dd}&nights={nights}"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(nights, getCalendarResult.Dates.Count);

                Assert.Equal(DateTime.Now.Date, getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Contains(getCalendarResult.Dates[0].Bookings, x => x.Unit == 1);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(DateTime.Now.Date.AddDays(1), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Unit == 1);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Unit == 2);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(DateTime.Now.Date.AddDays(2), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Unit == 2);
                Assert.Empty(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(DateTime.Now.Date.AddDays(3), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);

                Assert.Equal(DateTime.Now.Date.AddDays(4), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Empty(getCalendarResult.Dates[4].PreparationTimes);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendarWhithPreparationTime_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult = new ResourceIdViewModel();
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.Now.Date
            };

            ResourceIdViewModel postBooking1Result;

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }


            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = DateTime.Now.Date.AddDays(1)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            int nights = 5;
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start={DateTime.Now.Date:yyyy-MM-dd}&nights={nights}"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(nights, getCalendarResult.Dates.Count);

                Assert.Equal(DateTime.Now.Date, getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Contains(getCalendarResult.Dates[0].Bookings, x => x.Unit == 1);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(DateTime.Now.Date.AddDays(1), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Unit == 1);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Unit == 2);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(DateTime.Now.Date.AddDays(2), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Unit == 2);
                Assert.Contains(getCalendarResult.Dates[2].PreparationTimes, x => x.Unit == 1);

                Assert.Equal(DateTime.Now.Date.AddDays(3), getCalendarResult.Dates[3].Date);
                Assert.Empty(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].PreparationTimes, x => x.Unit == 2);

                Assert.Equal(DateTime.Now.Date.AddDays(4), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Empty(getCalendarResult.Dates[4].PreparationTimes);
            }
        }
    }
}
