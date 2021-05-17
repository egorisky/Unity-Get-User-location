using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;


public class GPS : MonoBehaviour
{
    public Text LongText;
    public Text latText;

    // Start is called before the first frame update
    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }  
        StartCoroutine(StartGPS());
    }

    private IEnumerator StartGPS()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

            // Start service before querying location
            Input.location.Start();            
        


        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }

            // Access granted and location value could be retrieved
            latText.text = "latitude: " + Input.location.lastData.latitude.ToString();
            LongText.text = "longitude: " + Input.location.lastData.longitude.ToString();
        

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}
