using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string Letter;
        public MessageType MessageType;
        public MessageTopic MessageTopic;

        public Category(string letter, MessageType messageType, MessageTopic messageTopic) 
        {
            this.Letter = letter;
            this.MessageType = messageType; 
            this.MessageTopic = messageTopic;
        }

        public override bool Equals(object obj)
        {
            if (obj is Category category)
            {
                if (category.Letter == Letter && category.MessageType == MessageType && category.MessageTopic == MessageTopic)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public override string ToString() 
        {
            return Letter + "." + MessageType + "." + MessageTopic;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Category category)) return 0;
            category = (Category)obj;
            var leterCompare = String.Compare(Letter, category.Letter);

            if (leterCompare == -1) return -1;
            if (leterCompare == 1) return 1;

            if (MessageType > category.MessageType) return 1;
            if(MessageType < category.MessageType) return -1;

            if (MessageTopic > category.MessageTopic) return 1;
            if (MessageTopic < category.MessageTopic) return -1;

            return 0;
        }



        public static bool operator<= (Category left, Category right) 
        {
            return left.CompareTo(right) < 1;
        }

        public static bool operator>= (Category left, Category right)
        {
            return left.CompareTo(right) > -1;
        }

        public static bool operator< (Category left, Category right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool operator> (Category left, Category right)
        {
            return left.CompareTo(right) == 1;
        }
    }
}
