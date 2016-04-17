using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

    void Start() {
        UnityEngine.Cursor.visible = false;
    }
	
	void Update () {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        transform.position = mouseWorldPosition;
    }
}
