using ApplicationCore.Domain.Entities.Room;

namespace Application.UnitTests.DomainTests.Rooms
{
    public class RoomAvailabilityTests
    {
        [Fact]
        public void GetPrice_WhenNoMatchingPricePeriod_ReturnsZero()
        {
            // Arrange
            var roomAvailability = new RoomAvailability
            {
                PricePeriods = new List<RoomPricePeriod>
                {
                    new RoomPricePeriod
                    {
                        Price = 50, // A different price period with different dates
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(2)
                    }
                }
            };

            // Act
            var price = roomAvailability.GetPrice();

            // Assert
            Assert.Equal(0, price);
        }

        [Fact]
        public void GetPrice_WhenMatchingPricePeriod_ReturnsPrice()
        {
            // Arrange
            var roomAvailability = new RoomAvailability
            {
                PricePeriods = new List<RoomPricePeriod>
                {
                    new RoomPricePeriod
                    {
                        Price = 50,
                        StartDate = DateTime.Now.AddMonths(-1), // A matching price period
                        EndDate = DateTime.Now.AddMonths(1)
                    }
                }
            };

            // Act
            var price = roomAvailability.GetPrice();

            // Assert
            Assert.Equal(50, price);
        }

        [Fact]
        public void GetPrice_WhenNoPricePeriods_ReturnsZero()
        {
            // Arrange
            var roomAvailability = new RoomAvailability
            {
                PricePeriods = new List<RoomPricePeriod>() // No price periods
            };

            // Act
            decimal price = roomAvailability.GetPrice();

            // Assert
            Assert.Equal(0, price);
        }

        [Fact]
        public void GetPrice_WhenMultipleMatchingPricePeriods_ReturnsMatchingPrice()
        {
            // Arrange
            var roomAvailability = new RoomAvailability
            {
                PricePeriods = new List<RoomPricePeriod>
                {
                    new RoomPricePeriod
                    {
                        Price = 60,
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(2)
                    },
                    new RoomPricePeriod
                    {
                        Price = 50,
                        StartDate = DateTime.Now.AddMonths(-1),
                        EndDate = DateTime.Now.AddMonths(1)
                    },
                }
            };

            // Act
            var price = roomAvailability.GetPrice();

            // Assert
            Assert.Equal(50, price);
        }

        [Fact]
        public void GetPrice_WhenPricePeriodStartDateIsFutureDate_ReturnsZero()
        {
            // Arrange
            var roomAvailability = new RoomAvailability
            {
                PricePeriods = new List<RoomPricePeriod>
                {
                    new RoomPricePeriod
                    {
                        Price = 50,
                        StartDate = DateTime.Now.AddMonths(1), // A future start date
                        EndDate = DateTime.Now.AddMonths(2)
                    }
                }
            };

            // Act
            var price = roomAvailability.GetPrice();

            // Assert
            Assert.Equal(0, price);
        }

        [Fact]
        public void GetPrice_WhenPricePeriodEndDateIsPastDate_ReturnsZero()
        {
            // Arrange
            var roomAvailability = new RoomAvailability
            {
                PricePeriods = new List<RoomPricePeriod>
                {
                    new RoomPricePeriod
                    {
                        Price = 50,
                        StartDate = DateTime.Now.AddMonths(-2),
                        EndDate = DateTime.Now.AddMonths(-1)
                    }
                }
            };

            // Act
            var price = roomAvailability.GetPrice();

            // Assert
            Assert.Equal(0, price);
        }
    }
}
