using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public GameObject rock;
    public GameObject gameplay;

    private float maxRotation;
    private float rotationX;
    private float rotationY;
    private float rotationZ;
    private Rigidbody rb;
    private Camera mainCam;
    private float maxSpeed;

    public int asteroidSize; // 3 = Большой, 2 = Средний, 1 = Маленький.
    public GameObject asteroidMedium;
    public GameObject asteroidSmall;
    public ParticleSystem smallExplosion;
    public AudioSource explosionSound;

    void Start()
    {

        mainCam = Camera.main;

        gameplay = GameObject.Find("Gameplay");

        maxRotation = 25f;
        rotationX = Random.Range(-maxRotation, maxRotation);
        rotationY = Random.Range(-maxRotation, maxRotation);
        rotationZ = Random.Range(-maxRotation, maxRotation);

        rb = rock.GetComponent<Rigidbody>();

        float speedX = Random.Range(200f, 600f);
        int selectorX = Random.Range(0, 2);
        float dirX = 0;
        if (selectorX == 1) { dirX = -1; }
        else { dirX = 1; }
        float finalSpeedX = speedX * dirX;
        rb.AddForce(transform.right * finalSpeedX);

        float speedY = Random.Range(200f, 800f);
        int selectorY = Random.Range(0, 2);
        float dirY = 0;
        if (selectorY == 1) { dirY = -1; }
        else { dirY = 1; }
        float finalSpeedY = speedY * dirY;
        rb.AddForce(transform.up * finalSpeedY);

    }

    void Update()
    {
        rock.transform.Rotate(new Vector3(rotationX, rotationY, 0) * Time.deltaTime);
        CheckPosition();
        float dynamicMaxSpeed = 3f;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -dynamicMaxSpeed, dynamicMaxSpeed), Mathf.Clamp(rb.velocity.y, -dynamicMaxSpeed, dynamicMaxSpeed));
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // print("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
        // print("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
        // print("Their relative velocity is " + collisionInfo.relativeVelocity);
        // print("gameObject.name: " + gameObject.name);
        print("collisionInfo.collider.name: " + collisionInfo.collider.name);

        if (collisionInfo.gameObject.CompareTag("Bullet"))
        {
            if(asteroidSize == 3)
            {
                // Спауним два средних астероида
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + 3);

                ParticleSystem explosion = Instantiate(smallExplosion, transform.position, transform.rotation);
                explosionSound = smallExplosion.GetComponent<AudioSource>();
                explosion.Play();
                explosionSound.Play();


                Instantiate(asteroidMedium, transform.position, transform.rotation);
                Instantiate(asteroidMedium, transform.position, transform.rotation);


                Destroy(explosion, explosion.duration);
                Destroy(gameObject);
                
            }
            else if(asteroidSize == 2)
            {
                // Спауним два маленьких астероида
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + 2);

                ParticleSystem explosion = Instantiate(smallExplosion, transform.position, transform.rotation);
                explosionSound = smallExplosion.GetComponent<AudioSource>();
                explosion.Play();
                explosionSound.Play();

                Instantiate(asteroidSmall, transform.position, transform.rotation);
                Instantiate(asteroidSmall, transform.position, transform.rotation);

                Destroy(explosion, explosion.duration);
                Destroy(gameObject);
               
            }
            else if(asteroidSize == 1)
            {
                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score", 0) + 1);

                ParticleSystem explosion = Instantiate(smallExplosion, transform.position, transform.rotation);
                explosionSound = smallExplosion.GetComponent<AudioSource>();
                explosion.Play();
                explosionSound.Play();
                // Уничтожаем астероид
                Destroy(explosion, explosion.duration);
                Destroy(gameObject);
                
            }

        }

        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            gameplay.GetComponent<Gameplay>().RocketFail();
        }
    }

    private void CheckPosition()
    {

        float rockOffset = 1;

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (rock.transform.position.x > sceneRightEdge + rockOffset)
        {
            rock.transform.position = new Vector2(sceneLeftEdge - rockOffset, rock.transform.position.y);
        }

        if (rock.transform.position.x < sceneLeftEdge - rockOffset)
        {
            rock.transform.position = new Vector2(sceneRightEdge + rockOffset, rock.transform.position.y);
        }

        if (rock.transform.position.y > sceneTopEdge + rockOffset)
        {
            rock.transform.position = new Vector2(rock.transform.position.x, sceneBottomEdge - rockOffset);
        }

        if (rock.transform.position.y < sceneBottomEdge - rockOffset)
        {
            rock.transform.position = new Vector2(rock.transform.position.x, sceneTopEdge + rockOffset);
        }

    }

    public void Destroy()
    {
        Destroy(gameObject, 0.01f);
    }

}
