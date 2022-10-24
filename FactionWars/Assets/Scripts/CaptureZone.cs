using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureZone : MonoBehaviour
{
    public Material blueMat;
    public Material redMat;

    private float StartingTime;
    public float realStartingtime;
    public static float totalTime;
    public static bool stopTextTimer { get; set; }
    public bool timerOn = false;

    public Text text;

    private float seconds;

  //  public List<GameObject> _BlueInZone = new List<GameObject>();
   // public List<GameObject> _RedInZone = new List<GameObject>();
    public int blueInZone = 0;
    public int redInZone = 0;

    //public List.<Collider> TriggerList = new List.<Collider>();

    public bool LeftZone;
    public bool RightZone;
    public bool leftzoneBool;
    private string team;


    public GameManager gameManager;

    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }

    void Start()
	{
        totalTime = realStartingtime;
	}

	private void Update()
	{
        if (gameObject.tag == "Player")
        {
            //  go.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = blueMat;
        }
        if (gameObject.tag == "Enemy")
        {
            gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = redMat;
        }

        if (!stopTextTimer)
		{
            totalTime -= Time.deltaTime;

            seconds = (int)(totalTime % 60);
            text.text = seconds.ToString();
		}



        if(seconds <= 0 && timerOn)
		{
            CheckTags();
            //Debug.Log("team is;" + team);
            if (team == "blue")
			{
                //Debug.Log("Player  capture zone");
                /*  Debug.Log("Player entered capture zone");
                  if (leftzoneBool)
                      LeftZone = false;
                  else
                      RightZone = false; */
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                gameObject.tag = "Player";
                gameObject.transform.GetChild(0).tag = "Player";
                GameManager.UpdateZones(team, leftzoneBool);
            }
            else if (team == "red")
            {
             //   Debug.Log("Enemy  capture zone");
                /*  gameObject.GetComponent<Renderer>().material.color = Color.red;
                  if (leftzoneBool)
                      LeftZone = true;
                  else
                     RightZone = false; */
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                gameObject.transform.GetChild(0).tag = "Enemy";
                gameObject.tag = "Enemy";
                GameManager.UpdateZones(team, leftzoneBool);
            }

        }
	}

    void ResetTextTimer()
	{
     //   Debug.Log("resetting timer");
        totalTime = 11;
        seconds = (int)(totalTime % 60);
        text.text = seconds.ToString();
	}        

	public void OnTriggerEnter(Collider other)
    {

	/*	if (!gameObject.CompareTag(other.transform.parent.parent.parent.tag)) 
        {
            gameObject.tag = "Neutral";
            gameObject.transform.GetChild(0).tag = GameConstants.NEUTRAL_TAG;
        } */

        //CheckTags();
        if (other.transform.parent.parent.parent.CompareTag("Player"))
        {
            // _BlueInZone.Add(other.gameObject);
           // blueInZone += 1;
            timerOn = true;
            stopTextTimer = false;
            ResetTextTimer();

          //  Debug.Log("Player hit capture zone");
        }
        else if (other.transform.parent.parent.parent.CompareTag("Enemy"))
        {
            //  _RedInZone.Add(other.gameObject);
           // redInZone += 1;
            timerOn = true;
            stopTextTimer = false;
            ResetTextTimer();

           // Debug.Log("Target hit capture zone");
        }
    }

	private void OnTriggerExit(Collider other)
	{

        if (other.transform.parent.parent.parent.CompareTag("Player"))
        {
            //_BlueInZone.Remove(other.gameObject);
           // blueInZone -= 1;
            timerOn = false; ;
            stopTextTimer = false;
        }

        if (other.transform.parent.parent.parent.CompareTag("Enemy"))
        {
           //    _RedInZone.Remove(other.gameObject);
           // redInZone += 1;
            timerOn = false;
            stopTextTimer = false;
        }
    }

	void CheckTags()
	{
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity);


        blueInZone = 0;
        redInZone = 0;
        gameObject.tag = GameConstants.NEUTRAL_TAG;
        gameObject.transform.GetChild(0).tag = GameConstants.NEUTRAL_TAG;

        foreach (var gameObject in hitColliders)
		{
          //  Debug.Log(gameObject.tag);
            if(gameObject.tag == "Player")
            {
                blueInZone += 1;
            }
             else if (gameObject.tag == "Enemy")
            {
                redInZone += 1;
            }
        }



        if (redInZone > 0 && blueInZone > 0)
		{
           /* gameObject.transform.GetChild(0).tag = GameConstants.NEUTRAL_TAG;
            gameObject.tag = GameConstants.NEUTRAL_TAG; */
            ResetTextTimer();
		}
        if (redInZone > 0 && blueInZone == 0)
		{
       //     Debug.Log("team set red");
         //   gameObject.tag = "Enemy";
            team = "red";

		}
        if (blueInZone > 0 && redInZone == 0)
        { 
       //     Debug.Log("team set blue");
         //   gameObject.tag = "Player";
            team = "blue";
		}
       // Debug.Log("reds:" + redInZone + "blues:" + blueInZone); 
        return;
    }
}
