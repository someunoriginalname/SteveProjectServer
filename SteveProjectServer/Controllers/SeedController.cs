using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameModel.Models;
using OfficeOpenXml;
using System.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GameModel.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly GamesContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<PublisherUser> _userManager;
        public SeedController(
            GamesContext context,
            IWebHostEnvironment env,
            UserManager<PublisherUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<ActionResult> Import()
        {
            var path = Path.Combine(
                _env.ContentRootPath,
                "Data/gamedata.xlsx");
            using var stream = System.IO.File.OpenRead(path);
            using var excelPackage = new ExcelPackage(stream);
            // get the first worksheet 
            var worksheet = excelPackage.Workbook.Worksheets[0];
            // define how many rows we want to process 
            var nEndRow = worksheet.Dimension.End.Row;
            // initialize the record counters 
            var numberOfPublishersAdded = 0;
            var numberOfGamesAdded = 0;
            // create a lookup dictionary 
            // containing all the countries already existing 
            // into the Database (it will be empty on first run).
            var publishersByName = _context.Publishers
                .AsNoTracking()
                .ToDictionary(x => x.PublisherName, StringComparer.OrdinalIgnoreCase);
            // iterates through all rows, skipping the first one 
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[
                    nRow, 1, nRow, worksheet.Dimension.End.Column];
                var publisherName = row[nRow, 3].GetValue<string>();
                var publisherYear = row[nRow, 8].GetValue<int>();
                // skip this publisher if it already exists in the database
                if (publishersByName.ContainsKey(publisherName))
                    continue;
                // create the Publisher entity and fill it with xlsx data 
                var publisher = new Publisher
                {
                    PublisherName = publisherName,
                    PublisherYear = publisherYear
                };
                // add the new publisher to the DB context 
                await _context.Publishers.AddAsync(publisher);
                // store the publisher in our lookup to retrieve its Id later on
                publishersByName.Add(publisherName, publisher);
                // increment the counter 
                numberOfPublishersAdded++;
            }
            // save all the countries into the Database 
            if (numberOfPublishersAdded > 0)
                await _context.SaveChangesAsync();
            // create a lookup dictionary
            // containing all the games already existing 
            // into the Database (it will be empty on first run). 
            
            var games = _context.Games
                .AsNoTracking()
                .ToDictionary(x => (
                    AppId: x.AppId,
                    GameName: x.GameName,
                    Developer: x.Developer,
                    Year: x.Year,
                    Players: x.Players,
                    Price: x.Price,
                    Revenue: x.Revenue,
                    PublisherId: x.PublisherId));
            // iterates through all rows, skipping the first one 
            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[
                    nRow, 1, nRow, worksheet.Dimension.End.Column];
                var name = row[nRow, 1].GetValue<string>();
                var developer = row[nRow, 2].GetValue<string>();
                var year = row[nRow, 4].GetValue<int>();
                var players = row[nRow, 5].GetValue<int>();
                var publisherName = row[nRow, 3].GetValue<string>();
                var appid = row[nRow, 6].GetValue<int>();
                var price = row[nRow, 7].GetValue<decimal>();
                var revenue = price * players;
                // retrieve publisher Id by publisherName
                var publisherId = publishersByName[publisherName].PublisherId;
               
                // skip this game if it already exists in the database
                if (!games.ContainsKey((
                    AppId: appid,
                    GameName: name,
                    Developer: developer,
                    Year: year,
                    Players: players,
                    Price: price,
                    Revenue: revenue,
                    PublisherId: publisherId
                    )))
                {
                    // create the City entity and fill it with xlsx data 
                    var game = new Game
                    {
                        AppId = appid,
                        GameName = name,
                        Developer = developer,
                        Year = year,
                        Players = players,
                        Price = price,
                        Revenue = revenue,
                        PublisherId = publisherId
                    };
                    // add the new game to the DB context 
                    _context.Games.Add(game);
                    // increment the counter
                    numberOfGamesAdded++;
                }
            }
            // save all the games into the Database 
            if (numberOfGamesAdded > 0)
                await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                Games = numberOfGamesAdded,
                Publishers = numberOfPublishersAdded
            });
        }
        [HttpPost("User")]
        public async Task<ActionResult> SeedUser()
        {
            (string name, string email) = ("user1", "comp584@csun.edu");
            PublisherUser user = new()
            {
                UserName = name,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            if (await _userManager.FindByNameAsync(name) is not null)
            {
                user.UserName = "user2";
            }
            _ = await _userManager.CreateAsync(user, "P@ssw0rd!")
                ?? throw new InvalidOperationException();
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}