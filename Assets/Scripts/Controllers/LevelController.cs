using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

    public int levelSize;
    public int ceilingHeight = 50;

    public List<GameObject> trees;

    public BoxCollider2D groundCollider;
    public GameObject ground_TL;
    public GameObject ground_T;
    public GameObject ground_TR;
    public GameObject ground_L;
    public GameObject ground;
    public GameObject ground_R;
    public GameObject ground_BL;
    public GameObject ground_B;
    public GameObject ground_BR;

    // Use this for initialization
    void Start () {
        BuildLevel(0);
	}

    private void BuildLevel(int level) {

        int width = 0;
        int previousHeight = 0;
        int groundNodeWidth = Random.Range(3, 15);
        int groundNodeHeight = Random.Range(3, 8);


        while (width < levelSize) {

            groundNodeWidth = Random.Range(3, 15);
            while (groundNodeHeight == previousHeight) {
                groundNodeHeight = Random.Range(3, 8);
            }
            previousHeight = groundNodeHeight;


            BoxCollider2D newGroundCollider = (BoxCollider2D) Instantiate(groundCollider, new Vector3(width + (float)groundNodeWidth / 2, (float)groundNodeHeight / 2, 0), Quaternion.identity);
            newGroundCollider.size = new Vector2(groundNodeWidth, groundNodeHeight);

            Vector3 tileOffset = new Vector3(width - 0.5f, -0.5f, 0);
            for (int y = 1; y <= groundNodeHeight; y++) {
                for (int x = 1; x <= groundNodeWidth; x++) {
                    Vector3 tilePosition = new Vector3(x, y, 0);
                    if (y == groundNodeHeight) {
                        if (x == 1) {
                            Instantiate(ground_TL, tilePosition + tileOffset, Quaternion.identity);
                        } else if (x == groundNodeWidth) {
                            Instantiate(ground_TR, tilePosition + tileOffset, Quaternion.identity);
                        } else {
                            Instantiate(ground_T, tilePosition + tileOffset, Quaternion.identity);
                            TryNewTree(tilePosition + tileOffset);
                        }
                    } else if (y == 1) {
                        if (x == 1) {
                            Instantiate(ground_BL, tilePosition + tileOffset, Quaternion.identity);
                        } else if (x == groundNodeWidth) {
                            Instantiate(ground_BR, tilePosition + tileOffset, Quaternion.identity);
                        } else {
                            Instantiate(ground_B, tilePosition + tileOffset, Quaternion.identity);
                        }
                    } else {
                        if (x == 1) {
                            Instantiate(ground_L, tilePosition + tileOffset, Quaternion.identity);
                        } else if (x == groundNodeWidth) {
                            Instantiate(ground_R, tilePosition + tileOffset, Quaternion.identity);
                        } else {
                            Instantiate(ground, tilePosition + tileOffset, Quaternion.identity);
                        }
                    }
                } 
            }

            width += groundNodeWidth;
        }

        BoxCollider2D ceilingCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3((float)width / 2, ceilingHeight, 0), Quaternion.identity);
        ceilingCollider.size = new Vector2(width, 1);

        BoxCollider2D leftWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(-0.5f, (float) ceilingHeight / 2, 0), Quaternion.identity);
        leftWallCollider.size = new Vector2(1, ceilingHeight);

        BoxCollider2D rightWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(width + 0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
        rightWallCollider.size = new Vector2(1, ceilingHeight);

        Camera.main.GetComponent<FollowCamera>().maxY = ceilingHeight;
        Camera.main.GetComponent<FollowCamera>().maxX = width;

    }

    public void TryNewTree(Vector3 groundTile) {
        if (Random.Range(0, 1.0f) > 0.6f) {
            GameObject tree = (GameObject) Instantiate(trees[Random.Range(0, trees.Count)], groundTile + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }
}
