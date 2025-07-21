using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game
{
    public class Wizard : Character
    {
        private Random rnd = new Random();
       
        public BurnStatus BurnEffect { get; set; }

        public Wizard(string name, int strength, int endurance, int agility, int luck)
            : base(name, strength, endurance, agility, luck)
        {
        }
        
        public override int Attack(Character target)
        {
            // Wizards may have spell-based attacks that can ignore agility
            int damage = (int)(Strength * 1.2 + (rnd.NextDouble() < (Luck * 0.01) ? Strength * 1.5 : 0));
            return damage;
        }

        public override void Defend(int damage)
        {
            // Wizards could have magical shields that absorb damage based on luck
            int shield = (int)(Luck * 0.5);
            int finalDamage = damage - shield;
            Health -= finalDamage > 0 ? finalDamage : 0;
        }

        public override void PerformUniqueAbility(Character target)
        {
            //Wizard's Fireball - Inflicts damage and may burn the target for additional damage
            int fireballDamage = (int)(Strength * 2.5);
            target.Health -= fireballDamage;

            Console.WriteLine($"{Name} casts a Fireball, dealing {fireballDamage} damage to {target.Name}.");

            // Implement burn effect
            if (target.BurnEffect.Duration <= 0)
            {
                int burnDamage = (int)(Strength * 0.5);
                target.BurnEffect = new BurnStatus(burnDamage, 3);

                Console.WriteLine($"{target.Name} is now burned, taking {burnDamage} damage for the next 3 turns.");
            }
        }
        public override void ApplyStatusEffects()
        {
            if (BurnEffect.Duration > 0)
            {
                Health -= BurnEffect.DamagePerTurn;
                var updatedBurnEffect = new BurnStatus(BurnEffect.DamagePerTurn, BurnEffect.Duration - 1);
                BurnEffect = updatedBurnEffect;
            }
        }
    }
}
