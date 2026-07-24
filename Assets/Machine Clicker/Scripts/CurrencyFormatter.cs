using UnityEngine;

public static class CurrencyFormatter 
{
    
    
    public static string FormatCurrency(long amount)
    {
        if (amount >= 1_000_000_000)
            return (amount / 1_000_000_000f).ToString("0.#") + "B"; // billions
        else if (amount >= 1_000_000)
            return (amount / 1_000_000f).ToString("0.#") + "M";     // millions
        else if (amount >= 1_000)
            return (amount / 1_000f).ToString("0.#") + "K";         // thousands
        else
            return amount.ToString();
    }
}
