using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameModel;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;

namespace SteveProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(GameContext db, IHostEnvironment environment) : ControllerBase
    {
        private readonly string _pathName = Path.Combine(environment.ContentRootPath, "Data/GameData.csv");

        [HttpPost("Game")]
        public async Task<ActionResult<Game>> SeedGame()
        {
            Dictionary<string, Publisher> countries = await db.Publishers//.AsNoTracking()
             .ToDictionaryAsync(c => c.PublisherName);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };
            int sold = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<GameData>? records = csv.GetRecords<GameData>();
                foreach (GameData record in records)
                {
                    if (!countries.TryGetValue(record.publisher, out Publisher? value))
                    {
                        Console.WriteLine($"Not found publisher for {record.game}");
                        return NotFound(record);
                    }

                    if (!record.players.HasValue || string.IsNullOrEmpty(record.game))
                    {
                        Console.WriteLine($"Skipping {record.game}");
                        continue;
                    }
                    Game game = new()
                    {
                        GameName = record.game,
                        Developer = record.developer,
                        Year = (int)record.year,
                        Players = (int)record.players,
                        AppID = (int)record.appid,
                        PublisherID = value.PublisherID
                    };
                    db.Games.Add(game);
                    sold++;
                }
                await db.SaveChangesAsync();
            }
            return new JsonResult(sold);
        }

        [HttpPost("Publisher")]
        public async Task<ActionResult<Game>> SeedCountry()
        {
            // create a lookup dictionary containing all the countries already existing 
            // into the Database (it will be empty on first run).
            Dictionary<string, Publisher> publishersByName = db.Publishers.AsNoTracking().ToDictionary(x => x.PublisherName, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<GameData> records = csv.GetRecords<GameData>().ToList();
            foreach (GameData record in records)
            {
                if (publishersByName.ContainsKey(record.publisher))
                {
                    continue;
                }

                Publisher publisher = new()
                {
                    PublisherName = record.publisher,
                };
                await db.Publishers.AddAsync(publisher);
                publishersByName.Add(record.publisher, publisher);
            }

            await db.SaveChangesAsync();

            return new JsonResult(publishersByName.Count);
        }
    }
}
