using UnityEngine;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using TestClasses;
/// <summary>
/// Test class for custom types' methods
/// </summary>
public class CustomTypesTest : MonoBehaviour
{
    [CallableFunction]
    public void CharacterMethod(CharacterTestClass a)
    {
        Debug.Log("CustomTypesTest/CharacterTest : " + (a==null?"null":a.ToString()));
    }
    [CallableFunction]
    public void MagicCharacterMethod(MagicCharacterTestClass b)
    {
        Debug.Log("CustomTypesTest/MagicCharacterTest : " + (b == null?"null":b.ToString()));
    }

    [CallableFunction]
    public void ArrayOfCharactersTest(CharacterTestClass[] characters)
    {
        int healthSum = (from c in characters select (c==null?0:c.health)).Sum();
        Debug.Log("CustomTypesTest/ArrayOfCharactersTest, length = "  + characters.Length + " sum of healthes = " + healthSum);
    }

    [CallableFunction]
    public void MageInterfaceTest(IMageTestInterface mage)
    {
        Debug.Log("CustomTypesTest/MageInterfaceTest, name = " + (mage==null?"NULL OBJECT":mage.GetName()));
    }

    [CallableFunction]
    public CharacterTestClass CharacterReturnMethod()
    {
        return new CharacterTestClass("Returned Character", 123);
    }

    [CallableFunction]
    public void EndlessRecursionMethod(EndlessConstructorClass e)
    {
        Debug.Log("CustomTypesTest/EndlessRecursionMethod, name=" + e.name);
    }

    [CallableFunction]
    public void InnerTypesTest(IInnerInterface inner)
    {
        Debug.Log("CustomTypesTest/InnerTypesTest, type = " + inner.GetType().Name + ", name = " + inner.Name);
    }
    public interface IInnerInterface
    {
        string Name { get; }
    }

    public class PublicA : IInnerInterface
    {
        public string Name { get; set; }

        public PublicA(string name)
        {
            Name = name;
        }
    }
    private class PrivateB : IInnerInterface
    {
        public string Name { get; set; }

        public PrivateB(string name)
        {
            Name = name;
        }
    }
}

namespace TestClasses
{
    /// <summary>
    /// Interface for mage
    /// </summary>
    public interface IMageTestInterface
    {
        string GetName();
    }

    /// <summary>
    /// Class with copy constructor
    /// </summary>
    public class EndlessConstructorClass
    {
        public string name;
        public EndlessConstructorClass(string name)
        {
            this.name = name;
        }
        public EndlessConstructorClass(EndlessConstructorClass e)
        {
            this.name = e.name;
        }
    }
    /// <summary>
    /// Class, representing one character
    /// </summary>
    public class CharacterTestClass
    {
        public string name;
        public int health;
        public string[] items;

        public CharacterTestClass()
        {
            name = "unnamed";
            health = 100;
            items = new string[0];
        }

        public CharacterTestClass(string name, int health)
        {
            this.name = name;
            this.health = health;
            items = new string[0];
        }

        public CharacterTestClass(string name, int health, string[] items)
        {
            this.name = name;
            this.health = health;
            this.items = items;
        }

        public override string ToString()
        {
            string itemsstr = "";
            foreach (var item in items)
            {
                itemsstr += item.ToString() + ", ";
            }
            itemsstr = itemsstr.TrimEnd(',', ' ');
            return string.Format("Character {0}, health : {1}, items list : {2}", name, health, itemsstr);
        }
    }
    /// <summary>
    /// Class, representing mage
    /// </summary>
    public class MagicCharacterTestClass : CharacterTestClass, IMageTestInterface
    {
        public float mana;
        public MagicCharacterTestClass()
        {
            mana = 0;
        }

        public MagicCharacterTestClass(string name, int health, string[] items, float mana) : base(name, health, items)
        {
            this.mana = mana;
        }

        public override string ToString()
        {
            string str = base.ToString().Substring(9);
            return "Magic character " + str + ", mana : " + mana.ToString();

        }

        public string GetName()
        {
            return name;
        }
    }
}

