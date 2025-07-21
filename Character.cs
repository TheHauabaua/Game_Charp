using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Character : ICharacter
    {
        public struct BurnStatus
        {
            public int DamagePerTurn;
            public int Duration;

            public BurnStatus(int damagePerTurn, int duration)
            {
                DamagePerTurn = damagePerTurn;
                Duration = duration;
            }
        }
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Endurance { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public int Health { get; set; }
        public int Level { get; private set; } = 1;
        public int Experience { get; private set; } = 0;
        public int AttributePoints { get; private set; } = 0;
        public BurnStatus BurnEffect { get; set; } = new BurnStatus(0, 0);
        protected Character(string name, int strength, int endurance, int agility, int luck)
        {
            Name = name;
            Strength = strength;
            Endurance = endurance;
            Agility = agility;
            Luck = luck;
            Health = endurance * 10; // health is 10x the endurance level
        }
        
        public abstract int Attack(Character target);
        public abstract void Defend(int damage);
        public virtual void PerformUniqueAbility(Character target){}
        public virtual void ApplyStatusEffects()
        {
            if (BurnEffect.Duration > 0)
            {
                Health -= BurnEffect.DamagePerTurn;
                Console.WriteLine($"{Name} takes {BurnEffect.DamagePerTurn} burn damage.");
                BurnEffect = new BurnStatus(BurnEffect.DamagePerTurn, BurnEffect.Duration - 1);
            }
        }
        public bool IsAlive()
        {
            return Health > 0;
        }
        public void ShowStats()
        {
            Console.WriteLine($"{Name} - Health: {Health}, Strength: {Strength}");
        }
        public void ShowFullStats()
        {
            Console.WriteLine($"{Name} - Stats");
            Console.WriteLine($"Health: {Health}");
            Console.WriteLine($"Strength: {Strength}");
            Console.WriteLine($"Endurance: {Endurance}");
            Console.WriteLine($"Agility: {Agility}");
            Console.WriteLine($"Luck: {Luck}");
            Console.WriteLine($"Level: {Level}");
            Console.WriteLine($"Experience: {Experience}/{ExperienceToLevelUp(Level)}");
            Console.WriteLine($"Attribute Points: {AttributePoints}");
        }
        public void GainExperience(int xp)
        {
            Experience += xp;
            while (Experience >= ExperienceToLevelUp(Level))
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            Experience -= ExperienceToLevelUp(Level);
            Level++;
            AttributePoints += 5; // Award 5 attribute points for level up
            Console.WriteLine($"{Name} has reached level {Level}!");
        }
        private int ExperienceToLevelUp(int currentLevel)
        {
            return 100 * currentLevel;
        }
        public void SpendAttributePoints(int strength, int endurance, int agility, int luck)
        {
            // Ensure the character has enough attribute points
            int totalPoints = strength + endurance + agility + luck;
            if (totalPoints <= AttributePoints)
            {
                Strength += strength;
                Endurance += endurance;
                Agility += agility;
                Luck += luck;
                AttributePoints -= totalPoints;
                Health = Endurance * 10; // Update health in case endurance was increased
            }
            else
            {
                Console.WriteLine("Not enough attribute points.");
            }
        }
    }

}
