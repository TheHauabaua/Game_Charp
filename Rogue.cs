using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game
{
    public class Rogue : Character
    {
        private Random rnd = new Random();
        public Rogue(string name, int strength, int endurance, int agility, int luck)
            : base(name, strength, endurance, agility, luck)
        {
        }

        public override int Attack(Character target)
        {
            // Rogues have a high chance of critical hits and dodging
            int damage = Strength + (rnd.NextDouble() < (Luck * 0.05) ? Strength * 2 : 0);
            return damage;
        }

        public override void Defend(int damage)
        {
            // Rogues may dodge more frequently
            if (rnd.NextDouble() >= (Agility * 0.04))
            {
                Health -= damage;
            }
        }

        public override void PerformUniqueAbility(Character target)
        {
            //Rogue's Backstab - Extra damage if the luck roll succeeds
            int backstabDamage = 0;
            if (rnd.NextDouble() < (Luck * 0.05))
            {
                backstabDamage = Strength * 3;
                target.Health -= backstabDamage;
            }

            Console.WriteLine($"{Name} attempts a Backstab, " + (backstabDamage > 0 ? $"succeeding and dealing {backstabDamage} damage to {target.Name}." : "but fails to land the critical hit."));
        }
    }
}
