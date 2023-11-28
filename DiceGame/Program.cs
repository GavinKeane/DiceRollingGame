Console.WriteLine("Simulating...");

// Various counters.
int wins = 0;
int loses = 0;
int streak = 0;
int biggestStreak = 0;

// Debugging status. It is unwise to set this to true when simulating a large amount of games.
bool debug = false;

// Looping games. The upper bound on i indicates how many games will be played in total.
for (int i = 0; i < 1000000; i++) {

    // True = win, False = loss.
    if (playGame(debug)) {
        wins++;
        streak = 0;
    } else {
        loses++;
        streak++;

        // Keeping track of Biggest Pot statistic.
        if (streak > biggestStreak) {
            biggestStreak = streak;
        }
    }
}

// Clearing the console if not in debug mode.
if (!debug) {
    Console.Clear();
} else {
    Console.WriteLine();
}

// Printing all of the results very sloppily.
Console.WriteLine("TOTAL GAMES: " + (wins + loses) + "\nWINS: " + wins + "\nLOSES: " + loses + "\nBIGGEST POT: $" + biggestStreak);
Console.WriteLine("WIN PERCENTAGE: " + Math.Round((100 * (double)wins / (double)loses), 4) + "%\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");

bool playGame(bool debug) {
    // Variable that is to be returned.
    bool win = true;

    // This represents the board of numbers remaining. When a number is eliminated, it is set to -100. Zero is present for single-number eliminations (i.e 12+0 = 12).
    List<int> numbers = new List<int> { 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };

    // This is used for comparison at the end of this method. A game which has resulted in a win will be equal to this list.
    List<int> winningGame = new List<int> { -100, -100, -100, -100, -100, -100, -100, -100, -100, -100, -100, -100, 0 };

    // Initializing dice.
    Random r1 = new Random();
    Random r2 = new Random();

    // Debug message.
    if (debug) {
        Console.WriteLine("\nNEW GAME");
    }

    // Main game loop.
    bool hasChoice = true;
    while (hasChoice) {
        bool found = false;

        // Rolling dice.
        int dice1 = r1.Next(1, 7);
        int dice2 = r2.Next(1, 7);
        int sum = dice1 + dice2;
        if (debug) {
            Console.WriteLine("\nSUM: " + sum);
        }

        // Nested loop to iterate to find two numbers available on the board which add to the sum.
        // These two numbers cannot be the same number.
        // The largest numbers are always prioritzed. (i.e Choosing 12+0 before 11+1 before 10+2, etc).
        for (int i = 0; i < numbers.Count && !found; i++) {
            for (int j = 6; j < numbers.Count && !found; j++) {

                // Are there two distinct numbers left on the board that add to the sum?
                if (numbers[i] + numbers[j] == sum && i != j) {
                    
                    // Debug message.
                    if (debug) {
                        string jNum = numbers[j] == 0 ? "" : "" + numbers[j];
                        Console.WriteLine("CHOSEN: " + numbers[i] + " " + jNum);
                    }

                    // `found` is a loop exit criteria. It means that a valid move exists.
                    found = true;

                    // Numbers eliminated from board. The `0` found in the list is always kept intact.
                    numbers[i] = -100;
                    numbers[j] = -100;
                    numbers[numbers.Count - 1] = 0;

                    // Debug message.
                    if (debug) {
                        Console.Write("STATUS: ");
                        for (int k = 0; k < numbers.Count; k++) {
                            if (numbers[k] != -100 && numbers[k] != 0) {
                                Console.Write(numbers[k] + " ");
                            }
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        // No move was made. The main game loop will break when there is no move to be made.
        if (!found) {
            hasChoice = false;
        }
    }

    // A comparison is made to determine whether or not the game resulted in a win or loss.
    if (numbers.SequenceEqual(winningGame)) {
        win = true;

        // Debug message.
        if (debug) {
            Console.WriteLine("WIN");
        }
    } else {
        win = false;

        // Debug message.
        if (debug) {
            Console.WriteLine("NO CHOICE\nLOSS");
        }
    }
    return win;
}