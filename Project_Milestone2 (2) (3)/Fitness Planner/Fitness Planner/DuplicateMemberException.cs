using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Planner
{
    //creating a custom exception
    //To ensure a new member cannot be added because their profile already exists
    internal class DuplicateMemberException:Exception
    {
        public DuplicateMemberException(string name):base($"Member {name} already exists.")
        {
        }
    }
}
