using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// consistent updates of UI Conditions
public class UICondition : MonoBehaviour
{
    public Condition health;

    public Condition hunger;

    public Condition stamina;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.Condition.uiCondition = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}