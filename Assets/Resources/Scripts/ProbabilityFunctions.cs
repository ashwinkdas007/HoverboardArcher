using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProbabilityFunctions { 

    //this function returns the number of possible permutations (distinct orderings) for "numOfElements" elements
    public static int Permutations(int numOfElements)
    {
        return Factorial(numOfElements);
    }

    //this function returns the number of possible variations (ordered subsets) of length "lengthOfVariation" selected from "numOfElements" elements
    public static int Variations(int numOfElements, int lengthOfVariation)
    {
        return Factorial(numOfElements) / Factorial(numOfElements - lengthOfVariation); //temp
    }

    //this function returns the number of combinations (unordered subsets) of length "lengthOfCombination" selected from "numOfElements" elements
    public static int Combinations(int numOfElements, int lengthOfCombination)
    {
        return Factorial(numOfElements) / (Factorial(numOfElements - lengthOfCombination) * Factorial(lengthOfCombination)); //temp
    }

    //this function returns a random int between 0 and "numberOfSides"
    public static int DiceRoll(int numOfSides)
    {
        return Random.Range(0, numOfSides);
    }


    //this function does the same as the previous overload, except it shifts the range of ints by "startsAt"
    public static int DiceRoll(int numOfSides, int startsAt)
    {
        return Random.Range(startsAt, startsAt + numOfSides);
    }

    //this function has a 1/n chance of returning true
    public static bool OneInNChance(int n)
    {
        return DiceRoll(n) == 0;
    }

    //this function has a 1/2 chance of returning true
    public static bool CoinToss()
    {
        return OneInNChance(2);
    }

    //this function has a "chanceOfTrue01" chance of returning true
    public static bool WeightedCoinToss(float chanceOfTrue01)
    {
        return Random.Range(0f, 1f) <= chanceOfTrue01;
    }

    //factorial
    public static int Factorial(int input)
    {
        if (input != 1)
        {
            return input * Factorial(input - 1);
        }
        else
        {
            return 1;
        }
    }

}
