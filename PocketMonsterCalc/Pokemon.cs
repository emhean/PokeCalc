namespace PokeCalc
{
    struct Pokemon
    {
        public string name;
        public string type1, type2;
        public int dexID;

        public byte level;

        public Stats stats_base;
        public Stats stats_ev;
        public Stats stats_iv;

        public Pokemon(int dexID, string name, string type1, string type2, Stats stats_base)
        {
            this.dexID = dexID;
            this.name = name;
            this.type1 = type1;
            this.type2 = type2;
            this.stats_base = stats_base;

            this.level = 1;
            this.stats_ev = Stats.Zero;
            this.stats_iv = Stats.Zero;
        }

        /// <summary>
        /// Create a Pokémon from a string.
        /// </summary>
        /// <param name="str">Example: 001;Bulbasaur;45;49;49;65;65;45</param>
        public Pokemon(string str)
        {
            //001;Bulbasaur;45;49;49;65;65;45
            string[] split = str.Split(';');

            dexID = int.Parse(split[0]);
            name = split[1];

            type1 = string.Empty;
            type2 = string.Empty;
            this.level = 1;
            this.stats_ev = Stats.Zero;
            this.stats_iv = Stats.Zero;

            string[] split_stats = new string[split.Length - 2];

            for (int i = 0; i < split.Length - 2; ++i)
            {
                split_stats[i] = split[i + 2];
            }

            stats_base = new Stats(split_stats);
        }



        public override string ToString()
        {
            return string.Format("{0} [{1}]", name, dexID);
        }

        //public void UpdateFinalStats()
        //{
        //    stats_final.HP = GetStat(level, stats_base.HP, stats_iv.HP, stats_ev.HP, true);
        //    stats_final.Attack = GetStat(level, stats_base.Attack, stats_iv.Attack, stats_ev.Attack);
        //    stats_final.Defence = GetStat(level, stats_base.Defence, stats_iv.Defence, stats_ev.Defence);
        //    stats_final.SpAtk = GetStat(level, stats_base.SpAtk, stats_iv.SpAtk, stats_ev.SpAtk);
        //    stats_final.SpDef = GetStat(level, stats_base.SpDef, stats_iv.SpDef, stats_ev.SpDef);
        //    stats_final.Speed = GetStat(level, stats_base.Speed, stats_iv.Speed, stats_ev.Speed);
        //}

        //public int LevelUp()
        //{
        //    if (level == 100)
        //        return level;

        //    level += 1;
        //    UpdateFinalStats();

        //    return level;
        //}
    }
}
