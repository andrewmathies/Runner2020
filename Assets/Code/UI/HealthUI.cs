using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public int SpaceBetweenIcons;
    public int yPosition;

    public Sprite HeartSprite;
    public int Health;

    private Stack<GameObject> healthIcons;

    // Start is called before the first frame update
    void Start()
    {
        healthIcons = new Stack<GameObject>();

        for (int i = 0; i < Health; i++) {
            GameObject healthIconGO = new GameObject();
            healthIconGO.transform.parent = this.gameObject.transform;
            Image cur = healthIconGO.AddComponent<Image>();
            cur.sprite = HeartSprite;
            //cur.transform.position = 
            healthIconGO.transform.position = new Vector3(SpaceBetweenIcons + i * SpaceBetweenIcons, yPosition, 0f);
            healthIcons.Push(healthIconGO);
        }

        Debug.Log("displaying " + healthIcons.Count + " health icons");
    }

    // Update is called once per frame
    void Update()
    {
        if (healthIcons.Count != Health) {
            GameObject iconToRemove = healthIcons.Pop();
            Destroy(iconToRemove);
        }
    }
}
