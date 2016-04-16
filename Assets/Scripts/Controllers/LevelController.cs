using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

    public int[,] level;

	// Use this for initialization
	void Start () {
        BuildLevel(3);
	}

    private void BuildLevel(int size) {
        level = new int[size, size];
    } 
}
