using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class TowerController : MonoBehaviour
{
    public DataManager.TowerNames towerDescriptor;
    public bool isPlaced = false;
    public DataManager.Effects[] currentEffects;
    public string towerName;
    [Multiline]
    public string towerDescription;
    public int cost = 10; //Running tally of the cost of the tower
    public DataManager.Effects currentEffect;

    //level will be increased when it is upgraded
    public int level = 1;
    public float touchRadius = 1.5f;
    public float rangeRadius = 5f;
    [Range(0,50)]
    public int segments = 50; //how many pieces in the rendered range radius
    public float xRangeRadius = 1;
    public float yRangeRadius = 1;
    LineRenderer line;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject target; //The current enemy to shoot at
    
    public bool isOverlapping = false;
    public GameObject placeButton, cancelButton;

    //List of enemies this tower is more effective against
    public Dictionary<DataManager.EnemyNames, float> damageIndex;
    public float base_damage = 1f; //Damage for all other enemies
    public Upgrades nextUpgrade; // The next upgrade available
    private PlaceButton placeButtonScript;
    private CancelButton cancelButtonScript;
    //Holds the offset from the touch point to the center of a tower.
    private float deltaX, deltaY;
    private bool canMove = false;
    public string enemyTag = "Enemy";

    public GameObject bulletPrefab;
    public Transform firePoint; //The point where the bullet generates
    //Audio Variables
    public AudioClip upgradeSound; // sound effect for upgrade
    public AudioClip shootSound;
    AudioSource audioSource;

    //A list of all the possible upgrades
    //This list should be in the same order that the upgrades are purchased
    public enum Upgrades{
        increaseRange,
        increaseAttackSpeed,
        increaseDamage,
        fullyUpgraded
    }

    //Define the costs for various upgrades
    public Dictionary<Upgrades, int> upgradeCosts = 
        new Dictionary<Upgrades, int>(){
            {Upgrades.increaseRange, 10},
            {Upgrades.increaseAttackSpeed, 15},
            {Upgrades.increaseDamage, 20},
            {Upgrades.fullyUpgraded, int.MaxValue}
        };

    //Make a description string for each upgrade
    public Dictionary<Upgrades, string> upgradeDescription;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.localScale);
        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.useWorldSpace = false;
        DrawCircle();
        //Get the audio source
        audioSource = GetComponent<AudioSource>();
        //Set the tower name to the name of the enumerated descriptor
        towerName = Enum.GetName(typeof(DataManager.TowerNames), towerDescriptor);
        PopulateDamages(); //Get the damage values for this tower

        //Populate the descriptions for each upgrade
        upgradeDescription = 
            new Dictionary<Upgrades, string>(){
                {Upgrades.increaseAttackSpeed, GenerateUgradeDescription(Upgrades.increaseAttackSpeed)},
                {Upgrades.increaseDamage, GenerateUgradeDescription(Upgrades.increaseDamage)},
                {Upgrades.increaseRange, GenerateUgradeDescription(Upgrades.increaseRange)},
                {Upgrades.fullyUpgraded, GenerateUgradeDescription(Upgrades.fullyUpgraded)}
            };
        nextUpgrade = 0; //Start at the first upgrade in the list

        //Find the buttons used to place or cancel a tower placement
        foreach (GameObject button in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (button.tag == "PlaceButton"){
                placeButton = button;
                placeButtonScript = placeButton.GetComponent<PlaceButton>();
            }
            else if(button.tag == "CancelButton"){
                cancelButton = button;
                cancelButtonScript = cancelButton.GetComponent<CancelButton>();
            }
        }
    }

    //function to find the enemy and set it as target
    void updateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        //find the nearest enemy to attack
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance (transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        //find enemy in range
        if (nearestEnemy != null && shortestDistance <= rangeRadius){
            target = nearestEnemy;
        }
        //no enemy in range
        else{
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //shoot the target based on the firerate of tower if there is enemy
        if(isPlaced)
        {
            line.enabled = false; //Don't draw the radius if the tower is placed
            updateTarget();

            if (fireCountdown <= 0f && target != null)
            {

                Shoot();
                fireCountdown = 1f/fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }


        if(!isPlaced){
            line.enabled = true;
            DrawCircle();
            
            //Set the opacity to make it slightly transparent
            if(!isOverlapping){
                Color temp = GetComponent<SpriteRenderer>().color;
                temp.a = 0.75f;
                GetComponent<SpriteRenderer>().color = temp;
            }
            //Set the opacity more transparent if it can't be placed in the current location
            else{
                Color temp = GetComponent<SpriteRenderer>().color;
                temp.a = 0.5f;
                GetComponent<SpriteRenderer>().color = temp;
            }
            //Set the current tower on the place button to this one
            placeButtonScript.currentTower = this;
            placeButton.SetActive(true);
            cancelButtonScript.currentTower = this.gameObject;
            cancelButton.SetActive(true);
            GameManager.towerBeingPlaced = true;
        }
        else{
            //Set the opacity back to normal
            Color temp = GetComponent<SpriteRenderer>().color;
            temp.a = 1f;
            GetComponent<SpriteRenderer>().color = temp;
        }

        //Logic for touch controls when the tower is being placed
        if(!isPlaced && Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            //Get the touch position in the world
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch(touch.phase){
                //When the player starts touching the tower
                case TouchPhase.Began:
                    //Make sure the touch is within the touch radius
                    float touchDistance = Vector2.Distance(touchPos, transform.position);
                    if(touchDistance <= touchRadius){
                        //Set an offset from where the player touched
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;

                        canMove = true;
                    }
                    break;

                //When the player moves the tower
                case TouchPhase.Moved:
                    if(canMove){
                        transform.position = new Vector2(touchPos.x - deltaX, touchPos.y - deltaY);
                    }
                break;

                //When the player releases their finger
                case TouchPhase.Ended:
                    canMove = false;
                break;
            }
        }

        //Touch logic for when a tower is already placed
        else if(isPlaced && Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            //Get the touch position in the world
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch(touch.phase){
                //When the player starts touching the tower
                case TouchPhase.Began:
                    //Make sure the touch is within the touch radius
                    if(GetComponent<Collider2D> () == Physics2D.OverlapPoint (touchPos)){
                        GameManager.towerIsSelected = true;
                        GameManager.selectedTower = this;
                    }
                    break;
            }
        }

        if(GameManager.towerIsSelected && GameManager.selectedTower == this){
            line.enabled = true; //Draw the radius when the tower is selected
            DrawCircle();
        }
    }

    //Draws the range radius around the tower
    void DrawCircle()
    {
        float x;
        float y;

        //Convert the radius to account for scaling of local object
        xRangeRadius = rangeRadius / transform.localScale.x;
        yRangeRadius = rangeRadius / transform.localScale.y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xRangeRadius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * yRangeRadius;

            line.SetPosition (i,new Vector3(x,y,0) );

            angle += (360f / segments);
        }
    }

    
    //Shoot function is called when a tower find a target enemy. 
    void Shoot()
    {
        //Create a new bullet
        GameObject bulletObject = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        EnemyController targetController = target.GetComponent<EnemyController>();

        if(bullet != null && bullet != null)
        {
            
            bullet.setBullet(target); //Set the target on the bullet

            //Set the damage based on tower type
            if(damageIndex.ContainsKey(targetController.enemyDescriptor)){
                bullet.bulletDamage = damageIndex[targetController.enemyDescriptor];
            }
            else{
                bullet.bulletDamage = base_damage;
            }
            audioSource.PlayOneShot(shootSound, 1.0F); // play sound
        }
    }

    //Draw a visual of the range and touch radius in the editor
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, touchRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }

    public void Upgrade()
    {
        //Get the cost of the next available upgrade
        int upCost = upgradeCosts[nextUpgrade];
        if(upCost <= GameManager.currency){ //Make sure there's enough money
            level++;
            GameManager.currency -= upCost;
            cost += upCost; //Increase the total cost for the tower
            switch(nextUpgrade){
                case Upgrades.increaseAttackSpeed:
                    fireRate *= 2.0f;
                    break;
                case Upgrades.increaseDamage:
                    //Double the base damage
                    base_damage = (float)(base_damage * 2.0f);
                    //Double all other damage values
                    Dictionary<DataManager.EnemyNames, float> newDamages = new Dictionary<DataManager.EnemyNames, float>();
                    foreach(KeyValuePair<DataManager.EnemyNames, float> e in damageIndex){
                        newDamages.Add(e.Key, e.Value * 2.0f);
                    }
                    damageIndex = newDamages;
                    break;
                case Upgrades.increaseRange:
                    rangeRadius = (float)(rangeRadius * 1.3);
                    break;
            }
            nextUpgrade++;
            audioSource.PlayOneShot(upgradeSound, 1.0F); // play sound
        }
        else{
            Debug.Log("Not enough currency for upgrade");
        }

    }

    //Sets given text fields to the current name and description of the next available upgrade
    public void SetDescription(TextMeshProUGUI name, TextMeshProUGUI desc){
        name.SetText(towerName);
        int saleValue = (int)(0.5f * cost);
        desc.SetText(upgradeDescription[nextUpgrade] + "\nSell for: " + saleValue.ToString());
    }
        
    //Returns a string with a description for a given upgrade
    private string GenerateUgradeDescription(Upgrades currUpgrade){
        string description = "Next Upgrade: ";
        switch(currUpgrade){
                case Upgrades.increaseAttackSpeed:
                    description += "Increase Attack Speed\n";
                    break;
                case Upgrades.increaseDamage:
                    description += "Increase Damage\n";
                    break;
                case Upgrades.increaseRange:
                    description += "Increase Range\n";
                    break;
                case Upgrades.fullyUpgraded:
                    description = "Fully Upgraded\n";
                    return description;
            }
            description += ("Upgrade Cost: " + upgradeCosts[currUpgrade]);
            return description;
    }

    //Searched through the dictionary in datamanager and finds the corresponding list of
    //of enemies this tower is more effective against
    public void PopulateDamages(){
        DataManager.EnemyNames[] enemies = DataManager.TowerEffectiveness[towerDescriptor];
        damageIndex =  new Dictionary<DataManager.EnemyNames, float>();
        //For all enemies this tower is effective against, set the damage to 5
        foreach(DataManager.EnemyNames name in enemies){
            damageIndex.Add(name, 5.0f);
        }
    }

    void OnTriggerStay2D(Collider2D col){
        isOverlapping = true;
    }
    void OnTriggerExit2D(Collider2D col){
        isOverlapping = false;
    }
}
