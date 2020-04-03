namespace PokeCalc
{
    class StatsCalculator
    {
        Stats[] stats;

        public StatsCalculator()
        {
            stats = new Stats[100];
        }

        public Stats this[int level]
        {
            get
            {
                try
                {
                    return stats[level - 1]; // -1 so that level 1 is actually index 1
                }
                catch
                {
                    return Stats.Zero;
                }
            }
        }

        public void Update(Pokemon pokemon)
        {
            for (int i = 1; i < 101; ++i)
                stats[i - 1] = GetStats(i, pokemon); //-1 so that level 1 is actually index 1
        }

        public Stats GetStats(int level, Pokemon pokemon)
        {
            return new Stats(
                (((pokemon.stats_base.HP * 2) + pokemon.stats_iv.HP + pokemon.stats_ev.HP / 4) * level / 100) + level + 10,
                ((((pokemon.stats_base.Attack * 2) + pokemon.stats_iv.Attack + pokemon.stats_ev.Attack / 4) * level / 100) + 5),
                ((((pokemon.stats_base.Defence * 2) + pokemon.stats_iv.Defence + pokemon.stats_ev.Defence / 4) * level / 100) + 5),
                ((((pokemon.stats_base .SpAtk * 2) + pokemon.stats_iv.SpAtk + pokemon.stats_ev.SpAtk / 4) * level / 100) + 5),
                ((((pokemon.stats_base.SpDef * 2) + pokemon.stats_iv.SpDef + pokemon.stats_ev.SpDef / 4) * level / 100) + 5),
                ((((pokemon.stats_base.Speed * 2) + pokemon.stats_iv .Speed + pokemon.stats_ev.Speed / 4) * level / 100) + 5)
                );
        }

        //int GetStat(int level, int baseStat, int iv, int ev, bool isHPstat = false) // Don't touch this method
        //{
        //    // Don't touch this
        //    if (!isHPstat)
        //        return ((((baseStat * 2) + iv + ev / 4) * level / 100) + 5);
        //    else // it's the hp stat
        //        return (((baseStat * 2) + iv + ev / 4) * level / 100) + level + 10;
        //}

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
