using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Gameplay : MonoBehaviour
{

    // public GameObject bullet;
    public GameObject asteroid;
    // public GameObject asteroidContainer;
    public Text score;
    public GameObject rocket;
    public GameObject menu;

    private void OnEnable()
    {
        rocket.SetActive(true);
        rocket.transform.position = Vector3.zero;
        rocket.GetComponent<Rigidbody>().velocity = Vector3.zero;
        UnityEngine.Cursor.visible = false;
        
        asteroid.SetActive(false);
        CreateAsteroids(4);

        score.text = "Score: " + PlayerPrefs.GetInt("score", 0);

        InvokeRepeating("SpawnAsteroids", 5f, 5f);
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.8f);
        score.text = "Score: " + PlayerPrefs.GetInt("score", 0);
    }

    private void CreateAsteroids(float asteroidsNum)
    {
        for (int i = 1; i <= asteroidsNum; i++) {
            GameObject asteroidsClone = Instantiate(asteroid, new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)), transform.rotation);
            // asteroidsClone.transform.parent = asteroidContainer.transform;
            asteroidsClone.SetActive(true);
        }

    }

    private void SpawnAsteroids()
    {
        if (GameObject.Find("Asteroid(Clone)") == null)
            CreateAsteroids(2);
    }

     public void RocketFail()
    {
        if (PlayerPrefs.GetInt("maxScore", 0) < PlayerPrefs.GetInt("score", 0))
            PlayerPrefs.SetInt("maxScore", PlayerPrefs.GetInt("score"));
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetString("highScore", "High score: " + PlayerPrefs.GetInt("maxScore"));
        score.text = PlayerPrefs.GetString("highScore");
        rocket.SetActive(false);
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i]);
        }
        menu.SetActive(true);
        CancelInvoke();
        UnityEngine.Cursor.visible = true;
        gameObject.GetComponent<Gameplay>().enabled = false;
    }



}
