using UnityEngine.UI;
using UnityEngine;
using System;

public class UI_Information : MonoBehaviour
{
    public GameObject velocityText;
    public GameObject altitudeText;
    public GameObject fartScale;
    public GameObject fartPercentage;
    public GameObject distance;
    // Start is called before the first frame update
    void Start()
    {
        velocityText.GetComponent<Text>().text = "Velocidad: 0 km/h";
        altitudeText.GetComponent<Text>().text = "Altitud: 0 m";
        fartScale.GetComponent<Slider>().value = 1;
        fartPercentage.GetComponent<Text>().text = "100%";
        distance.GetComponent<Text>().text = "Distancia: 0 m";
    }

    // Update is called once per frame
    void Update()
    {
        velocityText.GetComponent<Text>().text = "Velocidad: " + Math.Round(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Physics>().speed, 1) + " km/h";
        altitudeText.GetComponent<Text>().text = "Altitud: " + Math.Round(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Physics>().transform.position.y + 3.3f, 1) + " m";
        fartScale.GetComponent<Slider>().value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Physics>().fartEnergy / 20f;
        fartPercentage.GetComponent<Text>().text = (Math.Round(GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Physics>().fartEnergy / 20f, 2) * 100) + "%";
        distance.GetComponent<Text>().text = "Distancia: " + Math.Round((GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Physics>().transform.position.x + 5.9f) / 1000f , 2) + " km";
    }
}
