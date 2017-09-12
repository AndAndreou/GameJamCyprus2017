using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour {

    public int playerNumber = 1;              // Used to identify which robot belongs to which player.  This is set by this robot's manager.
    public float speed = 12f;                 // How fast the robot moves forward and back.
    public GameObject bodyGO;

    private string movementHorAxisName;          // The name of the input axis for moving forward and back.
    private string movementVerAxisName;              // The name of the input axis for turning.
    private string bodyTurnAxisHorName;
    private string bodyTurnAxisVerName;
    private string fireBtnName;
    private string powerUpFirstBtnName;
    private string powerUpSecondBtnName;
    private Rigidbody rigidbody;              // Reference used to move the robot.
    private float movementHorInputValue;         // The current value of the movement input.
    private float movementVerInputValue;             // The current value of the turn input.
    private float bodyTurnHorInputValue;         // The current value of the  robot body turn input.
    private float bodyTurnVerInputValue;
    private float originalPitch;              // The pitch of the audio source at the start of the scene.

    public bool canReceiveDmg = true;
    public bool canUsePowerUps = true;
    public float maxHealth = 1000f;
    public float currentHealth;
    public bool empBool;
    public bool shieldBool;
    public bool pMineBool;
    public bool hackBool;

    public bool isAlive = true;

    public PowerUpTypeValue powerUpFirstId;
    public PowerUpTypeValue powerUpSecondId;

    Vector3 relativeForward;
    Vector3 relativeRight;
    float currentSpeed;

    //for firing
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;

    private PlayerCustomizationData playerData = null;
    private HUDRepo hudRepo = null;

    public GameObject[] gunsRightHand;
    public GameObject[] gunsLeftHand;

    public GameObject[] gunsPrefab;

    [Header("UI Componenets")]
    public UIComponents uiComponent;

    [System.Serializable]
    public struct UIComponents
    {
        public GameObject panel;
        public Image health;
        public Image powerUp1;
        public Image powerUp2;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // When the robot is turned on, make sure it's not kinematic.
        rigidbody.isKinematic = false;

        // Also reset the input values.
        movementHorInputValue = 0f;
        movementVerInputValue = 0f;

        movementVerInputValue = 0f;
    }


    private void OnDisable()
    {
        // When the robot is turned off, set it to kinematic so it stops moving.
        rigidbody.isKinematic = true;
    }


    private void Start()
    {
        playerData = GameObject.FindObjectOfType<PlayerCustomizationData>();
        hudRepo = GameObject.FindObjectOfType<HUDRepo>();
        InitPlayer();

        empBool = gameObject.GetComponent<EMP>();
        shieldBool = gameObject.GetComponent<Shield>();
        pMineBool = gameObject.GetComponent<ProximityMine>();
        hackBool = gameObject.GetComponent<Hack>();

        currentHealth = maxHealth;

        // The axes names are based on player number.
        movementHorAxisName = "Vertical" + playerNumber;
        movementVerAxisName = "Horizontal" + playerNumber;

        bodyTurnAxisHorName = "BodyTurnAxisHor" + playerNumber;
        bodyTurnAxisVerName = "BodyTurnAxisVer" + playerNumber;

        fireBtnName = "Fire" + playerNumber; 

        powerUpFirstBtnName = "PowerUpFirst" + playerNumber;
        powerUpSecondBtnName = "PowerUpSecond" + playerNumber;
    }

    private void InitPlayer()
    {
        powerUpFirstId = playerData.players[playerNumber - 1].powerUp1Type.value;
        powerUpSecondId = playerData.players[playerNumber - 1].powerUp2Type.value;

        //delete not used powerUps
        if (powerUpFirstId != PowerUpTypeValue.EMP && powerUpSecondId != PowerUpTypeValue.EMP) {
            Destroy(gameObject.GetComponent<EMP>());
        }
        if (powerUpFirstId != PowerUpTypeValue.Shield && powerUpSecondId != PowerUpTypeValue.Shield)
        {
            Destroy(gameObject.GetComponent<Shield>());
        }
        if (powerUpFirstId != PowerUpTypeValue.ProximityMine && powerUpSecondId != PowerUpTypeValue.ProximityMine)
        {
            Destroy(gameObject.GetComponent<ProximityMine>());
        }
        if (powerUpFirstId != PowerUpTypeValue.Hack && powerUpSecondId != PowerUpTypeValue.Hack)
        {
            Destroy(gameObject.GetComponent<Hack>());
        }

        gameObject.name = playerData.players[playerNumber - 1].playerEntity.nickname;


        switch ( playerData.players[playerNumber - 1].weaponType.value)
        {
            case WeaponTypeValue.LaserGun1:
                gunsRightHand[0].SetActive(true);
                gunsLeftHand[0].SetActive(true);
                shot = gunsPrefab[0];
                fireRate = 0.25f;
                break;
            case WeaponTypeValue.PulseGun2:
                gunsRightHand[1].SetActive(true);
                gunsLeftHand[1].SetActive(true);
                shot = gunsPrefab[1];
                fireRate = 0.5f;
                break;
            case WeaponTypeValue.BeamGun3:
                gunsRightHand[2].SetActive(true);
                gunsLeftHand[2].SetActive(true);
                shot = gunsPrefab[2];
                fireRate = 1.25f;
                break;
            case WeaponTypeValue.PlasmaGun4:
                gunsRightHand[3].SetActive(true);
                gunsLeftHand[3].SetActive(true);
                shot = gunsPrefab[3];
                fireRate = 1f;
                break;
        }

        initHud();
    }

    private void initHud()
    {
        uiComponent.health.sprite = hudRepo.health_6of6;

        switch (powerUpFirstId)
        {
            case PowerUpTypeValue.EMP:
                uiComponent.powerUp1.sprite = hudRepo.emr;
                break;
            case PowerUpTypeValue.Hack:
                uiComponent.powerUp1.sprite = hudRepo.hax;
                break;
            case PowerUpTypeValue.ProximityMine:
                uiComponent.powerUp1.sprite = hudRepo.mne;
                break;
            case PowerUpTypeValue.Shield:
                uiComponent.powerUp1.sprite = hudRepo.grd;
                break;
        }

        switch (powerUpSecondId)
        {
            case PowerUpTypeValue.EMP:
                uiComponent.powerUp2.sprite = hudRepo.emr;
                break;
            case PowerUpTypeValue.Hack:
                uiComponent.powerUp2.sprite = hudRepo.hax;
                break;
            case PowerUpTypeValue.ProximityMine:
                uiComponent.powerUp2.sprite = hudRepo.mne;
                break;
            case PowerUpTypeValue.Shield:
                uiComponent.powerUp2.sprite = hudRepo.grd;
                break;
        }
    }

    private void Update()
    {
        // Store the value of both input axes.
        movementHorInputValue = Input.GetAxis(movementHorAxisName);
        movementVerInputValue = Input.GetAxis(movementVerAxisName);

        bodyTurnHorInputValue = Input.GetAxis(bodyTurnAxisHorName);
        bodyTurnVerInputValue = Input.GetAxis(bodyTurnAxisVerName);

        if (Input.GetButton(fireBtnName) && (Time.time > nextFire))
        {
            FireInTheHole();
        }

        if (Input.GetButtonDown(powerUpFirstBtnName))
        {
            switch (powerUpFirstId)
            {
                case PowerUpTypeValue.EMP:
                    gameObject.GetComponent<EMP>().UsePowerUp();
                    break;
                case PowerUpTypeValue.Shield:
                    gameObject.GetComponent<Shield>().UsePowerUp();
                    break;
                case PowerUpTypeValue.ProximityMine:
                    gameObject.GetComponent<ProximityMine>().UsePowerUp();
                    break;
                case PowerUpTypeValue.Hack:
                    gameObject.GetComponent<Hack>().UsePowerUp();
                    break;

            }
        }

        if (Input.GetButtonDown(powerUpSecondBtnName))
        {
            switch (powerUpSecondId)
            {
                case PowerUpTypeValue.EMP:
                    gameObject.GetComponent<EMP>().UsePowerUp();
                    break;
                case PowerUpTypeValue.Shield:
                    gameObject.GetComponent<Shield>().UsePowerUp();
                    break;
                case PowerUpTypeValue.ProximityMine:
                    gameObject.GetComponent<ProximityMine>().UsePowerUp();
                    break;
                case PowerUpTypeValue.Hack:
                    gameObject.GetComponent<Hack>().UsePowerUp();
                    break;

            }
        }

        CheckIfHavePowerUp();
    }

    private void CheckIfHavePowerUp()
    {
        switch (powerUpFirstId)
        {
            case PowerUpTypeValue.EMP:
                if ((gameObject.GetComponent<EMP>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.black))
                {
                    uiComponent.powerUp1.color = Color.white;
                }
                else if((!gameObject.GetComponent<EMP>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.white))
                {
                    uiComponent.powerUp1.color = Color.black;
                }
                break;
            case PowerUpTypeValue.Shield:
                if ((gameObject.GetComponent<Shield>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.black))
                {
                    uiComponent.powerUp1.color = Color.white;
                }
                else if ((!gameObject.GetComponent<Shield>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.white))
                {
                    uiComponent.powerUp1.color = Color.black;
                }
                break;
            case PowerUpTypeValue.ProximityMine:
                if ((gameObject.GetComponent<ProximityMine>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.black))
                {
                    uiComponent.powerUp1.color = Color.white;
                }
                else if ((!gameObject.GetComponent<ProximityMine>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.white))
                {
                    uiComponent.powerUp1.color = Color.black;
                }
                break;
            case PowerUpTypeValue.Hack:
                if ((gameObject.GetComponent<Hack>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.black))
                {
                    uiComponent.powerUp1.color = Color.white;
                }
                else if ((!gameObject.GetComponent<Hack>().powerUpEnabed) && (uiComponent.powerUp1.color == Color.white))
                {
                    uiComponent.powerUp1.color = Color.black;
                }
                break;
        }

        switch (powerUpSecondId)
        {
            case PowerUpTypeValue.EMP:
                if ((gameObject.GetComponent<EMP>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.black))
                {
                    uiComponent.powerUp2.color = Color.white;
                }
                else if ((!gameObject.GetComponent<EMP>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.white))
                {
                    uiComponent.powerUp2.color = Color.black;
                }
                break;
            case PowerUpTypeValue.Shield:
                if ((gameObject.GetComponent<Shield>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.black))
                {
                    uiComponent.powerUp2.color = Color.white;
                }
                else if ((!gameObject.GetComponent<Shield>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.white))
                {
                    uiComponent.powerUp2.color = Color.black;
                }
                break;
            case PowerUpTypeValue.ProximityMine:
                if ((gameObject.GetComponent<ProximityMine>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.black))
                {
                    uiComponent.powerUp2.color = Color.white;
                }
                else if ((!gameObject.GetComponent<ProximityMine>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.white))
                {
                    uiComponent.powerUp2.color = Color.black;
                }
                break;
            case PowerUpTypeValue.Hack:
                if ((gameObject.GetComponent<Hack>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.black))
                {
                    uiComponent.powerUp2.color = Color.white;
                }
                else if ((!gameObject.GetComponent<Hack>().powerUpEnabed) && (uiComponent.powerUp2.color == Color.white))
                {
                    uiComponent.powerUp2.color = Color.black;
                }
                break;
        }

    }

    private void FixedUpdate()
    {
        BodyTurn();

        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();

        
    }

    private void Move()
    {

        relativeForward = Camera.main.transform.forward;
        relativeForward.y = 0;
        relativeForward = Vector3.Normalize(relativeForward);
        relativeRight = Quaternion.Euler(new Vector3(0, 90, 0)) * relativeForward;

        // Create a vector in the direction the robot is facing with a magnitude based on the input, speed and the time between frames.
        //Vector3 movement = transform.forward * movementHorInputValue * speed * Time.deltaTime;
        Vector3 movement = ((relativeForward * movementHorInputValue) + (relativeRight * movementVerInputValue)) * speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        rigidbody.MovePosition(rigidbody.position + movement);

        //Vector3 movement = new Vector3(movementVerInputValue, 0.0f, movementHorInputValue );
        //movement = Camera.main.transform.TransformDirection(movement) * speed * Time.deltaTime; ;
        //movement = new Vector3(movement.x, 0f, movement.z);
        //rigidbody.MovePosition(rigidbody.position + movement);

        if (movement != Vector3.zero)
        {
            rigidbody.MoveRotation(Quaternion.LookRotation(movement, Vector3.up));
        }
    }


    private void Turn()
    {
        
        //// Calculate direction based on input
        //Vector3 direction = new Vector3(movementVerInputValue, 0,movementHorInputValue);

        //if (direction.magnitude > 0.1f)
        //{
        //    // Calculate heading vector (camera dependent)
        //    Vector3 rightMovement = relativeRight * Time.deltaTime * movementHorInputValue;
        //    Vector3 upMovement = relativeForward * Time.deltaTime * movementVerInputValue;
        //    Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        //    // Rotate towards heading
        //    Quaternion turnRotation = Quaternion.LookRotation(heading, Vector3.up);
        //    transform.rotation = turnRotation;
        //}

        ///////

        //// Determine the number of degrees to be turned based on the input, speed and time between frames.
        //float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        //// Make this into a rotation in the y axis.
        //Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        //// Apply this rotation to the rigidbody's rotation.
        //m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);

        //////

        //float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        //transform.Rotate(transform.up, turn);
    }

    private void BodyTurn()
    {
        Vector3 lookAt = new Vector3(bodyTurnVerInputValue, 0.0f, bodyTurnHorInputValue );
        lookAt = Camera.main.transform.TransformDirection(lookAt) * speed * Time.deltaTime; ;
        lookAt = new Vector3(lookAt.x, 0f, lookAt.z);
        
        if (lookAt != Vector3.zero)
        {
            bodyGO.transform.rotation = (Quaternion.LookRotation(lookAt, Vector3.up));
        }

        //// Determine the number of degrees to be turned based on the input, speed and time between frames.
        //float bodyTurn = m_BodyTurnInputValue * m_BodyTurnSpeed * Time.deltaTime;

        //// Apply this rotation.
        //bodyGO.transform.Rotate(0f, bodyTurn, 0f);

    }

    public void ApplyDamage(float damage)
    {
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;

            if (currentHealth >= 6)
            {
                uiComponent.health.sprite = hudRepo.health_6of6;
            }
            else if(currentHealth >= 5)
            {
                uiComponent.health.sprite = hudRepo.health_5of6;
            }
            else if (currentHealth >= 4)
            {
                uiComponent.health.sprite = hudRepo.health_4of6;
            }
            else if (currentHealth >= 3)
            {
                uiComponent.health.sprite = hudRepo.health_3of6;
            }
            else if (currentHealth >= 2)
            {
                uiComponent.health.sprite = hudRepo.health_2of6;
            }
            else if (currentHealth >= 1)
            {
                uiComponent.health.sprite = hudRepo.health_1of6;
            }


        }
        else
        {
            uiComponent.panel.SetActive(false);
            isAlive = false;
            gameObject.SetActive(false);
            GameObject.FindObjectOfType<GameManager>().SomeoneDiedEvent.Invoke();

        }
    }

    public void FireInTheHole()
    {
        nextFire = Time.time + fireRate;
        GameObject go = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        go.GetComponent<Shot>().parentTransform = this.transform;
        go.GetComponent<Shot>().playerShotBy = gameObject.name;
    }
}