using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // health 0~160
    private const float MaxEnemeyHealth = 160; // Enemy ealth: min 0 ~ max 160
    private const float MaxPlayerHealth = 160; // Player Health: min 0 ~ max 160
    private float EnemeyHealth = MaxEnemeyHealth;
    private float PlayerHealth = MaxPlayerHealth;
    //private bool isHealthUpdated;

    // game result
    private enum GameResult { win, lose, draw, proceeding}
    private GameResult gameResult;

    //UI Manager
    [SerializeField]
    private UIManager UIManager;
    
    // Audio Manager
    [SerializeField]
    private AudioManager AudioManager;
    
    // Controller Manager
    [SerializeField]
    private ControllerManager controllerManager;

    // Enemy GameObject
    [SerializeField]
    private GameObject Enemy;

    private Animator enemyAnimator;
    
    void Start(){
        UIManager = FindObjectOfType<UIManager>();
        AudioManager = FindObjectOfType<AudioManager>();
        controllerManager = FindObjectOfType<ControllerManager>();
        Enemy = GameObject.FindWithTag("Enemy");
        enemyAnimator = Enemy.GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            Debug.Log("Attacked");
            UpdateEnemyHealth(0);
        }
        
        // compare Player's and Enemy's Health
        if (PlayerHealth > 0 && EnemeyHealth <= 0)
        {
            gameResult = GameResult.win;
            UIManager.SendMessage("Show_04ResultUI_Win");
            controllerManager.SendMessage("setHandRayActive",true);
            Enemy.SetActive(false);
        }
        else if(PlayerHealth <= 0 && EnemeyHealth > 0)
        {
            gameResult = GameResult.lose;
            UIManager.SendMessage("Show_05ResultUI_Lose");
            controllerManager.SendMessage("setHandRayActive",true);
            Enemy.SetActive(false);
        }
        else if(PlayerHealth <= 0 && EnemeyHealth <= 0){
            gameResult = GameResult.draw;
            UIManager.SendMessage("Show_06ResultUI_Draw");
            controllerManager.SendMessage("setHandRayActive",true);
            Enemy.SetActive(false);
        }
        else
            gameResult = GameResult.proceeding;    
    }


    // update health
    public void UpdatePlayerHealth( float PHealthLoss){
        PlayerHealth -= PHealthLoss;
        Debug.Log("Enemy Health: "+ EnemeyHealth + " , Player Health: " + PlayerHealth );
        UIManager.SendMessage("Update_02PlayerHealthUI",PlayerHealth);
    }

    public void UpdateEnemyHealth( float EHealthLoss){
        enemyAnimator.SetTrigger("Attacked");
        EnemeyHealth -= EHealthLoss;
        Debug.Log("Enemy Health: "+ EnemeyHealth + " , Player Health: " + PlayerHealth );
        UIManager.SendMessage("Update_03EnemyHealthUI",EnemeyHealth);
    }

    // load level scene
    public void LoadLevel1(){
        Debug.Log("Loading Level1 Scene...");
        StartCoroutine("LoadScene","02 GameScene_Lv1");
    }
    public void LoadLevel2(){
        Debug.Log("Loading Level2 Scene...");
        StartCoroutine("LoadScene","03 GameScene_Lv2");
        //SceneManager.LoadScene("03 GameScene_Lv2",LoadSceneMode.Single);
    }
    public void LoadLevel3(){
        Debug.Log("Loading Level3 Scene...");
        StartCoroutine("LoadScene","04 GameScene_Lv3");
    }

    IEnumerator LoadScene(string SceneName){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
    }

    // Quit game
    public void QuitGame(){
        // save any game data here
         #if UNITY_EDITOR
             // Application.Quit() does not work in the editor so
             // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
             UnityEditor.EditorApplication.isPlaying = false;
         #else
             Application.Quit();
         #endif
    }

    // Restrat game
    public void RestartGame(){
        SceneManager.LoadScene("01 LevelSelectionScene",LoadSceneMode.Single);
    }

}
