using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;
 
public sealed class GameObjectPointCloudVisualiser : MonoBehaviour
{ 
    PointBuffer pointBuffer;
    Camera maincam;
    PrefabRenderer backgroundPrefabRenderer;
    PrefabRenderer foregroundPrefabRenderer;
    public GameObject backgroundCubePrefab;
    public GameObject foregroundCubePrefab;
    public float beepDistance;

    void OnPointCloudChanged(ARPointCloudUpdatedEventArgs eventArgs)
    {
        Debug.Log("Pranav: Previous number of points: " + prevNumOfPoints);
        Debug.Log("Pranav: Current number of points: " + m_PointCloud.positions.Value.ToArray().Length);

        if (m_PointCloud.positions.HasValue)
        {
            if (!messageSent && m_PointCloud.positions.Value.ToArray().Length < 0.1 * prevNumOfPoints) {
                Debug.Log("Pranav: CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH CRASH");
                sendMessage();
            }
            List<ulong> identifiers = new List<ulong>();
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < m_PointCloud.identifiers.Value.Length; i++) {
                if (m_PointCloud.confidenceValues.Value[i] > 0.5f) {
                    identifiers.Add(m_PointCloud.identifiers.Value[i]);
                    positions.Add(m_PointCloud.positions.Value[i]);
                }
            }
            pointBuffer.addPoints(identifiers.ToArray(),positions.ToArray());
            prevNumOfPoints = identifiers.ToArray().Length;
            
            /*for (int i = 0; i < positions.Count; i++){
                positions[i] = new Vector3(positions[i].x, 0, positions[i].z);
            }*/
            positions.Sort((p1,p2)=>Vector3.Distance(maincam.transform.position,p1).CompareTo(Vector3.Distance(maincam.transform.position,p2)));
            //positions = positions.Where(p => Vector3.Angle(maincam.transform.forward,p-maincam.transform.position)<0.5).ToList();
            positions = positions.Where(p => (p-maincam.transform.position).magnitude < beepDistance).ToList();
            foregroundPrefabRenderer.RenderPoints(positions.GetRange(0,Math.Min(positions.Count, 10)));
            Debug.Log($"Count points: {positions.Count}");
        }
        
        Vector3[] points = pointBuffer.getClosest(maincam.transform.position,10);
        backgroundPrefabRenderer.RenderPoints(points.ToList());
    }

    void Awake()
    {
        maincam = GameObject.Find("AR Camera").GetComponent<Camera>();
        m_PointCloud = GetComponent<ARPointCloud>();
        backgroundPrefabRenderer = new PrefabRenderer(backgroundCubePrefab);
        foregroundPrefabRenderer = new PrefabRenderer(foregroundCubePrefab);
        pointBuffer = new PointBuffer(100000);
    }

    void OnEnable()
    {
        m_PointCloud.updated += OnPointCloudChanged;
        UpdateVisibility();
        prevNumOfPoints = 0;
    }

    void OnDisable()
    {
        m_PointCloud.updated -= OnPointCloudChanged;
        UpdateVisibility();
    }

    void Update()
    {
        UpdateVisibility();
    }

    void UpdateVisibility()
    {
        var visible =
            enabled &&
            (m_PointCloud.trackingState != TrackingState.None);

        SetVisible(visible);
    }

    void SetVisible(bool visible)
    {
        
    }

    ARPointCloud m_PointCloud;

    GameObject[] soundCubes;
    // ParticleSystem m_ParticleSystem;

    // ParticleSystem.Particle[] m_Particles;

    int m_NumParticles;

    static List<Vector3> s_Vertices = new List<Vector3>();

    int prevNumOfPoints;

    string text = "TWILIO_ACCOUNT_SID,TWILIO_AUTH_TOKEN";
    bool messageSent = false;

    string authenticate(string username, string password)
    {
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }

    public void sendMessage()
    {
        messageSent = true;
        string[] accountData = text.Split(',');
        string sendingPhoneNumber = "SENDING_NUMBER";
        string receivingPhoneNumber = "RECEIVING_NUMBER";

        string messageContent = "You have fallen over!";

        string authorization = authenticate(accountData[0], accountData[1]);
        string url = "https://api.twilio.com/2010-04-01/Accounts/" + accountData[0] + "/Messages.json";

        WWWForm form = new WWWForm();
        form.AddField("Body", messageContent);
        form.AddField("From", sendingPhoneNumber);
        form.AddField("To", receivingPhoneNumber);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.SetRequestHeader("AUTHORIZATION", authorization);


        Debug.Log(www.SendWebRequest());
    }
}