using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Warrior : Character
    {
        private Random rnd = new Random();

        public Warrior(string name, int strength, int endurance, int agility, int luck)
            : base(name, strength, endurance, agility, luck)
        {
        }

        public override int Attack(Character target)
        {
            // Warriors have a strong attack that can benefit from strength and a slightly chance of critical hits based on luck
            int damage = Strength + (rnd.NextDouble() < (Luck * 0.01) ? (int)(Strength * 0.5) : 0);
            return damage;
        }

        public override void Defend(int damage)
        {
            // Warriors have a chance to reduce the incoming damage based on their endurance
            int reducedDamage = (int)(damage - (Endurance * 0.2));
            Health -= reducedDamage > 0 ? reducedDamage : 0;
        }

        public override void PerformUniqueAbility(Character target)
        {
            // Berserk - Increase own strength temporarily and attack
            Strength += 5;
            int berserkAttack = Attack(target);
            Strength -= 5;

            target.Health -= berserkAttack;
            Console.WriteLine($"{Name} enters Berserk mode, increasing strength and dealing {berserkAttack} damage to {target.Name}.");
        }
    }
}
