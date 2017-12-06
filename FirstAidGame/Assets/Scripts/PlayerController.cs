using UnityEngine;
// Include the namespace required to use Unity UI
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public MenuController gameController;

	public Text timeText;
    public Text onScreenText;
    public Text scoreText;
    public GameObject miniMap;
    public GameObject playerOnMiniMap;
    public GameObject enemyOnMiniMap;
    public GameObject indicator_Step1;
    public GameObject indicator_Step2;
    public GameObject indicator_Step3;

    public GameObject recovery_instructions;
    public GameObject step1_description;
    public GameObject step2_description;
    public GameObject step3_description;
    public GameObject step1_picture;
    public GameObject step2_picture;
    public GameObject step3_picture;
    
    public GameObject enemy;
    public GameObject option1_button;
    public GameObject option2_button;
    public GameObject option3_button;

    public Camera fpsCamera;
    private Rigidbody rb; // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far

	private int count_collect_first;
    private int count_collect_second;
    private int count_collect_final;
    private bool collect_first;
    private bool collect_second;
    private bool collect_final;
    private int count_completed;
    private bool streak_completed;
    private float indicator_wait;
    private bool mistake;

    private float mapZsize;
    private float minimapYsize;
    private float ratioMapToMinimap;

    private bool preGame;
    private float speed;
    private bool game_running;
    private float timeRunning;

    private float enemyZpos;
    private float ENEMY_SPEED = 2.5f;
    private float initialEnemyZPos;

    //EXAM
    private float quiz_ZPos;
    private Vector2[] steps_desc_positions;
    private Vector2[] steps_pic_positions;
    private int rand_index1, rand_index2, rand_index3;
    private bool gotFirstStep;
    private bool gotSecondStep;
    private bool passedQuiz;
    private bool didQuiz;

    // At the start of the game..
    void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

        mapZsize = 300.0f;
        quiz_ZPos = mapZsize * 0.5f;
        minimapYsize = miniMap.GetComponent<RectTransform>().rect.height;
        ratioMapToMinimap = minimapYsize / mapZsize;

        speed = 7.5f;

        steps_desc_positions = new Vector2[3];
        steps_desc_positions[0] = step1_description.GetComponent<RectTransform>().anchoredPosition;
        steps_desc_positions[1] = step2_description.GetComponent<RectTransform>().anchoredPosition;
        steps_desc_positions[2] = step3_description.GetComponent<RectTransform>().anchoredPosition;

        steps_pic_positions = new Vector2[3];
        steps_pic_positions[0] = step1_picture.GetComponent<RectTransform>().anchoredPosition;
        steps_pic_positions[1] = step2_picture.GetComponent<RectTransform>().anchoredPosition;
        steps_pic_positions[2] = step3_picture.GetComponent<RectTransform>().anchoredPosition;

        initialEnemyZPos = enemy.transform.position.z;
        SettingsToReset();
    }

    void SettingsToReset()
    {
        game_running = false;
        preGame = true;

        didQuiz = false;
        gotFirstStep = false;
        gotSecondStep = false;
        passedQuiz = false;

        option1_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(steps_pic_positions[rand_index1].x, option1_button.GetComponent<RectTransform>().anchoredPosition.y);
        option2_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(steps_pic_positions[rand_index2].x, option2_button.GetComponent<RectTransform>().anchoredPosition.y);
        option3_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(steps_pic_positions[rand_index3].x, option3_button.GetComponent<RectTransform>().anchoredPosition.y);
        option1_button.GetComponent<Image>().color = Color.white;
        option2_button.GetComponent<Image>().color = Color.white;
        option3_button.GetComponent<Image>().color = Color.white;
  
        timeRunning = 0.0f;

        // Run the SetCountText function to update the UI (see below)
        scoreText.text = "Score: 0";

        onScreenText.gameObject.SetActive(true);
        recovery_instructions.SetActive(true);

        onScreenText.text = "Pick up collectables in the order below" + "\n" +
                            "Escape the chasing enemy (Warning sign on map)" + "\n" +
                            "Press any movement button (W/A/S/D) to begin";        

        // Set the count to zero 
        count_collect_first = 0;
        count_collect_second = 0;
        count_collect_final = 0;
        count_completed = 0;

        streak_completed = false;
        mistake = false;
        indicator_wait = 0.0f;

        enemyZpos = initialEnemyZPos;
        UpdateMinimap();
        // Set the bools to zero and color of indicators to white
        ResetCollections();
    }

    void BeginQuiz()
    {
        game_running = false;
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        onScreenText.gameObject.SetActive(true);
        onScreenText.text = "Select in correct order for:\n" +
                            "5 bonus points";

        recovery_instructions.SetActive(true);
        step1_picture.SetActive(false);
        step2_picture.SetActive(false);
        step3_picture.SetActive(false);

        rand_index1 = Random.Range(0, 2);
        step1_description.GetComponent<RectTransform>().anchoredPosition = steps_desc_positions[rand_index1];
        step1_picture.GetComponent<RectTransform>().anchoredPosition = steps_pic_positions[rand_index1];
        option1_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(steps_pic_positions[rand_index1].x, option1_button.GetComponent<RectTransform>().anchoredPosition.y);

        int tempMinRange, tempMaxRange;
        if (rand_index1 == 0) {
            tempMinRange = 1;
            tempMaxRange = 2;
            rand_index2 = Random.Range(tempMinRange, tempMaxRange);
            if (rand_index2 == 1)
                rand_index3 = 2;
            else
                rand_index3 = 1;
        }
        else if (rand_index1 == 1)
        {   //for 0 and 2  
            tempMinRange = 0;
            tempMaxRange = 1;
            int tempRand = Random.Range(tempMinRange, tempMaxRange);
            if (tempRand == 0)
                rand_index2 = 0;
            else
                rand_index2 = 2;

            if (rand_index2 == 0)
                rand_index3 = 2;
            else
                rand_index3 = 0;
        }
        else
        {
            rand_index2 = Random.Range(0, 1);
            if (rand_index2 == 0)
                rand_index3 = 1;
            else
                rand_index3 = 0;
        }
       
        step2_description.GetComponent<RectTransform>().anchoredPosition = steps_desc_positions[rand_index2];
        step2_picture.GetComponent<RectTransform>().anchoredPosition = steps_pic_positions[rand_index2];
        option2_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(steps_pic_positions[rand_index2].x, option2_button.GetComponent<RectTransform>().anchoredPosition.y);

        step3_description.GetComponent<RectTransform>().anchoredPosition = steps_desc_positions[rand_index3];
        step3_picture.GetComponent<RectTransform>().anchoredPosition = steps_pic_positions[rand_index3];
        option3_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(steps_pic_positions[rand_index3].x, option3_button.GetComponent<RectTransform>().anchoredPosition.y);

        option1_button.SetActive(true);
        option2_button.SetActive(true);
        option3_button.SetActive(true);
    }

    void EndQuizination()
    {
        onScreenText.gameObject.SetActive(false);

        recovery_instructions.SetActive(false);
        step1_description.GetComponent<RectTransform>().anchoredPosition = steps_desc_positions[0];
        step2_description.GetComponent<RectTransform>().anchoredPosition = steps_desc_positions[1];
        step3_description.GetComponent<RectTransform>().anchoredPosition = steps_desc_positions[2];
        step1_picture.GetComponent<RectTransform>().anchoredPosition = steps_pic_positions[0];
        step2_picture.GetComponent<RectTransform>().anchoredPosition = steps_pic_positions[1];
        step3_picture.GetComponent<RectTransform>().anchoredPosition = steps_pic_positions[2];
        
        step1_picture.SetActive(true);
        step2_picture.SetActive(true);
        step3_picture.SetActive(true);

        option1_button.SetActive(false);
        option2_button.SetActive(false);
        option3_button.SetActive(false);

        Unpause();
    }

    void UpdateMinimap()
    {
        float minimap_halfsize = minimapYsize * 0.5f;
        RectTransform myRectTransform = playerOnMiniMap.GetComponent<RectTransform>();
        myRectTransform.anchoredPosition = new Vector2(myRectTransform.anchoredPosition.x, (transform.position.z * ratioMapToMinimap) - minimap_halfsize);
        myRectTransform = enemyOnMiniMap.GetComponent<RectTransform>();
        myRectTransform.anchoredPosition = new Vector2(myRectTransform.anchoredPosition.x, (enemy.transform.position.z * ratioMapToMinimap) - minimap_halfsize);
    }

    void Unpause()
    {
        onScreenText.gameObject.SetActive(false);
        recovery_instructions.SetActive(false);
        game_running = true;
    }

    void FailedQuiz()
    {
        passedQuiz = true;
        onScreenText.gameObject.SetActive(true);
        onScreenText.text = "Failed";
    }

    public void Option1_Click()
    {
        option1_button.GetComponent<Image>().color = Color.green;
        step1_picture.SetActive(true);
        gotFirstStep = true;
    }

    public void Option2_Click()
    {
        if(gotFirstStep)
        {
            option2_button.GetComponent<Image>().color = Color.green;
            step2_picture.SetActive(true);
            gotSecondStep = true;
        }
        else {
            FailedQuiz();
        }
    }

    public void Option3_Click()
    {
        if (gotFirstStep && gotSecondStep) {
            option3_button.GetComponent<Image>().color = Color.green;
            step3_picture.SetActive(true);
            passedQuiz = true;
            onScreenText.gameObject.SetActive(true);
            onScreenText.text = "Passed";
            if (gameController.GetIfOffline() == true)
                count_completed += 20;
            else
                QuizPassed();
        }
        else
        {
            FailedQuiz();
        }
    }

    void Update()
    {
        if (passedQuiz == true)
        {
            indicator_wait += Time.deltaTime;
            if (indicator_wait >= 2.0f)
            {
                passedQuiz = false;
                indicator_wait = 0.0f;
                onScreenText.gameObject.SetActive(false);
                EndQuizination();
            }
        }

        if (game_running == true)
        {
            timeRunning += Time.deltaTime;
            enemyZpos += ENEMY_SPEED * Time.deltaTime;
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemyZpos);

            if (streak_completed || mistake)
            {
                indicator_wait += Time.deltaTime;
                if (indicator_wait >= 0.3f)
                {
                    streak_completed = false;
                    mistake = false;
                    ResetCollections();
                    indicator_wait = 0.0f;
                }
                if (mistake)
                {
                    indicator_Step1.GetComponent<Image>().color = Color.red;
                    indicator_Step2.GetComponent<Image>().color = Color.red;
                    indicator_Step3.GetComponent<Image>().color = Color.red;
                }
            }

            if (didQuiz == false)
            {
                if (transform.position.z >= quiz_ZPos)
                {
                    BeginQuiz();
                    didQuiz = true;
                }
            }

            if (transform.position.z >= mapZsize - 10)
            {
                EndGame(true);
            }

            UpdateMinimap();
        }
        else if (preGame == true)
        {
            if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
            {
                preGame = false;
                Unpause();
            }
        }

        timeText.text = "Time: " + timeRunning.ToString();
        if (gameController.GetIfOffline() == true)
            scoreText.text = "Score: " + count_completed.ToString();  
    }

	// Each physics step..
	void FixedUpdate ()
	{
        if (game_running)
        {
            // Set some local float variables equal to the value of our Horizontal and Vertical Inputs

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            // Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
            // multiplying it by 'speed' - our public player speed that appears in the inspector
            rb.AddForce(movement * speed);
        }
	}

    // After the standard 'Update()' loop runs, and just before each frame is rendered..
    void LateUpdate()
    {
        // Set the position of the Camera (the game object this script is attached to)
        // to the player's position, plus the offset amount
        fpsCamera.transform.position = transform.position;// + offset;
    }


    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Recovery_PickUpStep1"))
		{
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);
            count_collect_first = count_collect_first + 1;

            if (collect_first == false)
            {
                collect_first = true;
                indicator_Step1.GetComponent<Image>().color = Color.red;
            }
            else
            {
                mistake = true;
            }
		}

        if (other.gameObject.CompareTag("Recovery_PickUpStep2"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);
            count_collect_second = count_collect_second + 1;

            if (collect_first == true && collect_second == false)
            {
                collect_second = true;
                indicator_Step2.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                mistake = true;
            }

        }

        if (other.gameObject.CompareTag("Recovery_PickUpStep3"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);
            count_collect_final = count_collect_final + 1;

            if (collect_first == true && collect_second == true && collect_final == false)
            {
                collect_final = true;
                indicator_Step3.GetComponent<Image>().color = Color.green;
                if (gameController.GetIfOffline() == true)
                    count_completed += 10;
                else
                    IncreaseScore();
                streak_completed = true;
            }
            else {
                mistake = true;
            }
        }

        if (other.gameObject.CompareTag("Enemy_Hit"))
        {
            EndGame(false);
        }
    }

    void IncreaseScore()
    {
        count_completed++;

        JSONNode vals = JSON.Parse("{\"sequence\" : \"" + "add_score" + "\" }");
        // ask EngAGe to assess the action based on the config file
        StartCoroutine(EngAGe.E.assess("recovery_sequenceComplete", vals, gameController.ActionAssessed));
    }

    void QuizPassed()
    {
        JSONNode vals = JSON.Parse("{\"passed\" : \"" + "add_score" + "\" }");
        // ask EngAGe to assess the action based on the config file
        StartCoroutine(EngAGe.E.assess("recovery_quizPassed", vals, gameController.ActionAssessed));
    }

    void ResetCollections()
    {
        collect_first = false;
        collect_second = false;
        collect_final = false;

        indicator_Step1.GetComponent<Image>().color = Color.white;
        indicator_Step2.GetComponent<Image>().color = Color.white;
        indicator_Step3.GetComponent<Image>().color = Color.white;
    }

    void EndGame(bool win)
    {
        game_running = false;
        recovery_instructions.SetActive(true);
        onScreenText.gameObject.SetActive(true);

        // just said 30. there are better ways of doin it fo sho
        int extraPickups = (count_collect_first + count_collect_second + count_collect_final) - count_completed * 3;
        if (win == true)
        {
            onScreenText.text = "You won with " + count_completed + " completed streaks" + "\n" +
                                "in " + timeRunning + " seconds!";// with " + extraPickups + " mistakes.";
            if (gameController.GetIfOffline() == false)
                StartCoroutine(EngAGe.E.endGameplay(true));
        }
        else
        {
            onScreenText.text = "Game over. You got " + count_completed + " completed streaks" + "\n" +
                                "in " + timeRunning + " seconds!";// with " + extraPickups + " mistakes.";
            if (gameController.GetIfOffline() == false)
                StartCoroutine(EngAGe.E.endGameplay(false));
        }

        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void ResetGame()
    {
        SettingsToReset();
        transform.position = new Vector3(0.0f, 2.0f, 0.0f);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
}