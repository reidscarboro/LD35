using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

    public int levelSize;
    public int ceilingHeight = 50;

    private int minX = 20;

    public List<GameObject> trees;
    public List<GameObject> houses;
    public List<Enemy> groundEnemies;
    public Enemy boss;
    public GameObject castle;

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

    public Sign sign;

    void Start () {
        BuildLevel(GameController.GetLevelIndex());
	}

    private void BuildLevel(int level) {
        if (level == 0) {
            int width = 60;

            CreateIsland(0, 0, 20, 4, false, false);
            CreateIsland(20, 0, 8, 6, false, false);
            CreateIsland(28, 0, 8, 3, false, false);
            CreateIsland(36, 0, 4, 9, false, false);
            CreateIsland(40, 0, 20, 5, false, false);

            CreateSign(new Vector3(2, 4.5f, 0), "Welcome!", "[ A ]  and  [ D ]", "to move");
            CreateSign(new Vector3(10, 4.5f, 0), "Act 0:", "Birdperson Lives", "");
            CreateSign(new Vector3(15, 4.5f, 0), "[ SPACE ]", "to jump", "");
            CreateSign(new Vector3(30, 3.5f, 0), "[ Right Click ]", "to shapeshift", "");
            CreateSign(new Vector3(39, 9.5f, 0), "[ Left Click ]", "to shoot an arrow", "");

            Instantiate(groundEnemies[0], new Vector3(50, 5, 0), Quaternion.identity);
            GameController.IncrementEnemies();

            BoxCollider2D ceilingCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3((float)width / 2, ceilingHeight, 0), Quaternion.identity);
            ceilingCollider.size = new Vector2(width, 1);

            BoxCollider2D leftWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(-0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
            leftWallCollider.size = new Vector2(1, ceilingHeight);

            BoxCollider2D rightWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(width + 0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
            rightWallCollider.size = new Vector2(1, ceilingHeight);

            Camera.main.GetComponent<FollowCamera>().maxY = ceilingHeight;
            Camera.main.GetComponent<FollowCamera>().maxX = width;

            GameController.DoneLoading();
        } else if (level == 3) {
            int width = 60;
            CreateIsland(0, 0, width, 4, false, false);
            Instantiate(castle, new Vector3(12, 0, 0), Quaternion.identity);
            string line1 = "Act " + GameController.GetRawLevel().ToString() + ":";
            string line2 = "Bring out your Dead";
            string line3 = "";
            CreateSign(new Vector3(5, 4.5f, 0), line1, line2, line3);

            for (int i = 0; i < GameController.GetDifficulty(); i++) {
                Instantiate(boss, new Vector3(width / 2 + Random.Range(-5, 5), ceilingHeight / 2 + Random.Range(-5, 5), 0), Quaternion.identity);
                GameController.IncrementEnemies();
            }

            BoxCollider2D ceilingCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3((float)width / 2, ceilingHeight, 0), Quaternion.identity);
            ceilingCollider.size = new Vector2(width, 1);

            BoxCollider2D leftWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(-0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
            leftWallCollider.size = new Vector2(1, ceilingHeight);

            BoxCollider2D rightWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(width + 0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
            rightWallCollider.size = new Vector2(1, ceilingHeight);

            Camera.main.GetComponent<FollowCamera>().maxY = ceilingHeight;
            Camera.main.GetComponent<FollowCamera>().maxX = width;

            GameController.DoneLoading();
        } else {
            int width = 0;
            int height = 0;
            int previousHeight = 0;
            int groundNodeWidth = Random.Range(4, 10);
            int groundNodeHeight = Random.Range(3, 8);

            int islandCounter = 0;

            while (width < levelSize) {
                groundNodeWidth = Random.Range(4, 10);
                while (groundNodeHeight == previousHeight) {
                    groundNodeHeight = Random.Range(3, 8);
                }

                previousHeight = groundNodeHeight;
                CreateIsland(width, height, groundNodeWidth, groundNodeHeight, islandCounter == 0, true);

                islandCounter++;

                width += groundNodeWidth;
            }

            BoxCollider2D ceilingCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3((float)width / 2, ceilingHeight, 0), Quaternion.identity);
            ceilingCollider.size = new Vector2(width, 1);

            BoxCollider2D leftWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(-0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
            leftWallCollider.size = new Vector2(1, ceilingHeight);

            BoxCollider2D rightWallCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(width + 0.5f, (float)ceilingHeight / 2, 0), Quaternion.identity);
            rightWallCollider.size = new Vector2(1, ceilingHeight);

            Camera.main.GetComponent<FollowCamera>().maxY = ceilingHeight;
            Camera.main.GetComponent<FollowCamera>().maxX = width;

            if (level > 0) {
                //build islands
                int numIslands = Random.Range(3, 5);
                width = minX + Random.Range(0, 20);
                while (width < levelSize - 10) {
                    groundNodeWidth = Random.Range(5, 10);
                    groundNodeHeight = Random.Range(3, 4);

                    height = Random.Range(10, 15);

                    CreateIsland(width, height, groundNodeWidth, groundNodeHeight, false, true);

                    width += groundNodeWidth + Random.Range(0, 10);
                }
            }

            GameController.DoneLoading();
        }
    }

    public void CreateIsland(int width, int height, int groundNodeWidth, int groundNodeHeight, bool hasSign, bool spawnEnemy) {
        
        BoxCollider2D newGroundCollider = (BoxCollider2D)Instantiate(groundCollider, new Vector3(width + (float)groundNodeWidth / 2, height + (float)groundNodeHeight / 2, 0), Quaternion.identity);
        newGroundCollider.size = new Vector2(groundNodeWidth, groundNodeHeight);
        Vector3 tileOffset = new Vector3(width - 0.5f, height - 0.5f, 0);

        int levelIndex = GameController.GetLevelIndex();
        int numLevels = GameController.GetNumLevels();
        int rawLevel = GameController.GetRawLevel();

        if (hasSign) {

            string line1 = "Act "+ rawLevel.ToString() + ":";
            string line2 = "";
            string line3 = "";
            
            switch (levelIndex) {
                case 0://tutorial
                    line2 = "Birdperson Lives";
                    break;
                case 1://jungle
                    line2 = "Welcome to the";
                    line3 = "Jungle";
                    break;
                case 2://village
                    line2 = "A Settlement Needs";
                    line3 = "Your Help";
                    break;
                case 3://castle
                    line2 = "Bring out your Dead";
                    break;

            }
            CreateSign(new Vector3(groundNodeWidth / 2, groundNodeHeight + 1, 0) + tileOffset, line1, line2, line3);
        }

        
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
                        CreateDecor(tilePosition + tileOffset);
                        if (spawnEnemy && levelIndex < numLevels) TryNewGroundEnemy(tilePosition + tileOffset);
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

        if (levelIndex == numLevels) {
            //spawn bosses
        }
    }

    public void CreateDecor(Vector3 groundTile) {
        int levelIndex = GameController.GetLevelIndex();
        if (levelIndex == 0 || levelIndex == 1) {
            TryNewTree(groundTile);
        } else if (levelIndex == 2 || levelIndex == 3) {
            if (Random.Range(0, 1.0f) > 0.5f) {
                TryNewHouse(groundTile);
            } else {
                TryNewTree(groundTile);
            }
        }
    }

    public void TryNewTree(Vector3 groundTile) {
        if (Random.Range(0, 1.0f) < 0.6f) {
            Instantiate(trees[Random.Range(0, trees.Count)], groundTile + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

    public void TryNewHouse(Vector3 groundTile) {
        if (Random.Range(0, 1.0f) < 0.4f) {
            Instantiate(houses[Random.Range(0, houses.Count)], groundTile + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }

    public void TryNewGroundEnemy(Vector3 groundTile) {
        int levelIndex = GameController.GetLevelIndex();
        int difficulty = GameController.GetDifficulty();
        int enemyIndex = levelIndex - 1;
        if (enemyIndex < 0) enemyIndex = 0;
        bool enemyCreated = false;
        if (groundTile.x > minX && Random.Range(0, 1.0f) > (0.9f - 0.1f * difficulty)) {
            Instantiate(groundEnemies[enemyIndex], groundTile + new Vector3(0, 0.5f, 0), Quaternion.identity);
            GameController.IncrementEnemies();
            enemyCreated = true;
        }
        if (!enemyCreated && groundTile.x > minX && Random.Range(0, 1.0f) > (0.9f - 0.1f * difficulty)) {
            Instantiate(groundEnemies[Random.Range(0, enemyIndex)], groundTile + new Vector3(0, 0.5f, 0), Quaternion.identity);
            GameController.IncrementEnemies();
        }
    }

    private void CreateSign(Vector3 position, string line1, string line2, string line3) {
        Sign newSign = (Sign)Instantiate(sign, position, Quaternion.identity);
        newSign.Set(line1, line2, line3);
    }
}
