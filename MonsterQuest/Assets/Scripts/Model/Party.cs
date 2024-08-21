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

        // METHODS
        public void RemoveCharacter(Character character)
        {
            Characters.Remove(character);
        }
    }
}
