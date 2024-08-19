namespace MonsterQuest
{
    public class Character
    {
        // PROPERTIES
        public string DisplayName { get; private set; }

        // CONSTRUCTORS
        public Character(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
