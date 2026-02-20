using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    
        // Defines serialization contract for saving/loading from file
        public interface ISerializableEntity<T>
        {
            string Serialize();
            T Deserialize(string line);
        }
    
}
