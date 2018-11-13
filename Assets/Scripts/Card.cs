namespace Tadget
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Card : ScriptableObject, IEquatable<Card>
    {
        public enum Type
        {
            _, _0, _1, _2, _3, _4, _5, _6, _7, _8, _9,
            _Skip, _Reverse, _DrawTwo, _Wild, _WildDrawFour
        }

        public enum Color
        {
            _1,
            _2,
            _3,
            _4,
            _
        }

        [SerializeField] public Type type {get; private set;}
        [SerializeField] public Color color {get; private set;}

        public static Card Create(Type type, Color color) 
        {
            return ScriptableObject.CreateInstance<Card>().Init(type, color);
        }

        private Card Init(Type t, Color c)
        {
            type = t;
            color = c;
            return this;
        }

        public override string ToString()
        {
            string t, c;
            switch (type)
            {
                case Type._0:
                {
                    t = "0";
                    break;
                }
                case Type._1:
                {
                    t = "1";
                    break;
                }
                case Type._2:
                {
                    t = "2";
                    break;
                }
                case Type._3:
                {
                    t = "3";
                    break;
                }
                case Type._4:
                {
                    t = "4";
                    break;
                }
                case Type._5:
                {
                    t = "5";
                    break;
                }
                case Type._6:
                {
                    t = "6";
                    break;
                }
                case Type._7:
                {
                    t = "7";
                    break;
                }
                case Type._8:
                {
                    t = "8";
                    break;
                }
                case Type._9:
                {
                    t = "9";
                    break;
                }
                case Type._Skip:
                {
                    t = "Skip";
                    break;
                }
                case Type._Reverse:
                {
                    t = "Reverse";
                    break;
                }
                case Type._DrawTwo:
                {
                    t = "Draw Two";
                    break;
                }
                case Type._Wild:
                {
                    t = "Wild";
                    break;
                }
                case Type._WildDrawFour:
                {
                    t = "Wild Draw Four";
                    break;
                }
                default:
                    t = "Unknown";
                    break;
            }

            switch (color)
            {
                case Color._1:
                {
                    c = "Red";
                    break;
                }
                case Color._2:
                {
                    c = "Green";
                    break;
                }
                case Color._3:
                {
                    c = "Blue";
                    break;
                }
                case Color._4:
                {
                    c = "Yellow";
                    break;
                }
                default:
                    c = "Unknown";
                    break;
            }
            return "{" + c + "}|" + t;
        }

        public void SetWildcardColor(Color c)
        {
            if(type == Type._Wild || type == Type._WildDrawFour)
            {
                color = c;
            }
            else
            {
                Debug.LogWarning("Invalid attempt to change non Wild type card color.");
            }
        }

        public static bool operator ==(Card left, Card right)
        {
            // Review https://msdn.microsoft.com/en-US/library/ms173147%28v=vs.80%29.aspx
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            // Return true if the fields match:
            return left.Equals(right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }

        public bool Equals(Card other)
        {
            if(other == null)
            {
                return false;
            }

            return other.color == color && other.type == type;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Card);
        }

        public override int GetHashCode()
        {
            return color.GetHashCode() ^ type.GetHashCode();
        }

    }
}

