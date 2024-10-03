using System.Collections;

namespace MonsterQuest
{
    public partial class CreaturePresenter
    {
        // Public methods

        public IEnumerator PerformAbilityCheck(bool success, int rollResult)
        {
            yield return _dicePresenter.RollD20(transform.position, rollResult, success);
        }
    }
}
