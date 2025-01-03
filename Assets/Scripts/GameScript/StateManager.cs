using UnityEngine;

public class StateManager : MonoBehaviour {
//States
public int PlayerState = 0;  //0 = office, 1 = MonitorUp, 2 = monitorDown
public int previousState = 0;
//time
public float DownTime = 0.13f;

//Scripts
public MonitorManager monitorManager;

private void Start() {
    
}
private void Update() {
    if (!monitorManager.isMonitorActive) {
        if (previousState == 1) {
            PlayerState = 2;
            DownTime -= Time.deltaTime;
            if (DownTime <= 0) {
                previousState = 0;
                PlayerState = 0;
                DownTime = 0.13f;
            }
        } else {
            PlayerState = 0;
        }
    } else {
        PlayerState = 1;
        previousState = 1;
    }
}
}