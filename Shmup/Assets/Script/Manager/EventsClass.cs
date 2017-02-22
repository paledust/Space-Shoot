using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {
    public delegate void Handler(Event e);
}

public class EnemyDestroy: Event
{
    public EnemyType enemyType;

    public void setEnemyType(EnemyType _Type)
    {
        enemyType = _Type;
    }
}

public class EnemyWaveDestroy: Event
{
    //public delegate void Handler(PlayerShootEvent e);
}
