using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;

//using UnityEditor;

using AssemblyCSharp;

public class appwarp : MonoBehaviour
{
    public string apiKey = "Your API Key";
    public string secretKey = "Your Secret Key";
    public string roomid = "";
    public static string username = "AEew";
	
    public bool useUDP = true;
	
    Listener listen;
	
    public Transform remotePrefab;

    void Start()
    {
        WarpClient.initialize(apiKey, secretKey);
        listen = GetComponent<Listener>();
        WarpClient.GetInstance().AddConnectionRequestListener(listen);
        WarpClient.GetInstance().AddChatRequestListener(listen);
        WarpClient.GetInstance().AddLobbyRequestListener(listen);
        WarpClient.GetInstance().AddNotificationListener(listen);
        WarpClient.GetInstance().AddRoomRequestListener(listen);
        WarpClient.GetInstance().AddUpdateRequestListener(listen);
        WarpClient.GetInstance().AddZoneRequestListener(listen);
		
        // join with a unique name (current time stamp)
        username = System.DateTime.UtcNow.Ticks.ToString();
        WarpClient.GetInstance().Connect(username);
//        WarpClient.GetInstance().initUDP();
        //EditorApplication.playmodeStateChanged += OnEditorStateChanged;
		
        Controller ctr = GetComponent<Controller>();
        ctr.isLocal = true;
    }

    public float interval = 0.05f;
    float timer = 0;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
			
            float[] data_f = new float[4];
            data_f [0] = transform.position.x;
            data_f [1] = transform.position.y;
            data_f [2] = transform.position.z;
            data_f [3] = transform.FindChild("Capsule").transform.localRotation.eulerAngles.y;

			
            int data_len = (sizeof(float) * 4) + (username.Length * sizeof(char));
            byte[] data = new byte[data_len];
            System.Buffer.BlockCopy(data_f, 0, data, 0, sizeof(float) * 4);
            System.Buffer.BlockCopy(username.ToCharArray(), 0, data, sizeof(float) * 4, username.Length * sizeof(char));
	
            listen.sendBytes(data, useUDP);
			
            timer = interval;
        }
		
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        WarpClient.GetInstance().Update();
    }

    void OnGUI()
    {
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(10, 10, 400, 100), listen.getDebug());
    }
	
    /*void OnEditorStateChanged()
	{
    	if(EditorApplication.isPlaying == false) 
		{
			WarpClient.GetInstance().Disconnect();
    	}
	}*/
	
    void OnApplicationQuit()
    {
        WarpClient.GetInstance().Disconnect();
    }

    public void onMsg(string sender, string msg)
    {
        /*
		if(sender != username)
		{
			
		}
		*/
    }

    public void onBytes(byte[] msg)
    {	
        float[] data_f = new float[4];
        char[] data_c = new char[(msg.Length - (sizeof(float) * 4)) / sizeof(char)];
		
        System.Buffer.BlockCopy(msg, 0, data_f, 0, sizeof(float) * 4);
        System.Buffer.BlockCopy(msg, sizeof(float) * 4, data_c, 0, msg.Length - (sizeof(float) * 4));
		
        string sender = new string(data_c);
		
        if (sender != username)
        {
            GameObject remote;
            remote = GameObject.Find(sender);
            if (remote == null)
            {
                Object newRemote = Instantiate(remotePrefab, new Vector3(data_f [0], data_f [1], data_f [2]), Quaternion.identity);
                newRemote.name = sender;
//                GameObject go = GameObject.Find(sender);
//                go.GetComponent<Controller>().inputX = data_f [3];
//                go.GetComponent<Controller>().inputY = data_f [4];
            } else
            {
                
            
                RemoteThirdPersonSmooth rtps = remote.GetComponent<RemoteThirdPersonSmooth>();
                rtps.SetTransform(new Vector3(data_f [0], data_f [1], data_f [2]), data_f [3]);
            }		
        }
    }

    public void onUserLeft(string user)
    {
        GameObject remote;
        remote = GameObject.Find(user);
		
        if (remote != null)
        {
            Destroy(remote);
        }
    }
	
}
