namespace Tadget
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Settings", menuName = "Game/Settings", order = 1)]
    public class GameSettings : ScriptableObject {

        public int drawGameStartCardCount = 7;
        public int drawWildCardCount = 4;
        public int drawPlusTwoCardCount = 2;
        public float timePerTurn = 10;
        public float afterTurnDelay = 0;
        public float cardDealDelay = 0.5f;
    }
}
