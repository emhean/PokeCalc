namespace PokeCalc
{
    class StatsCalculator
    {
        Stats[] stats;
        Nature nature;

        public StatsCalculator()
        {
            stats = new Stats[100];
        }

        public void SetNature(Nature nature)
        {
            this.nature = nature;
        }

        public void Update(Pokemon pokemon)
        {
            for (int i = 1; i < 101; ++i)
                stats[i - 1] = GetStats(i, pokemon); //-1 so that level 1 is actually index 1
        }

        public Stats GetStats(int level, Pokemon pokemon)
        {
            var stats = new Stats(
                (((pokemon.stats_base.HP * 2) + pokemon.stats_iv.HP + pokemon.stats_ev.HP / 4) * level / 100) + level + 10,
                ((((pokemon.stats_base.Attack * 2) + pokemon.stats_iv.Attack + pokemon.stats_ev.Attack / 4) * level / 100) + 5),
                ((((pokemon.stats_base.Defence * 2) + pokemon.stats_iv.Defence + pokemon.stats_ev.Defence / 4) * level / 100) + 5),
                ((((pokemon.stats_base.SpAtk * 2) + pokemon.stats_iv.SpAtk + pokemon.stats_ev.SpAtk / 4) * level / 100) + 5),
                ((((pokemon.stats_base.SpDef * 2) + pokemon.stats_iv.SpDef + pokemon.stats_ev.SpDef / 4) * level / 100) + 5),
                ((((pokemon.stats_base.Speed * 2) + pokemon.stats_iv .Speed + pokemon.stats_ev.Speed / 4) * level / 100) + 5)
                );

            return stats * nature;
        }
    }
}
