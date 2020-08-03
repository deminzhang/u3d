using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour {

    public float FPSInterval = 0.5F;
    public bool FPSDisplay = false;

    private float accum = 0;
    private int frames = 0;
    private float timeleft;
    private string stringFps;
	// Use this for initialization
    void Start()
    {
        timeleft = FPSInterval;
	}
	
	// Update is called once per frame
    void Update()
    {

        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            string format = string.Format("FPS: {0:F2}", fps);
            stringFps = format;
            timeleft = FPSInterval;
            accum = 0.0F;
            frames = 0;
        }
	}
    void OnGUI()
    {
        if (!FPSDisplay) return;
        GUI.Label(new Rect(0, 0, 100, 50), stringFps);
        //GUI.Label(new Rect(0, 15, 100, 50), stringFps);

    }

}
