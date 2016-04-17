using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sign : MonoBehaviour {

    private float triggerTooltipDistance = 1;

    public GameObject tooltip;
    public List<TextMesh> line1;
    public List<TextMesh> line2;
    public List<TextMesh> line3;

    // Update is called once per frame
    void Update () {
	    if (Vector3.Distance(ObjectController.GetPlayer().transform.position, transform.position) < triggerTooltipDistance) {
            if (!tooltip.gameObject.activeInHierarchy) tooltip.SetActive(true);
        } else {
            if (tooltip.gameObject.activeInHierarchy) tooltip.SetActive(false);
        }
	}

    public void Set(string _line1, string _line2, string _line3) {
        foreach (TextMesh mesh in line1) {
            mesh.text = _line1;
        }
        foreach (TextMesh mesh in line2) {
            mesh.text = _line2;
        }
        foreach (TextMesh mesh in line3) {
            mesh.text = _line3;
        }
    }
}
