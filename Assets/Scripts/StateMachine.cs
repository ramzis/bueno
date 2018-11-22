namespace Tadget
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "State", menuName = "Game/State Machine", order = 1)]
    public class StateMachine : ScriptableObject {

        [SerializeField] protected string state;
        [SerializeField] protected string pendingState;
        [SerializeField] protected string lastState;
        [SerializeField] protected bool autoStateChange = true;

		public void Set(string state)
        {
            lastState = this.state;
            if (autoStateChange)
            {
                this.state = state;
                pendingState = "";
            }
            else
            {
                this.state = "STATE_Null";
                pendingState = state;
            }
        }

		public string Get()
		{
			return state;
		}
    }
}
