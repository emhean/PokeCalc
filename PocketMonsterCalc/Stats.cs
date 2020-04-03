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
    }
}