using Microsoft.AspNetCore.Mvc;
using Search_Project.Manager;
using Search_Project.Models;

namespace Search_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SearchManager _searchManager;

        SearchManager searchManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            // _searchManager = searchManager;

            searchManager = new SearchManager();

            LoadJsonData();
            
        }



        public void LoadJsonData()
        {

           

            searchManager.LoadJsonData();
        }


        [HttpGet]

        public async Task<DataFile> Get()
        {

          return await searchManager.GetJsonData();

        }




        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}