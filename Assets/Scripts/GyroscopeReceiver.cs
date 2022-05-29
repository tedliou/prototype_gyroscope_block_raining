using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Server;

public class GyroscopeReceiver : MonoBehaviour
{
    public static GyroscopeReceiver instance;

    public bool isListening;
    public string ip;
    public int port;
    public GameObject block;
    public Button resetButton;

    public WebSocketServer wssv;
    public Quaternion gyroscope;
    public Quaternion originGyroscope;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        resetButton.onClick.AddListener(ResetGyroScopeRotate);

        wssv = new WebSocketServer($"ws://{ip}:{port}");
        wssv.AddWebSocketService<GyroscopeServer>("/data");
        wssv.Log.Output += (a, b) => Debug.Log($"[{a.Level}] {a.Message}");
        wssv.Start();
    }

    private void Update()
    {
        isListening = wssv.IsListening;
        transform.localRotation = gyroscope * originGyroscope;
    }

    private void OnDestroy()
    {
        wssv.Stop();
    }

    private void ResetGyroScopeRotate()
    {
        originGyroscope = Quaternion.Inverse(gyroscope);
    }
}

public class GyroscopeServer : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        var format = e.Data.Split(",");
        GyroscopeReceiver.instance.gyroscope = new Quaternion(float.Parse(format[0]), float.Parse(format[1]), float.Parse(format[2]), float.Parse(format[3]));
    }
}