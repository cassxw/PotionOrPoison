using System.Collections;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    //store customer prefabs
    public GameObject[] customerPrefabsLevelOne = new GameObject[4];
    public GameObject[] customerPrefabsLevelTwo = new GameObject[6];
    public GameObject[] customerPrefabsLevelThree = new GameObject[8];
    public GameObject[] currentPrefabs;
    public GameObject[] impostorPrefabs = new GameObject[7];
    public Customer[] customers;
    public int maxCustomers; //max number of customers per level
    private List<int> usedCustomerIndices = new List<int>(); //track customers already used
    public Transform canvasTransform; 
    public float timeToSpawn; // max time
    public Timer timer; //track timer
    private float currentTimeToSpawn; // track cooldown
    private GameObject currentCustomer; // Track the current customer
    private Customer currentCustomerData;
    public int count = 0; // Count of customers that have appeared

    //booleans to track customer states
    public bool isRandomized;
    public bool customerServed = false;
    private bool isSpawningCustomer = false;
    private bool levelComplete = false;
    private bool firstSpawn = true;
    public bool isImposter=false;

    //store potion requests and reasons for customers
    public DialogueBox dialogueBox;
    public PotionRequest[] potionRequests;
    public PotionRequest selectedRequest;
    public string selectedReason;

    public SlidingDoor windowShutter; //sliding door object for between spawning customers
    public int currentLevel; //track the current level
    public SceneController sceneController; //scene controller to switch to level intermission
    public LivesManager livesManager; //manage lives lost when player makes the wrong potion
    public CustomerSelector customerSelector; //select customers per level
    public PotionRequestSelector potionRequestSelector;//select potion requests per level
    public BackgroundTransition backgroundImage;
    private bool isHandlingTimeout = false;

    IEnumerator Start()
    {
        Timer.OnTimeDocked +=ReduceSpawnTime;

        sceneController = FindAnyObjectByType<SceneController>();
        livesManager = FindAnyObjectByType<LivesManager>();
        customerSelector = FindAnyObjectByType<CustomerSelector>();

        currentLevel = GameSystem.Instance.GetLevel();
        GetMaxCustomers();

        yield return new WaitForSeconds(0.1f);
        customers = customerSelector.GetCustomersByLevel(currentLevel);
        GetPrefabsByLevel();
        timeToSpawn = timer.GetGameTime();
    }

    void Update()
    {
        if(!isSpawningCustomer && count<=maxCustomers){
            currentTimeToSpawn-=Time.deltaTime;

            if(currentTimeToSpawn <=0){
                if(currentCustomer != null){
                    StartCoroutine(HandleCustomerTimeout());
                }
                else{
                    StartCoroutine(SpawnAndHandleCustomer());
                }
            }
        }
        currentLevel = GameSystem.Instance.GetLevel();

        if(levelComplete && sceneController!=null)
        {
            sceneController.LoadScene("LevelIntermission");
        }
    }

    private IEnumerator HandleCustomerTimeout(){
        
        if(isHandlingTimeout) yield break;
        isHandlingTimeout = true;
        if(currentCustomer == null) yield break;
        livesManager.DecreaseLives();
        GameSystem.Instance.incorrectCrafts += 1;

        if (isImposter)
        {
            GameSystem.Instance.threatLevel += 1;
            GameSystem.Instance.rebelsMissed += 1;
        }

        dialogueBox.UpdateDialogue("timeout");

        Debug.Log("in customer timeout");
        if(count == maxCustomers){
            Debug.Log("handling max customer timeout");
            yield return StartCoroutine(CloseShutterAfterLastCustomer());
            yield return new WaitForSeconds(1f);
            levelComplete=true;
        }
        else{
            windowShutter.MoveDown();
            StartCoroutine(HandleCustomerDestruction());
        }
        currentTimeToSpawn=timeToSpawn;
        isHandlingTimeout = false;
    }

    private void GetPrefabsByLevel(){
        switch (currentLevel){
            case 1:
                currentPrefabs = customerPrefabsLevelOne;
                break;
            case 2:
                currentPrefabs = customerPrefabsLevelTwo;
                break;
            case 3:
                currentPrefabs = customerPrefabsLevelThree;
                break;
            default:
                currentPrefabs = null;
                return;
        }
    }

    private IEnumerator HandleCustomerDestruction()
    {
        yield return new WaitForSeconds(1f);
        Destroy(currentCustomer);
        currentCustomer = null; // Set currentCustomer to null after destroying
        yield return new WaitForSeconds(1f);
        dialogueBox.ClearDialogue();
        isImposter = false;
        yield return StartCoroutine(SpawnAndHandleCustomer());
    }

    public IEnumerator CustomerServed(){
        if(count == maxCustomers){
            yield return StartCoroutine(CloseShutterAfterLastCustomer());
            yield return new WaitForSeconds(1f);
            levelComplete=true;
        }
        else if(count<maxCustomers){
            customerServed = true;
            StartCoroutine(SpawnAndHandleCustomer());
        }
        else{
            Debug.Log("Max customer count reached.");
        }
    }

    private IEnumerator SpawnAndHandleCustomer(){
        isSpawningCustomer = true;

        if(currentCustomer != null){
            windowShutter.MoveDown();
            timer.Pause();
            yield return new WaitForSeconds(1f);

            Destroy(currentCustomer);
            currentCustomer = null;
            yield return new WaitForSeconds(1f);
            dialogueBox.ClearDialogue();
            isImposter = false;
        }

        if(firstSpawn){
            yield return new WaitForSeconds(1f);
            firstSpawn=false;
        }

        if(count < maxCustomers){
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(SpawnCustomer());
            windowShutter.MoveUp();
            yield return new WaitForSeconds(1f);
            timer.Resume();
            customerServed=false;
            currentTimeToSpawn = timeToSpawn; //reset spawn timer
            isSpawningCustomer=false;
        }
    }

    private IEnumerator SpawnCustomer()
    {
        int randomIndex;
        int randomImpostorIndex = 0;
        List<int> usedImpostorIndices = GameSystem.Instance.GetImpostorIndices();
        int maxAttempts = 100; // Set a limit for the number of attempts
        int attempts = 0;

        // Find an unused random genuine customer index
        do {
            randomIndex = Random.Range(0, customers.Length);
            attempts++;
            if (attempts >= maxAttempts) {
                Debug.LogError("Max attempts reached while trying to find an unused customer index.");
                yield break;
            }
        } while (usedCustomerIndices.Contains(randomIndex) && usedCustomerIndices.Count < customers.Length);

        Debug.Log("Impostor index: "+ randomIndex);
        usedCustomerIndices.Add(randomIndex);
        currentCustomerData = customers[randomIndex];

        if(currentCustomerData.isImposter){
            // Find an unused random impostor index
            do {
                randomImpostorIndex = Random.Range(0, impostorPrefabs.Length);
                attempts++;
                if (attempts >= maxAttempts) {
                    Debug.LogError("Max attempts reached while trying to find an unused customer index.");
                    yield break;
                }
            } while (usedImpostorIndices.Contains(randomImpostorIndex) && usedImpostorIndices.Count < impostorPrefabs.Length);

            Debug.Log("Impostor index: "+ randomImpostorIndex);
            GameSystem.Instance.UpdateImpostorIndices(randomImpostorIndex);
        }

        // Reset imposter bool
        isImposter = false;

        // Instantiate a new customer and set its parent to the Canvas
        if(currentCustomerData.isImposter){
            //Instantiate customer with impostor prefab
            Debug.Log("Instantiating impostor at index "+randomImpostorIndex);
            currentCustomer = Instantiate(impostorPrefabs[randomImpostorIndex], canvasTransform);
            isImposter = true;
        }
        else{
            //Instantiate customer with genuine customer prefab
            Debug.Log("Instantiating customer at index "+ randomIndex);
            currentCustomer = Instantiate(currentPrefabs[randomIndex], canvasTransform); // Ensure prefab exists
        }

        // Convert the CustomerSpawner's position to canvas coordinates
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform canvasRect = canvasTransform.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out Vector2 localPos);

        // Set the new customer's position
        currentCustomer.GetComponent<RectTransform>().localPosition = localPos;
        //ensure background is behind customer
        backgroundImage.transform.SetSiblingIndex(1);
        //ensure customer is behind window shutter
        currentCustomer.transform.SetSiblingIndex(windowShutter.transform.GetSiblingIndex() - 1);

        // Get the potion request
        selectedRequest = potionRequestSelector.GetPotionRequestForCurrentLevel(currentCustomerData, currentLevel);
        
        // Check if the customer is an imposter
        if (currentCustomerData.isImposter) {
            selectedReason = selectedRequest.reasons[1];
            isImposter = true;
            Debug.Log("IMPOSTOR: "+selectedRequest.request);
        } else {
            selectedReason = selectedRequest.reasons[0];
            isImposter = false;
            Debug.Log("NOT IMPOSTOR: "+selectedRequest.request);
        }

        // Reset the timer when a new customer is spawned
        timer.ResetTime();

        //populate dialogue box with customer request and reason
        dialogueBox.UpdateDialogue(selectedRequest.request, selectedReason, isImposter);

        // Increment the customer count
        count++;

        yield return null;
    }

    private IEnumerator CloseShutterAfterLastCustomer(){
        if(currentCustomer != null){
            yield return new WaitForSeconds(1f);
            dialogueBox.ClearDialogue();
            windowShutter.MoveDown();
            yield return new WaitForSeconds(1f);

            Destroy(currentCustomer);
            currentCustomer = null;
        }
    }

    private void ReduceSpawnTime(float timeToDock)
    {
        // Reduce the time left until the next spawn based on the docked time
        currentTimeToSpawn -= timeToDock;

        // Ensure spawn time doesn't go negative
        if (currentTimeToSpawn < 0)
        {
            currentTimeToSpawn = 0;
        }
    }

    private void GetMaxCustomers(){
        if(currentLevel == 1){
            maxCustomers = 4; //no. of imposters = 1? (25% of total customers is imposter)
        }
        else if(currentLevel == 2){
            maxCustomers = 6; //no. of imposters = 2? (33% of total customers is imposter)
        }
        else if (currentLevel ==3){
            maxCustomers = 8; //no. of imposters = 4? (50% of total customers is imposter)
        }
        else{
            return;
        }
    }
}