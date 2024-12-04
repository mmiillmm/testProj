using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int money = 100;

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
