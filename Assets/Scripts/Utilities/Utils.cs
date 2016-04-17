using UnityEngine;
using System.Collections;

public class Utils : MonoBehaviour {

	public static float Angle(Vector2 p_vector2){
		if (p_vector2.x < 0){
			return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
		} else {
			return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
		}
	}

    public static Vector2 PolarToCartesian(Vector2 polar) {
        return new Vector2(polar.x * SinDegrees(polar.y), polar.x * CosDegrees(polar.y));
    }

    public static float SinDegrees(float radians) {
        return Mathf.Sin((radians * Mathf.PI) / 180);
    }

    public static float CosDegrees(float radians) {
        return Mathf.Cos((radians * Mathf.PI) / 180);
    }

    public static Vector2 Rotate(Vector2 v, float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
