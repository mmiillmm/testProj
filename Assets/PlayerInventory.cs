using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int money = 100; // Starting amount for the player, adjust as needed

    public void SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            Debug.Log("Money spent: " + amount);
        }
        else
        {
            Debug.Log("Not enough money.");
        }
    }
}
