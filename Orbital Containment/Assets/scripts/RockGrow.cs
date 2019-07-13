using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGrow : Rock
{
    private readonly float growthRateAlive = 0.15f;

    // Use this for initialization
    void Start()
    {
        UpdateStarting();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRock();
    }

    //Update for the alive state
    protected override void UpdateAlive()
    {
        this.UpdateShadow();

        AddToScale(this.growthRateAlive * Time.deltaTime);
    }

    //Add to the size of this rock
    public void AddToScale(float value)
    {
        var newScale = this.transform.localScale.x + value;
        this.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
}
