namespace Tadget
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Menu : MonoBehaviour {

		public Room room;

		private void Awake() 
		{
			room = GetComponent<Room>();
			Debug.Assert(room != null);
		}

		private void Start()
		{
			//room.Join("Tadas", false, true);
            //room.Join("Aldona", true, true);

            if (room.Play())
            {
                Debug.LogFormat("[MENU] Pressed play. Game started!");
            }
            else
            {
                Debug.LogFormat("[MENU] Pressed play. Game unable to start.");
            }
        }

	}
}
