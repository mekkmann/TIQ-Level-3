using System.Collections.Generic;
using System.Linq;

namespace MonsterQuest
{
    public class Party
    {
        // PROPERTIES
        public List<Character> Characters { get; private set; }

        // CONTRUCTORS
        public Party(IEnumerable<Character> initialCharacters)
        {
            Characters = initialCharacters.ToList();
        }
    }
}
