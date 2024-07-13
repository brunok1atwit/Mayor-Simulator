using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection _connection;

    void Awake()
    {
        string dbPath = $"{Application.persistentDataPath}/MayorSimulator.db";
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        CreateTables();
    }

    void CreateTables()
    {
        _connection.CreateTable<City>();
        _connection.CreateTable<Building>();
    }

    public class City
    {
        [PrimaryKey]
        public int Id { get; set; }  // Remove AutoIncrement
        public string Name { get; set; }
        public int Population { get; set; }
        public float Funds { get; set; }
    }

    public class Building
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public void SaveCity(City city)
    {
        var existingCity = _connection.Table<City>().FirstOrDefault(c => c.Id == city.Id);
        if (existingCity != null)
        {
            _connection.Update(city);
        }
        else
        {
            _connection.Insert(city);
        }
    }

    public void ClearBuildingsForCity(int cityId)
    {
        var buildingsToDelete = _connection.Table<Building>().Where(b => b.CityId == cityId).ToList();
        foreach (var building in buildingsToDelete)
        {
            _connection.Delete(building);
        }
    }

    public void SaveBuilding(Building building)
    {
        _connection.Insert(building);
    }

    public City GetCity(int cityId)
    {
        return _connection.Table<City>().FirstOrDefault(c => c.Id == cityId);
    }

    public List<Building> GetBuildings(int cityId)
    {
        return _connection.Table<Building>().Where(b => b.CityId == cityId).ToList();
    }
}
