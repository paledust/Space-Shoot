using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {
    public delegate void Handler(Event e);
}

public class EnemyDestroy: Event
{
    public EnemyType enemyType;
}

public class EnemyWaveDestroy: Event
{
    //public delegate void Handler(PlayerShootEvent e);
}

public class BossIn50Health: Event
{

}

public class CreateEnmey: Event
{
    public Transform _transform;
}

public class BossCreateEnemy: Event
{
    
}
