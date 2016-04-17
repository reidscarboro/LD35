using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

    protected static ObjectController instance;

    public Player player;
    public Arrow arrow;
    public GameObject arrowHit;

	void Start () {
        instance = this;
    }
	
	public static void CreateArrow(Vector3 position, Vector2 direction, float velocity) {
        Vector2 angleVector = direction;
        angleVector.y *= -1;
        Arrow newArrow = (Arrow) Instantiate(instance.arrow, position, Quaternion.AngleAxis(Utils.Angle(angleVector) - 90, Vector3.forward));
        newArrow.Spawn(direction, velocity);
    }

    public static void CreateArrowHit(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject newArrowHit = (GameObject) Instantiate(instance.arrowHit, position, rotation);
        newArrowHit.transform.SetParent(parent);
    }

    public static void SetPlayer(Player _player) {
        instance.player = _player;
    }

    public static Player GetPlayer() {
        return instance.player;
    }
}
