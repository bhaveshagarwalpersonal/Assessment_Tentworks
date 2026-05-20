using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Camera Setup")]
    public GameObject mainCam;
    public Transform camMenuPoint, camPlayPoint;

    [Header("Player Setup")]
    public GameObject player1;

    [Header("UI Items")]
    public GameObject canvasGame;
    public GameObject canvasMenu;
    public GameObject canvasPause;

    [Header("Manager Setup")]
    public GameObject levelManager;

    private void OnEnable()
    {
        InitGameManager();
    }

    void InitGameManager() 
    {
        Instance = this;
    }

    #region Button Functions
    public void StartGame() 
    {
        

        MoveCam_MenuToPlay(() =>
        {
            player1.SetActive(true);
            levelManager.SetActive(true);
            canvasGame.SetActive(true);
            canvasMenu.SetActive(false);
            Debug.Log("Transition to Play Completed!");
        });
    }
    public void PauseGame()
    {
        canvasPause.SetActive(true);// could be better but due to just assessment timeline had to work with this
        Time.timeScale = 0;// could be better but due to just assessment timeline had to work with this
        /* MoveCam_PlayToMenu(() =>
         {
             player1.SetActive(false);
             levelManager.SetActive(false);
             canvasGame.SetActive(false);
             canvasMenu.SetActive(true);
             Debug.Log("Returned to Menu!");
         });*/
    }
    public void ResumGame() 
    {
        canvasPause.SetActive(false);// could be better but due to just assessment timeline had to work with this
        Time.timeScale = 1;// could be better but due to just assessment timeline had to work with this
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(0);// could be better but due to just assessment timeline had to work with this
    }
    #endregion
    void SetGameProps() 
    {

    }

    #region Camera Transitions

    public void MoveCam_MenuToPlay(System.Action onComplete = null)
    {
        Vector3 start = mainCam.transform.position;
        Vector3 end = camPlayPoint.position;

        // Horizontal offset (sideways instead of upward)
        Vector3 direction = (end - start).normalized;
        Vector3 sideOffset = Vector3.Cross(Vector3.up, direction).normalized * 5f;

        Vector3[] path = new Vector3[]
        {
        start,
        (start + end) / 2 + sideOffset,
        end
        };

        mainCam.transform.DOPath(path, 2f, PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                OnCamReachedPlay();
            });

        mainCam.transform.DORotateQuaternion(camPlayPoint.rotation, 2f);
    }

    public void MoveCam_PlayToMenu(System.Action onComplete = null)
    {
        Vector3 start = mainCam.transform.position;
        Vector3 end = camMenuPoint.position;

        // Same horizontal logic
        Vector3 direction = (end - start).normalized;
        Vector3 sideOffset = Vector3.Cross(Vector3.up, direction).normalized * -5f;

        Vector3[] path = new Vector3[]
        {
        start,
        (start + end) / 2 + sideOffset,
        end
        };

        mainCam.transform.DOPath(path, 2f, PathType.CatmullRom)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                OnCamReachedMenu();
            });

        mainCam.transform.DORotateQuaternion(camMenuPoint.rotation, 2f);
    }

    void OnCamReachedPlay()
    {
        Debug.Log("Camera reached Play Position");
        // Enable gameplay, UI, etc.
    }

    void OnCamReachedMenu()
    {
        Debug.Log("Camera returned to Menu Position");
        // Enable menu UI, disable gameplay, etc.
    }

    #endregion

    
}
