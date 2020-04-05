using System;

namespace PokeCalc
{
    /// <summary>
    /// A field wrapper for stats.
    /// </summary>
    struct Stats
    {
        public int HP;
        public int Attack;
        public int Defence;
        public int SpAtk;
        public int SpDef;
        public int Speed;

        public Stats(int hp, int attack, int defence, int spAtk, int spDef, int speed)
        {
            this.HP = hp;
            this.Attack = attack;
            this.Defence = defence;
            this.SpAtk = spAtk;
            this.SpDef = spDef;
            this.Speed = speed;
        }

        /// <summary>
        /// Create a Stats instance from a string.
        /// </summary>
        /// <param name="str">Example: 45;49;49;65;65;45</param>
        public Stats(string str)
        {
            string[] s = str.Split(';');

            this.HP = int.Parse(s[0]);
            this.Attack = int.Parse(s[1]);
            this.Defence = int.Parse(s[2]);
            this.SpAtk = int.Parse(s[3]);
            this.SpDef = int.Parse(s[4]);
            this.Speed = int.Parse(s[5]);
        }

        /// <summary>
        /// Create a Stats instance from a string array.
        /// </summary>
        /// <param name="arr">Example: 45;49;49;65;65;45</param>
        public Stats(string[] arr)
        {
            this.HP = int.Parse(arr[0]);
            this.Attack = int.Parse(arr[1]);
            this.Defence = int.Parse(arr[2]);
            this.SpAtk = int.Parse(arr[3]);
            this.SpDef = int.Parse(arr[4]);
            this.Speed = int.Parse(arr[5]);
        }

        static public Stats Zero => new Stats(0, 0, 0, 0, 0, 0);

        static public Stats IV_Max => new Stats(31, 31, 31, 31, 31, 31);
        static public Stats EV_Max => new Stats(252, 252, 252, 252, 252, 252);

        static public Stats Unit_HP => new Stats(1, 0, 0, 0, 0, 0);
        static public Stats Unit_Attack => new Stats(0, 1, 0, 0, 0, 0);
        static public Stats Unit_Defence => new Stats(0, 0, 1, 0, 0, 0);
        static public Stats Unit_SpAtk => new Stats(0, 0, 0, 1, 0, 0);
        static public Stats Unit_SpDef => new Stats(0, 0, 0, 0, 1, 0);
        static public Stats Unit_Speed => new Stats(0, 0, 0, 0, 0, 1);


        static public int EV_SUM_MAX => 508;
        static public int IV_SUM_MAX => 186;

        public int GetSum()
        {
            return HP + Attack + Defence + SpAtk + SpDef + Speed;
        }

        public enum STAT
        {
            HP, ATTACK, DEFENCE, SP_ATTACK, SP_DEFENCE, SPEED
        }
        public int GetFirstNonZero()
        {
            if (HP != 0)
                return HP;
            else if (Attack != 0)
                return Attack;
            else if (Defence != 0)
                return Defence;
            else if (SpAtk != 0)
                return SpAtk;
            else if (SpDef != 0)
                return SpDef;
            else if (Speed != 0)
                return Speed;
            else
                return 0;
        }

        // Negative operator
        static public Stats operator -(Stats s1)
        {
            s1.HP = -s1.HP;
            s1.Attack = -s1.Attack;
            s1.Defence = -s1.Defence;
            s1.SpAtk = -s1.SpAtk;
            s1.SpDef = -s1.SpDef;
            s1.Speed = -s1.Speed;
            return s1;
        }

        // + operator 
        static public Stats operator +(Stats s1, Stats s2)
        {
            return new Stats(
                s1.HP + s2.HP,
                s1.Attack + s2.Attack,
                s1.Defence + s2.Defence,
                s1.SpAtk + s2.SpAtk,
                s1.SpDef + s2.SpDef,
                s1.Speed + s2.Speed);
        }

        // - operator 
        static public Stats operator -(Stats s1, Stats s2)
        {
            return new Stats(
                s1.HP - s2.HP,
                s1.Attack - s2.Attack,
                s1.Defence - s2.Defence,
                s1.SpAtk - s2.SpAtk,
                s1.SpDef - s2.SpDef,
                s1.Speed - s2.Speed);
        }

        // * operator 
        static public Stats operator *(Stats s1, Stats s2)
        {
            return new Stats(
                s1.HP * s2.HP,
                s1.Attack * s2.Attack,
                s1.Defence * s2.Defence,
                s1.SpAtk * s2.SpAtk,
                s1.SpDef * s2.SpDef,
                s1.Speed * s2.Speed);
        }

        static public Stats operator *(Stats s1, Nature nat)
        {
            // WARNING
            // DO NOT USE System.ConvertToInt()
            // Beause it rounds to most even integer.
            // In this case we want to round down.

            float atk = s1.Attack * nat.Attack;
            float def = s1.Defence * nat.Defence;
            float spAtk = s1.SpAtk * nat.SpAtk;
            float spDef = s1.SpDef * nat.SpDef;
            float spd = s1.Speed * nat.Speed;

            return new Stats(s1.HP, (int)atk, (int)def, (int)spAtk, (int)spDef, (int)spd);
        }
    }
}