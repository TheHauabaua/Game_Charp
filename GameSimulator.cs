using System.ComponentModel;

namespace Game
{
    public class GameSimulator
    {
        public static Character CreatePlayerCharacter()
        {
            Console.WriteLine("Choose your character class:");
            Console.WriteLine("1. Warrior");
            Console.WriteLine("2. Archer");
            Console.WriteLine("3. Rogue");
            Console.WriteLine("4. Wizard");
            Console.Write("Choose an option: ");
            string classChoice = Console.ReadLine();
            Console.WriteLine("");
            Character player = null;

            int strength = 0;
            int endurance = 0;
            int agility = 0;
            int luck = 0;
            int totalPoints = 20;

            // Function to allocate points
            Func<string, int> allocatePoints = (attributeName) =>
            {
                Console.WriteLine($"Enter points to allocate to {attributeName} (Remaining points: {totalPoints}):");
                int points;
                while (!int.TryParse(Console.ReadLine(), out points) || points < 0 || points > totalPoints)
                {
                    Console.WriteLine($"Invalid input. Please enter a value between 0 and {totalPoints}.");
                }
                totalPoints -= points;
                return points;
            };

            // Allocate points to attributes based on player input
            strength = allocatePoints("Strength");
            endurance = allocatePoints("Endurance");
            agility = allocatePoints("Agility");
            luck = allocatePoints("Luck");

            //check in case totalPoints were not fully allocated
            if (totalPoints > 0)
            {
                Console.WriteLine($"You have {totalPoints} unallocated points. Distributing them evenly across attributes.");
                // Distribute remaining points evenly
                int pointsToAdd = totalPoints / 4;
                strength += pointsToAdd;
                endurance += pointsToAdd;
                agility += pointsToAdd;
                luck += pointsToAdd;
            }

            // Create character based on class choice
            switch (classChoice)
            {
                case "1":
                    player = new Warrior("Player Warrior", strength, endurance, agility, luck);
                    break;
                case "2":
                    player = new Archer("Player Archer", strength, endurance, agility, luck);
                    break;
                case "3":
                    player = new Rogue("Player Rogue", strength, endurance, agility, luck);
                    break;
                case "4":
                    player = new Wizard("Player Wizard", strength, endurance, agility, luck);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Defaulting to Warrior.");
                    player = new Warrior("Player Warrior", strength, endurance, agility, luck);
                    break;
            }

            return player;
        }

        public static Character CreateComputerCharacter(string characterClass)
        {
            Random rnd = new Random();

            int totalPoints = 20;
            int minPointsPerAttribute = 1; // Minimum points to allocate to each attribute

            // Function to randomly allocate points
            Func<int, int> allocateRandomPoints = (pointsRemaining) =>
            {
                // Allocate a random number of points
                int points = rnd.Next(minPointsPerAttribute, pointsRemaining + 1);
                return points;
            };

            // Randomly allocate points to attributes
            int strength = allocateRandomPoints(totalPoints - (3 * minPointsPerAttribute));
            totalPoints -= strength;

            int endurance = allocateRandomPoints(totalPoints - (2 * minPointsPerAttribute));
            totalPoints -= endurance;

            int agility = allocateRandomPoints(totalPoints - minPointsPerAttribute);
            totalPoints -= agility;

            int luck = totalPoints; // Assign any leftover points to luck

            // Ensure that luck is not zero
            if (luck < minPointsPerAttribute)
            {
                // If luck is less than the minimum threshold, redistribute points
                if (agility > minPointsPerAttribute)
                {
                    agility--;
                    luck++;
                }
                else if (endurance > minPointsPerAttribute)
                {
                    endurance--;
                    luck++;
                }
                else if (strength > minPointsPerAttribute)
                {
                    strength--;
                    luck++;
                }
            }

            // Create character based on specified class
            Character enemy;
            switch (characterClass.ToLower())
            {
                case "warrior":
                    enemy = new Warrior("Computer Warrior", strength, endurance, agility, luck);
                    break;
                case "archer":
                    enemy = new Archer("Computer Archer", strength, endurance, agility, luck);
                    break;
                case "rogue":
                    enemy = new Rogue("Computer Rogue", strength, endurance, agility, luck);
                    break;
                case "wizard":
                    enemy = new Wizard("Computer Wizard", strength, endurance, agility, luck);
                    break;
                default:
                    throw new ArgumentException("Invalid character class specified.");
            }

            return enemy;
        }

        public static List<Character> CreateComputerCharacters(int numberOfCharacters)
        {
            List<Character> computerCharacters = new List<Character>();
            Random rnd = new Random();

            // Define available classes
            string[] characterClasses = { "warrior", "archer", "rogue", "wizard" };

            for (int i = 0; i < numberOfCharacters; i++)
            {
                // Randomly select a character class for each computer character
                string selectedClass = characterClasses[rnd.Next(characterClasses.Length)];

                // Create a computer character with the selected class and random attributes
                Character computerCharacter = CreateComputerCharacter(selectedClass);
                computerCharacters.Add(computerCharacter);
            }

            return computerCharacters;
        }

