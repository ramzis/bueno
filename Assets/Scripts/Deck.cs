namespace Tadget
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
    using System.Linq;

	public class Deck : MonoBehaviour {

        [SerializeField] protected List<Card> draw;
        [SerializeField] protected List<Card> discard;

        protected void InitDraw()
        {
            /* Card generation in Python
            for times in range(2):
                for color in range(1,5):
                    for typ in ['_0', '_1', '_2', '_3', '_4', '_5', '_6', '_7', '_8', '_9', '_Skip', '_Reverse', '_DrawTwo', '_Wild', '_WildDrawFour']:
                        if not (times == 1 and typ in ['_0', '_WildDrawFour', '_Wild']):
                            print('Card.Create().(Card.Type.{}, Card.Color._{}),'.format(typ, color))
            */

            draw = new List<Card>()
            {
                Card.Create(Card.Type._0, Card.Color._1),
                Card.Create(Card.Type._1, Card.Color._1),
                Card.Create(Card.Type._2, Card.Color._1),
                Card.Create(Card.Type._3, Card.Color._1),
                Card.Create(Card.Type._4, Card.Color._1),
                Card.Create(Card.Type._5, Card.Color._1),
                Card.Create(Card.Type._6, Card.Color._1),
                Card.Create(Card.Type._7, Card.Color._1),
                Card.Create(Card.Type._8, Card.Color._1),
                Card.Create(Card.Type._9, Card.Color._1),
                Card.Create(Card.Type._Skip, Card.Color._1),
                Card.Create(Card.Type._Reverse, Card.Color._1),
                Card.Create(Card.Type._DrawTwo, Card.Color._1),
                Card.Create(Card.Type._Wild, Card.Color._1),
                Card.Create(Card.Type._WildDrawFour, Card.Color._1),
                Card.Create(Card.Type._0, Card.Color._2),
                Card.Create(Card.Type._1, Card.Color._2),
                Card.Create(Card.Type._2, Card.Color._2),
                Card.Create(Card.Type._3, Card.Color._2),
                Card.Create(Card.Type._4, Card.Color._2),
                Card.Create(Card.Type._5, Card.Color._2),
                Card.Create(Card.Type._6, Card.Color._2),
                Card.Create(Card.Type._7, Card.Color._2),
                Card.Create(Card.Type._8, Card.Color._2),
                Card.Create(Card.Type._9, Card.Color._2),
                Card.Create(Card.Type._Skip, Card.Color._2),
                Card.Create(Card.Type._Reverse, Card.Color._2),
                Card.Create(Card.Type._DrawTwo, Card.Color._2),
                Card.Create(Card.Type._Wild, Card.Color._2),
                Card.Create(Card.Type._WildDrawFour, Card.Color._2),
                Card.Create(Card.Type._0, Card.Color._3),
                Card.Create(Card.Type._1, Card.Color._3),
                Card.Create(Card.Type._2, Card.Color._3),
                Card.Create(Card.Type._3, Card.Color._3),
                Card.Create(Card.Type._4, Card.Color._3),
                Card.Create(Card.Type._5, Card.Color._3),
                Card.Create(Card.Type._6, Card.Color._3),
                Card.Create(Card.Type._7, Card.Color._3),
                Card.Create(Card.Type._8, Card.Color._3),
                Card.Create(Card.Type._9, Card.Color._3),
                Card.Create(Card.Type._Skip, Card.Color._3),
                Card.Create(Card.Type._Reverse, Card.Color._3),
                Card.Create(Card.Type._DrawTwo, Card.Color._3),
                Card.Create(Card.Type._Wild, Card.Color._3),
                Card.Create(Card.Type._WildDrawFour, Card.Color._3),
                Card.Create(Card.Type._0, Card.Color._4),
                Card.Create(Card.Type._1, Card.Color._4),
                Card.Create(Card.Type._2, Card.Color._4),
                Card.Create(Card.Type._3, Card.Color._4),
                Card.Create(Card.Type._4, Card.Color._4),
                Card.Create(Card.Type._5, Card.Color._4),
                Card.Create(Card.Type._6, Card.Color._4),
                Card.Create(Card.Type._7, Card.Color._4),
                Card.Create(Card.Type._8, Card.Color._4),
                Card.Create(Card.Type._9, Card.Color._4),
                Card.Create(Card.Type._Skip, Card.Color._4),
                Card.Create(Card.Type._Reverse, Card.Color._4),
                Card.Create(Card.Type._DrawTwo, Card.Color._4),
                Card.Create(Card.Type._Wild, Card.Color._4),
                Card.Create(Card.Type._WildDrawFour, Card.Color._4),
                Card.Create(Card.Type._1, Card.Color._1),
                Card.Create(Card.Type._2, Card.Color._1),
                Card.Create(Card.Type._3, Card.Color._1),
                Card.Create(Card.Type._4, Card.Color._1),
                Card.Create(Card.Type._5, Card.Color._1),
                Card.Create(Card.Type._6, Card.Color._1),
                Card.Create(Card.Type._7, Card.Color._1),
                Card.Create(Card.Type._8, Card.Color._1),
                Card.Create(Card.Type._9, Card.Color._1),
                Card.Create(Card.Type._Skip, Card.Color._1),
                Card.Create(Card.Type._Reverse, Card.Color._1),
                Card.Create(Card.Type._DrawTwo, Card.Color._1),
                Card.Create(Card.Type._1, Card.Color._2),
                Card.Create(Card.Type._2, Card.Color._2),
                Card.Create(Card.Type._3, Card.Color._2),
                Card.Create(Card.Type._4, Card.Color._2),
                Card.Create(Card.Type._5, Card.Color._2),
                Card.Create(Card.Type._6, Card.Color._2),
                Card.Create(Card.Type._7, Card.Color._2),
                Card.Create(Card.Type._8, Card.Color._2),
                Card.Create(Card.Type._9, Card.Color._2),
                Card.Create(Card.Type._Skip, Card.Color._2),
                Card.Create(Card.Type._Reverse, Card.Color._2),
                Card.Create(Card.Type._DrawTwo, Card.Color._2),
                Card.Create(Card.Type._1, Card.Color._3),
                Card.Create(Card.Type._2, Card.Color._3),
                Card.Create(Card.Type._3, Card.Color._3),
                Card.Create(Card.Type._4, Card.Color._3),
                Card.Create(Card.Type._5, Card.Color._3),
                Card.Create(Card.Type._6, Card.Color._3),
                Card.Create(Card.Type._7, Card.Color._3),
                Card.Create(Card.Type._8, Card.Color._3),
                Card.Create(Card.Type._9, Card.Color._3),
                Card.Create(Card.Type._Skip, Card.Color._3),
                Card.Create(Card.Type._Reverse, Card.Color._3),
                Card.Create(Card.Type._DrawTwo, Card.Color._3),
                Card.Create(Card.Type._1, Card.Color._4),
                Card.Create(Card.Type._2, Card.Color._4),
                Card.Create(Card.Type._3, Card.Color._4),
                Card.Create(Card.Type._4, Card.Color._4),
                Card.Create(Card.Type._5, Card.Color._4),
                Card.Create(Card.Type._6, Card.Color._4),
                Card.Create(Card.Type._7, Card.Color._4),
                Card.Create(Card.Type._8, Card.Color._4),
                Card.Create(Card.Type._9, Card.Color._4),
                Card.Create(Card.Type._Skip, Card.Color._4),
                Card.Create(Card.Type._Reverse, Card.Color._4),
                Card.Create(Card.Type._DrawTwo, Card.Color._4)
            };
        }

        protected void InitDiscard()
        {
            discard = new List<Card>();
        }

        protected void ShuffleDraw()
        {
            draw = draw.OrderBy(x => Random.value).ToList();
        }

        public List<Card> Draw(int count)
        {
            List<Card> drawnCards = new List<Card>();

            // If there is no deck
            if(draw == null)
            {
                Debug.LogError("Deck is not available.");
                return drawnCards;
            }
            
            // If there is no discard pile
            if (discard == null)
            {
                Debug.LogError("Discard is not available.");
                return drawnCards;
            }

            // If there are not enough cards to draw.
            if(draw.Count < count)
            {
                // If it is not possible to merge discard to draw.
                if ((discard.Count - 1 + draw.Count) < count)
                {
                    Debug.LogWarningFormat("Not enough cards to draw.");
                    return drawnCards;
                }

                // Merge discard into draw and shuffle.
                Debug.Log("Merging discard and draw piles.");
                draw.AddRange(discard.GetRange(0, discard.Count - 1));
                discard.RemoveRange(0, discard.Count - 1);
                ShuffleDraw();
            }

            // Draw requested number of cards.
            for (int i = 0; i < count; i++)
            {
                var idx = draw.Count - 1;
                Card card = draw[idx];
                drawnCards.Add(card);
                draw.RemoveAt(idx); 
            }

            return drawnCards;
        }
        
        public void Discard(List<Card> cards)
        {
            discard.AddRange(cards);
        }

        public void Reset() 
        {
            InitDraw();
            InitDiscard();
            ShuffleDraw();
        }

        public Card GetCurrentCard()
        {
            if(discard.Count < 1)
            {
                Debug.Log("No current card as discard pile is empty.");
                return null;
            }

            return discard.Last();
        }
    }
}

