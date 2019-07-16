using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Cactus_Generation : MonoBehaviour
{
    public GameObject Player;
    public GameObject cactus1;
    private int maxCactus;

    private List<GameObject> cactusList;
    private Queue<GameObject> cactusesInScene;

    // Start is called before the first frame update
    void Start()
    {
        cactusList = new List<GameObject>();
        cactusesInScene = new Queue<GameObject>();
        maxCactus = 10;
        for (int i = 0; i < maxCactus; i++) {
            GameObject cactus = Instantiate(cactus1, transform);
            cactus.SetActive(false);
            cactusList.Add(cactus);
        }
    }

    //corregir este cagadero por transform.localPosition
    void Update()
    {
        bool playerIsLaunched = Player.GetComponent<Player_Physics>().IsLaunched;
        float playerSpeed = Player.GetComponent<Player_Physics>().speed;
        if (playerIsLaunched ){
            if (playerSpeed > 20f) {
                if (Player.transform.position.y < 50f) {
                    if (cactusList.Count > 0) {
                        float newCactusProbability = Random.Range(0f, 100f);
                        if (newCactusProbability < 1f) {
                            GameObject cactus = cactusList[0];
                            Vector3 cactusNewPos = Player.transform.position + (Vector3.right * 20f);
                            cactusNewPos.z = 10f;
                            cactusNewPos.y = Random.Range(cactusNewPos.y - 4f, cactusNewPos.y + 4f);
                            if(Player.transform.position.y < 3) {
                                cactusNewPos.y += 4f;
                            }
                            cactus.transform.position = cactusNewPos;
                            cactus.SetActive(true);
                            cactusesInScene.Enqueue(cactus);
                            cactusList.RemoveAt(0);
                        }
                    }
                }
            }
            if (cactusesInScene.Count > 0) {
                foreach (var cactus in cactusesInScene) {
                    Vector3 newPos = new Vector3(cactus.transform.position.x + (-.35f * Time.deltaTime * playerSpeed), cactus.transform.position.y, 0);

                    cactus.transform.position = newPos;
                }
                if (cactusesInScene.Peek().transform.position.x < Player.transform.position.x - 15f) {
                    cactusesInScene.Peek().SetActive(false);
                    cactusList.Add(cactusesInScene.Dequeue());
                }
            }
        }
    }
}
