using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPoolable
{
    void OnSpawned();
    void OnDeSpawned();
}