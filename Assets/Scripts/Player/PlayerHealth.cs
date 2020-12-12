using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float Health;

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health < 0)
        {
            Health = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Dead!");
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, Screen.height-150f, 100, 150f));
        GUILayout.Label($"HEALTH: {(int)Health}");
        GUILayout.EndArea();
    }
}
