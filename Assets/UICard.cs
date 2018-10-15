namespace Tadget
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;

    public class UICard : MonoBehaviour {

		[SerializeField] private Image back;
		[SerializeField] private Text text;
		public Card.Type type;
		public Card.Color color;

		public void Init(Card.Type type, Card.Color color) 
		{
			this.type = type;
			this.color = color;
		}
	}
}
