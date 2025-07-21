using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game
{
    public class Archer : Character
    {
        private Random rnd = new Random();
        public Archer(string name, int strength, int endurance, int agility, int luck)
            : base(name, strength, endurance, agility, luck)
        {
        }

        public override int Attack(Character target)
        {
            // Archers may have a higher chance to hit and critical strike based on agility and luck
            int damage = (int)(Strength * 1.5 + (rnd.NextDouble() < (Luck * 0.03) ? Strength : 0));
            return damage;
        }

        public override void Defend(int damage)
        {
            // Archers could have a chance to dodge an attack based on agility
            if (rnd.NextDouble() >= (Agility * 0.02))
            {
                Health -= damage;
            }
        }

        public override void PerformUniqueAbility(Character target)
        {
            // Piercing Shot - Ignores target's endurance
            int piercingDamage = (int)(Strength * 2);
            target.Health -= piercingDamage;
            Console.WriteLine($"{Name} performs a Piercing Shot, dealing {piercingDamage} damage to {target.Name}, ignoring endurance.");
        }
    }
}
