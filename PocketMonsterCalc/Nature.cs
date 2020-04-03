using System.Collections.Generic;

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

        public static Nature GetNature(string name)
        {
            return natures.Find(x => (x.Name.Equals(name)));
        }

        public string Name;
        public string fav_flavor, dis_flavor;

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

        static Nature neutral = new Nature("None");
        public static Nature Neutral
        {
            get => neutral;
        }
    }
}

#region Junk
//#region Operators
//public static Stats operator +(Stats s1, Stats s2)
//{
//    return new Stats(
//        s1.HP + s2.HP,
//        s1.Attack + s2.Attack,
//        s1.Defence + s2.Defence,
//        s1.SpAtk + s2.SpAtk,
//        s1.SpDef + s2.SpDef,
//        s1.SpDef + s2.Speed);
//}
//public static Stats operator -(Stats s1, Stats s2)
//{
//    return new Stats(
//        s1.HP - s2.HP,
//        s1.Attack - s2.Attack,
//        s1.Defence - s2.Defence,
//        s1.SpAtk - s2.SpAtk,
//        s1.SpDef - s2.SpDef,
//        s1.SpDef - s2.Speed);
//}
//public static Stats operator *(Stats s1, Stats s2)
//{
//    return new Stats(
//        s1.HP * s2.HP,
//        s1.Attack * s2.Attack,
//        s1.Defence * s2.Defence,
//        s1.SpAtk * s2.SpAtk,
//        s1.SpDef * s2.SpDef,
//        s1.SpDef * s2.Speed);
//}
//#endregion
#endregion