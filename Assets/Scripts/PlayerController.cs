using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO.Ports;
using System.Text;
using System;

public class PlayerController : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM9", 115200);
    public string receivedstring;
    public float incrVal = 1.0f;
    public float speedVal = 40.0f;
    public float speedDecreaseVal = 2.0f;
    public const float maxSpeedVal = 100.0f;
    private float previousSpeedVal = -1.0f; // Setting to a value that it can never be just for the first frame
    public Vector2 position;
    public int test = 0;
    public string[] datas;
    public Text speedDisplay;
    public static event Action OnPlayerDeath;
    public int lives = 3;
    public Text livesDisplay;


    void Start()
    {
        if (test == 0) {
            data_stream.Open();
        } else {
            StartCoroutine(PostData_Coroutine(speedVal));
        }
        position = transform.position;
        StartCoroutine(SpeedDecrease_Coroutine());
        UpdateLivesDisplay();
    }

    void Update()
    {
        if (position.y < 3.1 && position.y > -3.1)
        {
            transform.position = Vector2.MoveTowards(transform.position, position, 40.0f * Time.deltaTime);
        }

        if (test == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                position = new Vector2(transform.position.x, transform.position.y + incrVal);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                position = new Vector2(transform.position.x, transform.position.y - incrVal);
            }
        }
        else
        {
            // data_stream.DiscardInBuffer();
            if (data_stream.BytesToRead > 0) {
                receivedstring = data_stream.ReadLine();
            } else {
                receivedstring = "0,0";
            }
            Debug.Log(receivedstring);
            datas = receivedstring.Split(',');

            if (float.Parse(datas[0]) == 1)
            {
                position = new Vector2(transform.position.x, transform.position.y + incrVal);
            }
            else if (float.Parse(datas[0]) == -1)
            {
                position = new Vector2(transform.position.x, transform.position.y - incrVal);
            }
        }

        // Check if speedVal has changed since last frame
        if (speedVal != previousSpeedVal)
        {
            // If it has changed, start the coroutine to post the data
            if (test == 0)
            {
                SendSpeedToSerial(speedVal);
            } else {
                StartCoroutine(PostData_Coroutine(speedVal));
            }
            previousSpeedVal = speedVal; // Update previousSpeedVal to the new value
        }
    }

    public void UpdateSpeedDisplay(float newSpeedValue) {
        if (newSpeedValue < maxSpeedVal) {
            speedVal = newSpeedValue;
            speedDisplay.text = "Speed: " + speedVal.ToString("F0");
        } else {
            speedVal = maxSpeedVal;
            speedDisplay.text = "Speed: MAX";
        }
    }

    public void UpdateLivesDisplay() {
        livesDisplay.text = "Lives: " + lives.ToString();
        if (lives <= 0) {
            PlayerDeath();
        }
    }

    public void PlayerDeath() {
        if (test == 0) {
            SendSpeedToSerial(0.0f);
        } else {
            StartCoroutine(PostData_Coroutine(0.0f));
        }
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (data_stream.IsOpen) {
            data_stream.Close();
        }
    }

    IEnumerator SpeedDecrease_Coroutine() {
        while (speedVal > 0.0f)
        {
            UpdateSpeedDisplay(speedVal - speedDecreaseVal);
            yield return new WaitForSeconds(1.0f);
        }

        PlayerDeath();
    }

    void SendSpeedToSerial(float speed) {
        if (data_stream.IsOpen) {
            try {
                Debug.Log("Sending speed to serial: " + speed.ToString());
                data_stream.WriteLine(speed.ToString());
            }
            catch (Exception e) {
                Debug.Log("Error when sending to serial: " + e.Message);
            }
        }
    }

    IEnumerator PostData_Coroutine(float currentSpeedVal)
    {
        Debug.Log("Sending data...");
        string uri = "https://nodejs--ianjenatz.repl.co/webhook";

        string jsonData = "{\"speed\":" + currentSpeedVal.ToString("F1") + "}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(uri, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data sent successfully");
            }
        }
    }
}
