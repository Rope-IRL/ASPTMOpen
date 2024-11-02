using ASPTM.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;

namespace ASPTM.Middleware.Initializers
{
    public class DatabaseInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializerMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<RealestaterentalContext>();
                await InitializeDatabaseAsync(db);
            }

            await _next(context);
        }

        private async Task InitializeDatabaseAsync(RealestaterentalContext db)
        {
            db.Database.EnsureCreated();

            if (db.Flats.Any())
            {
                return;
            }

            Random rand = new Random();
            int numberOfLessees = 5;
            int numberOfLandLords = 5;
            int numberOfFlats = 10;
            int numberOfFlatsContracts = 20;

            for (int i = 1; i < numberOfLessees+1; i++)
            {
                string name = $"name {i}";
                string surname = $"surName {i}";

                long startNumber = 376000000000;
                long endNumber = 376999999999;
                string telephone = Convert.ToString((rand.NextInt64(startNumber, endNumber)));

                DateOnly startDate = new DateOnly(1980, 12, 11);
                DateOnly endDate = new DateOnly(2020, 12, 11);
                int range = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;
                int randomDays = rand.Next(range + 1);

                DateOnly birthDate = (startDate.AddDays(randomDays));

                long startPassportNumber = 000000000000;
                long endPassportNumber = 999999999999;
                string pasportId = Convert.ToString((rand.NextInt64(startPassportNumber, endPassportNumber)));


                int integerPart = rand.Next(0, 9);

                // Generate the fractional part (two digits between 0 and 99)
                int fractionalPart = rand.Next(0, 99);

                // Combine both parts into a decimal with two decimal places
                decimal result = integerPart + fractionalPart / 100.0m;

                decimal lAvgMark = Math.Round(result);

                LesseesAdditionalInfo lesseesAdditionalInfo = new LesseesAdditionalInfo
                {
                    Lid = i,
                    AvgMark = lAvgMark,
                    BirthDate = birthDate,
                    Name = name,
                    Surname = surname,
                    Telephone = telephone,
                    PassportId = pasportId,
                };
                string email = $"email {i}";
                string login = $"login {i}";
                string password = $"password {i}";

                db.Lessees.Add(new Lessee { Login = login, Password = password, Email = email, LesseesAdditionalInfo = lesseesAdditionalInfo });
                await db.SaveChangesAsync();


            }

            for (int i = 1; i < numberOfLandLords+1; i++)
            {
                string name = $"name {i}";
                string surname = $"surName {i}";

                long startNumber = 376000000000;
                long endNumber = 376999999999;
                string telephone = Convert.ToString((rand.NextInt64(startNumber, endNumber)));

                DateOnly startDate = new DateOnly(1980, 12, 11);
                DateOnly endDate = new DateOnly(2020, 12, 11);
                int range = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;
                int randomDays = rand.Next(range + 1);
                DateOnly birthDate = (startDate.AddDays(randomDays));

                long startPassportNumber = 000000000000;
                long endPassportNumber = 999999999999;
                string pasportId = Convert.ToString((rand.NextInt64(startPassportNumber, endPassportNumber)));


                int integerPart = rand.Next(0, 9);

                // Generate the fractional part (two digits between 0 and 99)
                int fractionalPart = rand.Next(0, 99);

                // Combine both parts into a decimal with two decimal places
                decimal result = integerPart + fractionalPart / 100.0m;

                decimal lAvgMark = Math.Round(result);

                LandLordsAdditionalInfo lesseesAdditionalInfo = new LandLordsAdditionalInfo
                {
                    Llid = i,
                    AvgMark = lAvgMark,
                    BirthDate = birthDate,
                    Name = name,
                    Surname = surname,
                    Telephone = telephone,
                    PassportId = pasportId,
                };
                string email = $"email {i}";
                string login = $"login {i}";
                string password = $"password {i}";

                db.LandLords.Add(new LandLord { Login = login, Password = password, Email = email, LandLordsAdditionalInfo = lesseesAdditionalInfo });
                await db.SaveChangesAsync();


            }
            
            for (int i = 1; i < numberOfFlats + 1; i++)
            {
                string header = $"flat {i}";
                string description = $"description of the flat{i}";
                decimal avgMark = (decimal)9.9;
                string city = $"city {i}";
                string address = $"address{i}";
                short numberOfRooms = (short)rand.Next(1, 3);
                short numberOfFloors = (short)rand.Next(1, 3);
                bool bathRoomAvailability = Convert.ToBoolean(rand.Next(0, 1));
                bool wifiAvailability = Convert.ToBoolean(rand.Next(0, 1));

                int integerPart = rand.Next(0, 9);

                // Generate the fractional part (two digits between 0 and 99)
                int fractionalPart = rand.Next(0, 99);

                // Combine both parts into a decimal with two decimal places
                decimal result = integerPart + fractionalPart / 100.0m;

                decimal cost = Math.Round(result, 2);

                db.Flats.Add(new Flat
                {
                    Header = header,
                    Description = description,
                    Address = address,
                    City = city,
                    AvgMark = avgMark,
                    WiFiAvailability = wifiAvailability,
                    BathroomAvailability = bathRoomAvailability,
                    NumberOfFloors = numberOfFloors,
                    NumberOfRooms = numberOfRooms,
                    CostPerDay = cost
                });
                await db.SaveChangesAsync();

            }
            for (int i = 1; i < numberOfFlatsContracts + 1; i++)
            {
                DateOnly startDate = new DateOnly(2020, 1, 11);
                DateOnly endDate = new DateOnly(2020, 12, 11);
                int range = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days;
                int randomDays = rand.Next(range+1);
                DateOnly rentStartDate = startDate.AddDays(randomDays);
                int rentRange = rand.Next(randomDays, range);
                DateOnly rentEndDate = startDate.AddDays(rentRange);

                int Fid = rand.Next(1, numberOfFlats+1);
                int Lid = rand.Next(1, numberOfLessees+1);
                int LLid = rand.Next(1, numberOfLandLords);

                int integerPart = rand.Next(0, 9);

                // Generate the fractional part (two digits between 0 and 99)
                int fractionalPart = rand.Next(0, 99);

                // Combine both parts into a decimal with two decimal places
                decimal result = integerPart + fractionalPart / 100.0m;

                decimal cost = Math.Round(result, 2);
                db.FlatsContracts.Add(new FlatsContract {
                    StartDate = rentStartDate,
                    EndDate = rentEndDate,
                    Fid = Fid,
                    Lid = Lid,
                    Llid = LLid,
                    Cost = cost,
                });
                await db.SaveChangesAsync();

            }



        }
    }
}
