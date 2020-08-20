using TMPro;
using UnityEngine;

public class HealthGUI : MonoBehaviour
{
    public BasePlayer player;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ChangeText();
    }

    public void SetEntity(BasePlayer entity)
    {
        player = entity;
        ChangeText();
    }

    public void ChangeText()
    {
        text.text = player.health.ToString();
    }

    public void ChangeText(float amount)
    {
        text.text = amount.ToString();
    }

    public void ChangeText(string amount)
    {
        text.text = amount;
    }
}
