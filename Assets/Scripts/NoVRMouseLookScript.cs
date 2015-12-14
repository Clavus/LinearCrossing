using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.VR;

public class NoVRMouseLookScript : MonoBehaviour {

    public float startSensitivity = 10F;

    public float sensitivityStep = 0.25f;
    public float minSensitivity = 0.5f;
    public float maxSensitivity = 20f;

    public float minimumY = -80F;
    public float maximumY = 80F;

    public static float sensitivity;

    [SerializeField]
    private PlayerScript player;

    private float rotationY = 0F;
    private bool lockMouse;

    void Start()
    {
        if (!VRSettings.enabled)
            LockMouse(true);

        if (PlayerPrefs.HasKey("sensitivity"))
            sensitivity = PlayerPrefs.GetFloat("sensitivity");
        else
            sensitivity = startSensitivity;

    }

    void Update ()
    {

        if (VRSettings.enabled)
        {
            LockMouse(false);
            return;
        }

        if (lockMouse)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X")*sensitivity;

            rotationY += Input.GetAxis("Mouse Y")*sensitivity;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                sensitivity = Mathf.Clamp(sensitivity + (Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : -1) * sensitivityStep, minSensitivity, maxSensitivity);
                player.ShowCrosshairMessage("Sensitivity: " + sensitivity.ToString("0.00"), Color.cyan);
                PlayerPrefs.SetFloat("sensitivity", sensitivity);
            }
        }

        // Keep mouse in editor window
        if (Application.isEditor)
        {
            bool mouseOverWindow = Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height;

            //check if the cursor is locked but not centred in the game viewport - i.e. like it is every time the game starts
            if (lockMouse && !mouseOverWindow)
                LockMouse(true);

            //if the player presses Escape, unlock the cursor
            if (lockMouse && Input.GetKeyDown(KeyCode.Escape))
                LockMouse(false);

            //if we press ctrl while mouse is over viewport, lock again
            if (!lockMouse && mouseOverWindow && Input.GetMouseButton(0))
                LockMouse(true);
        }
        else if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && lockMouse) // in case application lost focus
            Cursor.visible = false;

    }

    public void LockMouse(bool b)
    {
        lockMouse = b;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = !b;

        if (b)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
