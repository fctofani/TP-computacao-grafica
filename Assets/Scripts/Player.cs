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

    private string wordIncomplete = "cake";
    private int currentLetter = 0;

    private int[] wordLetters;
    private bool[] capturedLetters;
    int correctLetter = -1;

    public float rayRadius;
    public LayerMask letterLayer;

    private char[] aux;

    // Start is called before the first frame update
    void Start()
    {
        wordLetters = new int[wordIncomplete.Length];
        capturedLetters = new bool[wordIncomplete.Length];
        word.text = wordIncomplete;

        
        for(int i=0; i< wordIncomplete.Length; i++)
        {
            wordLetters[i] = Array.FindIndex(spawnLetters, letter => letter.name == wordIncomplete[i].ToString());
        }

        InvokeRepeating("SpawnRandom", spawntime, spawndelay);
        controller = GetComponent<CharacterController>();
        transform.position = Vector3.zero;
        anim = GetComponent<Animator>();
        anim.SetBool("Jumping", false);
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

        OnCollision();

        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * 10f);
        controller.Move((x - transform.position.x) * Vector3.right);
        direction.y = jumpSpeed;
        controller.Move(direction * Time.deltaTime);
        //Debug.LogWarning(wordIncomplete[2]);
    }

    void OnCollision()
    {
        RaycastHit letterHit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0f,1f,0f)), out letterHit, rayRadius, letterLayer))
        {
            Debug.LogWarning("ENCOSTOU");

            //Debug.LogWarning(letterHit.transform.gameObject.name);

            String[] name = letterHit.transform.gameObject.name.Split('(');
            aux = new char[wordIncomplete.Length];

            correctLetter = -1;

            for (int i=0; i<wordIncomplete.Length; i++)
            {
                if(wordIncomplete[i].ToString() == name[0] &&
                    capturedLetters[i] == false)
                {
                    capturedLetters[i] = true;
                    correctLetter = i; // controle para saber se a letra já foi capturada
                    Debug.LogWarning("CAPTUROU");

                    i = wordIncomplete.Length;
                }
            }

            if(correctLetter != -1)
            {
                for (int i = 0; i < wordIncomplete.Length; i++)
                {
                    if(correctLetter != i)
                    {
                        aux[i] = word.text[i];
                    } else
                    {
                        aux[i] = Char.ToUpper(word.text[i]);
                    }
                }

                string str = new string(aux);

                word.text = str;

            }

            Destroy(letterHit.transform.gameObject);

        }

    }

}