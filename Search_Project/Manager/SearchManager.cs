using System.Text.Json;
using System;
using Search_Project.Models;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Linq;

namespace Search_Project.Manager
{
    public class SearchManager
    {

        private DataFile DataFile_dataFile;

        public SearchManager()
        {
           // LoadJsonData();
        }


        public void LoadJsonData()
        {


            string text = File.ReadAllText(@"./Data/sv_lsm_data.json");


            DataFile_dataFile = JsonSerializer.Deserialize<DataFile>(text);


        }


        public async Task<DataFile> GetJsonData()
        {
            DataFile dataFile = new DataFile();
            //return  DataFile_dataFile;
            var buildingResult = SearchBuildingResult("PROD");

            var buildingsResult = buildingResult.Where(q => q.weight > 0).OrderByDescending(person => person.weight).ToArray();


            dataFile.buildings = buildingsResult;

            return DataFile_dataFile;

        }


        public List<Buildings> SearchBuildingResult(string search)
        {
            List<Buildings> Listbuildings = new List<Buildings>();

            foreach (var building in DataFile_dataFile.buildings)
            {
                var buildings = new Buildings();

                foreach (PropertyInfo prop in building.GetType().GetProperties())
                {
                    if (prop.Name == "id")
                        continue;

                    var property =  prop.GetValue(building, null);

                    if (prop.Name == "shortCut" && search.Equals(property.ToString()))  // property.ToString().ToLower().Equals(search))
                    {
                        buildings.shortCut = property.ToString();
                        buildings.weight += 7 * 10;
                    }
                    else
                    {
                        if (search.Contains(property.ToString()))
                        {
                            buildings.weight += 7;
                        }
                    }

                    if (prop.Name == "name" && property == search)
                    {
                        buildings.name = property.ToString();
                        buildings.weight += 9 * 10;
                    }
                    else
                    {
                        if (search.Contains(property.ToString()))
                        {
                            buildings.weight += 9;
                        }
                    }

                    if (prop.Name == "description" && property == search)
                    {
                        buildings.description = property.ToString();
                        buildings.weight += 5 * 10;
                    }
                    else
                    {
                        if (search.Contains(property.ToString()))
                        {
                            buildings.weight += 5;
                        }
                    }

                }





                Listbuildings.Add(buildings);
            }





            return Listbuildings;
        }





    }
}
