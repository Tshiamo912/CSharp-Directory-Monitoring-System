using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fitness_Planner
{
    // Class: FileManager
    // Handles saving, loading, updating and removing objects from a file
    public static class FileManager<T> where T : ISerializableEntity<T>, new()
    {
        // Method: Load
        // Loads objects from file into a list
        public static List<T> Load(string filePath)
        {
            if (!File.Exists(filePath)) return new List<T>();

            return File.ReadAllLines(filePath)
                       .Select(line => new T().Deserialize(line))
                       .Where(obj => obj != null)
                       .ToList();
        }

        // Method: Save
        // Saves a single object to file
        public static void Save(string filePath, T entity)
        {
            File.AppendAllLines(filePath, new[] { entity.Serialize() });
        }

        // Method: Update
        // Updates an existing object in the file
        public static void Update(string filePath, Func<T, bool> matchPredicate, T updatedEntity)
        {
            var items = Load(filePath);

            var existing = items.FirstOrDefault(matchPredicate);
            if (existing != null)
            {
                items.Remove(existing);
                items.Add(updatedEntity);

                File.WriteAllLines(filePath, items.Select(i => i.Serialize()));
            }
        }

        // Method: Remove
        // Removes objects from file that match a condition
        public static void Remove(string filePath, Predicate<T> predicate)
        {
            var items = Load(filePath);
            int removedCount = items.RemoveAll(predicate);

            File.WriteAllLines(filePath, items.Select(i => i.Serialize()));
            Console.WriteLine($"{removedCount} item(s) removed successfully.");
        }
    }
}
