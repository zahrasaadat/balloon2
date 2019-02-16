using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//[RequireComponent(typeof(AudioSource))]
public class BalloonSpawner : MonoBehaviour {
    public GameObject[] badkonakha = new GameObject[10];
    public GameObject[] dast = new GameObject[10];
    public GameObject boom1;
    public GameObject boom2;
    public GameObject boom3;
    public GameObject board;
    public GameObject correctBalloon;
    public GameObject MainCamera;
    Balloon[] balloons = new Balloon[10];
    Hand[] hands = new Hand[10];
    float[,] ballonpositions = { {-6, 1.8f ,0 }, { -2, 1.8f, 0 } , { 2, 1.8f, 0 }, { 6, 1.8f, 0 }, { -6, -2.6f, 0 }, { -2, -2.6f, 0 } };
    Hand chosenHand;
    Vector3 touchPosWorld;
    GameObject touchedObject;
    TouchPhase touchPhase = TouchPhase.Ended;
    public float Timeouttime = 4.0f;
    public float Timecounter=0.0f;
    // public Animator myanim;
    public AudioClip balloonPop;
    public AudioSource audios;
    public AudioClip wrong;
    private bool b;
    public class Balloon
    {
        public GameObject BalloonImage;
        public int num;
        public Balloon(GameObject b, int n)
        {
            BalloonImage = b;
            num = n;
           
        }

    }
    public class Hand
    {
        public GameObject HandImage;
        public int number;
       
        public Hand(GameObject b, int n)
        {
            HandImage = b;
            number = n;
          
        }

    }
    private void Awake()
    {

        audios = GetComponent<AudioSource>();
        balloonPop = (AudioClip)Resources.Load("Assets/prefabs/pop");
        wrong = (AudioClip)Resources.Load("Assets/prefabs/wrong");
        for (int i = 0; i < 10; i++)
        {
            balloons[i] = new Balloon(badkonakha[i], i + 1);
        }
        for (int i = 0; i < 10; i++)
        {
            hands[i] = new Hand(dast[i], i + 1);
        }
        makeRandomBoard();
    }
    void Start () {
        Timecounter = 0.0f;
       
    }
    void Update()
    {

        b = BalloonGame();
        if (b)
        {

            audios.clip=balloonPop;
            audios.Play();
          
            Destroy(touchedObject);
            GameObject BOOM3= Instantiate(boom3, correctBalloon.transform.position, boom1.transform.rotation);
            GameObject BOOM2 = Instantiate(boom2, correctBalloon.transform.position, boom2.transform.rotation);
            GameObject BOOM1 = Instantiate(boom1, correctBalloon.transform.position, boom3.transform.rotation);
            StartCoroutine(waiter(BOOM1,BOOM2,BOOM3));
          
            
        }
        else
        {
           audios.clip = wrong;
            audios.Play();
        }
       
    }
    bool BalloonGame()
    {
        if (Input.touchCount==1 && Input.GetTouch(Input.touchCount-1).phase == touchPhase)
        {
            touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            if (hitInformation.collider != null)
            {
                touchedObject = hitInformation.transform.gameObject;
                Debug.Log("Touched " + touchedObject.transform.name);
            }
            if (touchedObject == correctBalloon)
            {
                return true;
                
            }
        }
        return false;
    }
    private void FixedUpdate()
    {
        
    }
    IEnumerator waiter(GameObject g1, GameObject g2, GameObject g3)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(g1);
        yield return new WaitForSeconds(0.2f);
        Destroy(g2);
        yield return new WaitForSeconds(0.2f);
        Destroy(g3);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("main");
    }

    public void DeleteAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if(o!=board && o!=MainCamera )
                Destroy(o);
        }
    }
    void makeRandomBoard()
    {
        int pb = Random.Range(0, 5);
        float[] balloonPos = { ballonpositions[pb, 0], ballonpositions[pb, 1], ballonpositions[pb, 2] };
        int wh = Random.Range(0, 9);//which hand?
        chosenHand = hands[wh];
        Instantiate(chosenHand.HandImage, new Vector3(4,-2.6f,0), chosenHand.HandImage.transform.rotation);
        correctBalloon =Instantiate(balloons[wh].BalloonImage, new Vector3(balloonPos[0], balloonPos[1], 0), balloons[wh].BalloonImage.transform.rotation);
      
        List<int> randomList1 = new List<int>();//for the positions
        randomList1.Add(pb);
        List<int> randomList2 = new List<int>();//for the balloons
        randomList2.Add(wh);
        int j = 0;
        while (j < 5)
        {
            int wpb = Random.Range(0, 6); //wrong balloons position!
            int wb= Random.Range(0, 10);//which balloon?
            if (!randomList1.Contains(wpb) && !randomList2.Contains(wb))
            {
             
                Instantiate(balloons[wb].BalloonImage, new Vector3(ballonpositions[wpb, 0], ballonpositions[wpb, 1], 0), balloons[wb].BalloonImage.transform.rotation);
                randomList1.Add(wpb);
                randomList2.Add(wb);
                j++;
            }
        }
    }
}
