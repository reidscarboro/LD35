using UnityEngine;
using System.Collections;

public class Enemy : Killable {

    public int minLevelToAppear = 0;

    protected override void Kill() {
        GameController.DecrementEnemies();
    }
}
