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


        public async Task<DataFile> GetJsonData(string search)
        {
            DataFile dataFile = new DataFile();
            //return  DataFile_dataFile;

            //search in building array
            var buildingResult = SearchBuildingResult(search);
            var buildingsResult = buildingResult.Where(q => q.weight > 0).OrderByDescending(build => build.weight).ToArray();
            dataFile.buildings = buildingsResult;


            //search in locks array

            var lockResult = SearchLocksResult(search, DataFile_dataFile.buildings);
            var locksResult = lockResult.Where(q => q.weight > 0).OrderByDescending(lockey => lockey.weight).ToArray();
            dataFile.locks = locksResult;


            return dataFile;

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

                    var property = prop.GetValue(building, null);

                    if (prop.Name == "shortCut" && search.ToLower().Equals(property.ToString().ToLower()))  // property.ToString().ToLower().Equals(search))
                    {
                        buildings.shortCut = property.ToString();
                        buildings.weight += 7 * 10;
                        continue;
                    }
                    else
                    {
                        if (prop.Name == "shortCut" && property.ToString().ToLower().Contains(search.ToLower()))
                        {
                            buildings.shortCut = property.ToString();
                            buildings.weight += 7;
                            continue;
                        }   
                    }

                    if (prop.Name == "name" && search.Equals(property.ToString()))
                    {
                        buildings.name = property.ToString();
                        buildings.weight += 9 * 10;
                        continue;
                    }
                    else
                    {

                        if (prop.Name == "name" && property.ToString().ToLower().Contains(search.ToLower()))
                        {
                            buildings.name = property.ToString();
                            buildings.weight += 9;
                            continue;
                        }
                        //buildings.name = property.ToString();

                    }

                    if (property != null)
                    {
                        if (prop.Name == "description" && search.Equals(property.ToString()))
                        {
                            buildings.description = property.ToString();
                            buildings.weight += 5 * 10;
                            continue;
                        }
                        else
                        {
                            if (prop.Name == "description" && property.ToString().ToLower().Contains(search.ToLower()))
                            {
                                buildings.description = property.ToString();
                                buildings.weight += 5;
                                continue;
                            }
                            // buildings.description = property.ToString();
                        }
                    }

                }

                Listbuildings.Add(buildings);
            }

            return Listbuildings;
        }


        public List<Locks> SearchLocksResult(string search, Buildings[] buildings)
        {

            List<Locks> Listlocks = new List<Locks>();

            foreach (var locks in DataFile_dataFile.locks)
            {
                var lockObj = new Locks();

                foreach (PropertyInfo prop in locks.GetType().GetProperties())
                {
                    if (prop.Name == "id")
                        continue;

                    var property = prop.GetValue(locks, null);

                    if (property != null)
                    {

                        //for nevigation object  building id
                        if (prop.Name == "buildingId")//&& search.ToLower().Equals(property.ToString().ToLower()))
                        {
                            var buildingItem = new Buildings();

                            var buildinglist = buildings.Where(id => id.id == property.ToString()).ToList();

                            foreach (var buildingItem1 in buildinglist)
                            {
                                foreach (PropertyInfo prop1 in buildingItem.GetType().GetProperties())
                                {
                                    if (prop1.Name == "id")
                                        continue;

                                    var propertyItem = prop1.GetValue(buildingItem1, null);

                                    if (prop1.Name == "shortCut" && search.ToLower().Equals(propertyItem.ToString().ToLower()))  // property.ToString().ToLower().Equals(search))
                                    {
                                        buildingItem.shortCut = propertyItem.ToString();
                                        buildingItem.weight += 5 * 10;
                                        continue;
                                    }
                                    else
                                    {
                                        if (prop1.Name == "shortCut" && propertyItem.ToString().ToLower().Contains(search.ToLower()))
                                        {
                                            buildingItem.shortCut = propertyItem.ToString();
                                            buildingItem.weight += 5;
                                            continue;
                                        }
                                    }

                                    if (prop1.Name == "name" && search.Equals(propertyItem.ToString()))
                                    {
                                        buildingItem.name = propertyItem.ToString();
                                        buildingItem.weight += 8 * 10;
                                        continue;
                                    }
                                    else
                                    {

                                        if (prop1.Name == "name" && propertyItem.ToString().ToLower().Contains(search.ToLower()))
                                        {
                                            buildingItem.name = propertyItem.ToString();
                                            buildingItem.weight += 8;
                                            continue;
                                        }
                                    }

                                    if (prop1.Name == "description" && search.Equals(propertyItem.ToString()))
                                    {
                                        buildingItem.description = propertyItem.ToString();
                                        buildingItem.weight += 0;
                                        continue;
                                    }
                                    else
                                    {
                                        if (prop1.Name == "description" && propertyItem.ToString().ToLower().Contains(search.ToLower()))
                                        {
                                            buildingItem.description = propertyItem.ToString();
                                            buildingItem.weight += 0;
                                            continue;
                                        }
                                    }
                                }
                            }

                            lockObj.building_wt = buildingItem.weight;
                        }

                        //type
                        if (prop.Name == "type" && search.Equals(property.ToString()))
                        {
                            lockObj.type = property.ToString();
                            lockObj.weight += 3 * 10;
                            continue;
                        }
                        else
                        {
                            if (prop.Name == "type" && property.ToString().ToLower().Contains(search.ToLower()))
                            {
                                lockObj.type = property.ToString();
                                lockObj.weight += 3;
                                continue;
                            }
                        }

                        //name
                        if (prop.Name == "name" && search.Equals(property.ToString()))
                        {
                            lockObj.name = property.ToString();
                            lockObj.weight += 10 * 10;
                            continue;
                        }
                        else
                        {
                            if (prop.Name == "name" && property.ToString().ToLower().Contains(search.ToLower()))
                            {
                                lockObj.name = property.ToString();
                                lockObj.weight += 10;
                                continue;
                            }
                        }

                        //serialNumber
                        if (prop.Name == "serialNumber" && search.Equals(property.ToString()))
                        {
                            lockObj.serialNumber = property.ToString();
                            lockObj.weight += 8 * 10;
                            continue;
                        }
                        else
                        {
                            if (prop.Name == "serialNumber" && property.ToString().ToLower().Contains(search.ToLower()))
                            {
                                lockObj.serialNumber = property.ToString();
                                lockObj.weight += 8;
                                continue;
                            }
                        }

                        //floor
                        if (property != null)
                        {
                            if (prop.Name == "floor" && search.Equals(property.ToString()))
                            {
                                lockObj.serialNumber = property.ToString();
                                lockObj.weight += 6 * 10;
                                continue;
                            }
                            else
                            {
                                if (prop.Name == "floor" && property.ToString().ToLower().Contains(search.ToLower()))
                                {
                                    lockObj.serialNumber = property.ToString();
                                    lockObj.weight += 6;
                                    continue;
                                }
                            }
                        }

                        //roomNumber
                        if (prop.Name == "roomNumber" && search.Equals(property.ToString()))
                        {
                            lockObj.roomNumber = property.ToString();
                            lockObj.weight += 6 * 10;
                            continue;
                        }
                        else
                        {
                            if (prop.Name == "roomNumber" && property.ToString().ToLower().Contains(search.ToLower()))
                            {
                                lockObj.roomNumber = property.ToString();
                                lockObj.weight += 6;
                                continue;
                            }
                        }

                        //description
                        if (property != null)
                        {
                            if (prop.Name == "description" && search.Equals(property.ToString()))
                            {
                                lockObj.description = property.ToString();
                                lockObj.weight += 6 * 10;
                                continue;
                            }
                            else
                            {
                                if (prop.Name == "description" && property.ToString().ToLower().Contains(search.ToLower()))
                                {
                                    lockObj.description = property.ToString();
                                    lockObj.weight += 6;
                                    continue;
                                }
                            }
                        }
                    }

                }

                Listlocks.Add(lockObj);
            }

            return Listlocks;
        }

    }


    public class Search
    {
        public string searchName { get; set; }

    }
}
