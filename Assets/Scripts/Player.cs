using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum SIDE { Left, Mid, Right }

public class Player : MonoBehaviour
{
    private CharacterController controller;

    private float jumpSpeed;

    float NewXPos = 0f;
    public float speed;
    public float jumpHeight;
    public float gravity = -20;
    public float horizontalSpeed;
    public SIDE m_Side = SIDE.Mid;
    public float XValue = 1.5f;

    private float x;

    public Animator anim;

    public Text word;
    private int score = 0;

    public GameObject[] spawnLetters;
    private float[] xPositions = new float[3] { -1.5f, 0f, 1.5f };
    int randomLetter, randomPosition;
    public float spawntime;
    public float spawndelay;

    private string wordIncomplete;
    private int currentLetter = 0;

    private string[] dict = new string[7] {"bread", "cheese", "cherry", "fish", "olive", "pie", "ribs"};
    private string[] dictadvanced = new string[7] {"screwdriver", "hammer", "pliers", "screws", "spanner", "hacksaw", "axe"};

    public AudioClip[] audiosList;
    public AudioSource audio;
    private int currentWord;

    private int[] wordLetters;
    private bool[] capturedLetters;
    int correctLetter = -1;

    public float rayRadius;
    public LayerMask letterLayer;
    public LayerMask obstacleLayer;

    public bool isDead = false;
    private int maxSpeed;

    public int completeWordFlag = 0;

    private char[] aux;

    private GameController gc;

    // Start is called before the first frame update
    void Start()
    {      
        gc = FindObjectOfType<GameController>();
        if (gc.difficulty == 0) {
            speed = 8;
            maxSpeed = 18;
        } else {
            speed = 16;
            maxSpeed = 28;
        }
        generateNewWord();
        controller = GetComponent<CharacterController>();
        transform.position = Vector3.zero;
        anim = GetComponent<Animator>();
        anim.SetBool("Jumping", false);
        anim.SetBool("Dead", false);
    }

    void generateNewWord() {
        CancelInvoke("SpawnRandom");
        System.Random r = new System.Random();
        currentWord = r.Next(0, dict.Length);
        if (gc.difficulty == 0) { wordIncomplete = dict[currentWord]; } else { wordIncomplete = dictadvanced[currentWord]; }
        wordLetters = new int[wordIncomplete.Length];
        capturedLetters = new bool[wordIncomplete.Length];
        word.text = wordIncomplete;

        var goArray = FindObjectsOfType<GameObject>();
        var goList = new System.Collections.Generic.List<GameObject>();
        for (var i = 0; i < goArray.Length; i++) {
            if (goArray[i].layer == letterLayer) {
                Destroy(goArray[i]);
            }
        }
        for(int i=0; i< wordIncomplete.Length; i++)
        {
            wordLetters[i] = Array.FindIndex(spawnLetters, letter => letter.name == wordIncomplete[i].ToString());
        }

        InvokeRepeating("SpawnRandom", spawntime, spawndelay);
    }

    void SpawnRandom()
    {
        randomLetter = UnityEngine.Random.Range(0, wordLetters.Length);
        randomPosition = UnityEngine.Random.Range(0, xPositions.Length);
        //Debug.LogWarning(spawnLetters[randomLetter]);
        Instantiate(spawnLetters[wordLetters[randomLetter]], new Vector3(xPositions[randomPosition], 1f, transform.position.z+100f), new Quaternion(0f, 0f, 0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && speed < maxSpeed) {
            speed += 0.001f;
        }
        
        Vector3 direction = Vector3.forward * speed;

        if (controller.isGrounded)
        {
            anim.SetBool("Jumping", false);
            jumpSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpSpeed += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                anim.SetBool("Jumping", true);
            }
        }
        else
        {
            jumpSpeed += gravity * Time.deltaTime;

        }



        if (Input.GetKeyDown(KeyCode.A) && controller.transform.position.x > -2.2f)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXPos = -XValue;
                m_Side = SIDE.Left;
            }
            else if (m_Side == SIDE.Right)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && controller.transform.position.x < 2.2f)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXPos = XValue;
                m_Side = SIDE.Right;
            }
            else if (m_Side == SIDE.Left)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }

        }

        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * horizontalSpeed);
        controller.Move((x - transform.position.x) * Vector3.right);
        direction.y = jumpSpeed;
        controller.Move(direction * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.LogWarning(collision.gameObject.tag);

        // se for uma letra
        if (collision.gameObject.tag == "Letter") {
            Debug.LogWarning("Letra " + collision.gameObject.name);

            String[] name = collision.gameObject.name.Split('(');
            aux = new char[wordIncomplete.Length];

            correctLetter = -1;

            for (int i=0; i<wordIncomplete.Length; i++)
            {
                if (wordIncomplete[i].ToString() == name[0] &&
                    capturedLetters[i] == false)
                {
                    capturedLetters[i] = true;
                    gc.score += 50;
                    completeWordFlag++;
                    correctLetter = i; // controle para saber se a letra j� foi capturada
                    Debug.LogWarning("CAPTUROU");

                    i = wordIncomplete.Length;
                }
            }

            if (correctLetter != -1) {
                string str = "";
                for (int i = 0; i < wordIncomplete.Length; i++) {
                    if (capturedLetters[i] == true) {
                        str += "<color=lime>" + wordIncomplete[i] + "</color>";
                    } else {
                        str += wordIncomplete[i];
                    }
                }

                word.text = str;

                if(completeWordFlag == capturedLetters.Length)
                {
                    gc.score += 300;
                    audio.clip = audiosList[currentWord];
                    audio.Play();
                    completeWordFlag = 0;
                    generateNewWord();
                }

            }

            Destroy(collision.gameObject);

        } if (collision.gameObject.tag == "Object" && !isDead) {
            Debug.LogWarning("OBJETO");

            isDead = true;
            anim.SetBool("Dead", true);
            speed = 0;
            jumpHeight = 0;
            horizontalSpeed = 0;

            Invoke("GameOver", 0.5f);
        }

    }

    public void GameOver() {
        gc.ShowGameOver();
    }

}