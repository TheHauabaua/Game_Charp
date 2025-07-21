namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Character> Enemies = GameSimulator.CreateComputerCharacters(5); // Creates 5 computer characters

            Character player = GameSimulator.CreatePlayerCharacter();
            bool exitGame = false;
            while (!exitGame)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Fight");
                Console.WriteLine("2. Spend Attribute Points");
                Console.WriteLine("3. Display Stats");
                Console.WriteLine("4. Exit Game");
                Console.Write("Choose an option: ");
                string option = Console.ReadLine();
                Console.WriteLine("");
                switch (option)
                {
                    case "1":
                        GameSimulator.StartBattle(player, Enemies);
                        break;
                    case "2":
                        if (player.AttributePoints > 0)
                        {
                            SpendAttributePoints(player);
                        }
                        else
                        {
                            Console.WriteLine("You don't have any attribute points to spend right now.");
                        }
                        break;
                    case "3":
                        player.ShowFullStats();
                        break;
                    case "4":
                        exitGame = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please choose again.");
                        break;
                }
            }
            static void SpendAttributePoints(Character playerCharacter)
            {
                Console.WriteLine($"You have {playerCharacter.AttributePoints} attribute points to spend.");
                int pointsToSpend = playerCharacter.AttributePoints; // Store the total points available to spend
                int strength = 0;
                int endurance = 0;
                int agility = 0;
                int luck = 0;

                int GetPointsToSpend(string attributeName)
                {
                    int points;
                    do
                    {
                        Console.Write($"Enter points to allocate to {attributeName} (Remaining points: {pointsToSpend}): ");
                        if (!int.TryParse(Console.ReadLine(), out points) || points < 0 || points > pointsToSpend)
                        {
                            Console.WriteLine("Invalid input. Please enter a positive number within your available points.");
                            points = -1;
                        }
                    } while (points < 0);

                    pointsToSpend -= points;
                    return points;
                }

                // Get the points to allocate to each attribute
                strength = GetPointsToSpend("Strength");
                endurance = GetPointsToSpend("Endurance");
                agility = GetPointsToSpend("Agility");
                luck = GetPointsToSpend("Luck");

                // Spend the points on the character attributes
                playerCharacter.SpendAttributePoints(strength, endurance, agility, luck);
            }
        }
    }
}