        public static void StartBattle(Character player, List<Character> enemies)
        {
            Console.WriteLine("Choose your enemy:");
            for (int i = 0; i < enemies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {enemies[i].Name}");
            }

            int chosenIndex = Convert.ToInt32(Console.ReadLine()) - 1;
            Character enemy = enemies[chosenIndex];

            Console.WriteLine($"You have chosen to fight {enemy.Name}!");
            player.ShowStats();
            enemy.ShowStats();

            bool battleEnded = false;
            Random rnd = new Random();
            bool isEnemyDefending = false;
            bool isPlayerDefending = false;
            while (!battleEnded)
            {
                player.ApplyStatusEffects();
                if (player.IsAlive() && enemy.IsAlive())
                {
                    Console.WriteLine("\nYour turn:");
                    Console.WriteLine("1. Attack");
                    Console.WriteLine("2. Defend");
                    Console.WriteLine("3. Perform Unique Ability");
                    Console.WriteLine("4. Run Away");
                    Console.Write("Choose an action: ");
                    string action = Console.ReadLine();
                    Console.WriteLine("");
                    int damage;

                    switch (action)
                    {
                        case "1":
                            damage = player.Attack(enemy);
                            if (isEnemyDefending)
                            {
                                int initialOpponentHealth = enemy.Health;
                                enemy.Defend(damage);
                                int actualDamageTaken = initialOpponentHealth - enemy.Health;
                                Console.WriteLine($"{enemy.Name} defends and reduces the incoming damage from {damage} to {actualDamageTaken}.");
                            }
                            else
                            {
                                Console.WriteLine($"{player.Name} deals {damage} damage to {enemy.Name}.");
                                enemy.Health -= damage;
                            }
                            isEnemyDefending = false;
                            break;
                        case "2":
                            Console.WriteLine($"{player.Name} prepares to defend.");
                            isPlayerDefending = true;
                            break;
                        case "3":
                            player.PerformUniqueAbility(enemy);
                            break;
                        case "4":
                            Console.WriteLine("You decided to run away!");
                            battleEnded = true;
                            continue;
                        default:
                            Console.WriteLine("Invalid action, please choose again.");
                            continue;
                    }
                    player.ShowStats();
                    enemy.ShowStats();


                    enemy.ApplyStatusEffects();
                    if (!enemy.IsAlive())
                    {
                        Console.WriteLine($"{enemy.Name} has been defeated!");
                        Console.WriteLine("Player wins!");

                        // XP reward for defeating an enemy
                        int xpReward = CalculateXPReward(player, enemy);
                        player.GainExperience(xpReward);
                        Console.WriteLine($"{player.Name} gains {xpReward} XP for winning the battle.");

                        battleEnded = true;
                        break;
                    }

                    if (battleEnded) continue;// Skip enemy's turn if the battle has ended
                    Console.WriteLine("\nEnemy's turn:");
                    int choice = rnd.Next(1, 4); // Randomly decide whether to defend, attack or perform a unique ability

                    if (choice == 1)
                    {
                        Console.WriteLine($"{enemy.Name} prepares to defend.");
                        isEnemyDefending = true;
                    }
                    else
                    {
                        int attackChoice = rnd.Next(1, 6);// Roll again for attack or unique ability

                        if (attackChoice == 1)
                        {
                            enemy.PerformUniqueAbility(player);
                            Console.WriteLine($"{enemy.Name} uses their unique ability.");
                        }
                        else
                        {
                            int enemyDamage = enemy.Attack(player);
                            if (isPlayerDefending)
                            {
                                int initialHealth = player.Health;
                                player.Defend(enemyDamage);
                                int actualDamageTaken = initialHealth - player.Health;
                                Console.WriteLine($"{player.Name} defends and reduces the incoming damage from {enemyDamage} to {actualDamageTaken}.");
                                isPlayerDefending = false;
                            }
                            else
                            {
                                Console.WriteLine($"{enemy.Name} attacks and deals {enemyDamage} damage to {player.Name}.");
                                player.Health -= enemyDamage; // Apply the full damage if not defending
                            }
                        }
                        isEnemyDefending = false;
                    }

                    if (!player.IsAlive())
                    {
                        Console.WriteLine($"{player.Name} has been defeated!");
                        Console.WriteLine("Computer wins!");
                        battleEnded = true;
                        break;
                    }
                    player.ShowStats();
                    enemy.ShowStats();
                }
            }
        }
        private static int CalculateXPReward(Character player, Character enemy)
        {
            int baseXP = 50;
            // Calculate additional XP based on the difference in health and strength
            int healthDifference = enemy.Health - player.Health;
            int strengthDifference = enemy.Strength - player.Strength;

            // Ensure that healthDifference and strengthDifference are positive for XP calculation
            healthDifference = Math.Max(0, healthDifference);
            strengthDifference = Math.Max(0, strengthDifference);

            int bonusXP = (healthDifference / 10) + (strengthDifference * 2);
            int totalXP = baseXP + bonusXP;
            return totalXP;
        }
    }
}
