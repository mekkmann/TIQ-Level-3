using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class BeUnconsciousAction : IAction
    {
        private readonly Character _character;

        public BeUnconsciousAction(Character character)
        {
            _character = character;
        }

        public IEnumerator Execute()
        {
            yield return _character.HandleUnconsciousState();
        }
    }
}
