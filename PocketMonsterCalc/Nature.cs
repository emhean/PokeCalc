using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PokeCalc
{
    class Nature
    {
        static List<Nature> natures = new List<Nature>()
        {
            new Nature("Hardy"),
            new Nature("Lonely", attack:1.1f, defence:0.9f),
            new Nature("Brave", attack:1.1f, speed:0.9f),
            new Nature("Adamant", attack:1.1f, spAtk:0.9f),
            new Nature("Naughty", attack:1.1f, spDef:0.9f),
            new Nature("Bold", defence:1.1f, attack:0.9f),
            new Nature("Docile"),
            new Nature("Relaxed", defence:1.1f, speed:0.9f),
            new Nature("Impish", defence:1.1f, spAtk:0.9f),
            new Nature("Lax", defence:1.1f, spDef:0.9f),
            new Nature("Timid", speed:1.1f, attack:0.9f),
            new Nature("Hasty", speed:1.1f, defence:0.9f),
            new Nature("Serious"),
            new Nature("Jolly", speed:1.1f, spAtk:0.9f),
            new Nature("Naive", speed:1.1f, spDef:0.9f),
            new Nature("Modest", spAtk:1.1f, attack:0.9f),
            new Nature("Mild", spAtk:1.1f, defence:0.9f),
            new Nature("Quiet", spAtk:1.1f, speed:0.9f),
            new Nature("Bashful"),
            new Nature("Rash", spAtk:1.1f, spDef:0.9f),
            new Nature("Calm", spDef:1.1f, attack:0.9f),
            new Nature("Gentle", spDef:1.1f, defence:0.9f),
            new Nature("Sassy", spDef:1.1f, speed:0.9f),
            new Nature("Careful", spDef:1.1f, spAtk:0.9f),
            new Nature("Quirky"),
        };

        //public Nature this [int index] => natures[index];

        public static ReadOnlyCollection<Nature> GetNatures()
        {
            return natures.AsReadOnly();
        }

        public static Nature GetNature(string name)
        {
            return natures.Find(x => (x.Name.Equals(name)));
        }

        /// <summary>
        /// The name of the nature.
        /// </summary>
        public string Name;
        /// <summary>
        /// Liked berry flavor.
        /// </summary>
        public string fav_flavor;
        /// <summary>
        /// Disliked berry flavor.
        /// </summary>
        public string dis_flavor;

        public float Attack;
        public float Defence;
        public float SpAtk;
        public float SpDef;
        public float Speed;

        public Nature(string name, string fav_flavor="", string dis_flavor="", float attack=1f, float defence=1f, float spAtk=1f, float spDef=1f, float speed=1f)
        {
            this.Name = name;
            this.fav_flavor = fav_flavor;
            this.dis_flavor = dis_flavor;

            this.Attack = attack;
            this.Defence = defence;
            this.SpAtk = spAtk;
            this.SpDef = spDef;
            this.Speed = speed;
        }

        /// <summary>
        /// Returns the name of the nature.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}