using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerCoin : MonoBehaviour, IDestructable
{
    [SerializeField] int points;
    public void Destroyed()
    {
        RollerGameManager.Instance.Score += points;
    }

}
