using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Wx
{
    // This will be your most complex class. It must maintain events that
    // inform observers of new wind data and when it changes state from
    // simulated data to network-sourced data.

    public class Wind : MonoBehaviour
    {
        public string airportID;

        public event Action<float, float> ReportWind;
        public event Action<bool> ReportState;

        // Use these values to help in making controlled random
        // changes when simulating wind reports.

        const int windDirectionMax = 360;
        const float windSpeedMax = 30;
        const float windSpeedMin = 5;
        const float deltaWindSpeed = 3;
        const float deltaWindDirection = 10;

        System.Random r = new System.Random();

        float windDirection = 0;
        float windSpeed = 0;

        bool isSimulated = true;

        public void ChangeState()
        {
            // Add code here to switch states and inform observers
            if (isSimulated)
            {
                StopCoroutine(SimulateWind());
                isSimulated = !isSimulated;
                StartCoroutine(GetNetworkWind());
            }
            else
            {
                StopCoroutine(GetNetworkWind());
                isSimulated = !isSimulated;
                StartCoroutine(SimulateWind());
            }
            
        }

        IEnumerator SimulateWind()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while (true)
            {
                // Add code here that reports simulated wind direction
                // and speeds every half-second to the proper observers.
                // Be sure to use some randomness to change the direction
                // and speed.


                int seed = r.Next(6);
                if (isSimulated)
                {
                    yield return null;
                    if (seed < 2)
                    {
                        windDirection = windDirection - deltaWindDirection + windDirectionMax;
                        windDirection %= windDirectionMax;
                    }
                    else if (seed > 3)
                    {
                        windDirection += deltaWindDirection;
                        windDirection %= windDirectionMax;
                    }
                    if (windSpeed + deltaWindSpeed <= windSpeedMax)
                    {
                        if (windSpeed - deltaWindSpeed >= windSpeedMin)
                        {
                            if (seed < 2)
                            {
                                windSpeed -= deltaWindSpeed;
                            }
                            else if (seed > 3)
                            {
                                windSpeed += deltaWindSpeed;
                            }
                        }
                        else
                        {
                            if (seed > 2)
                            {
                                windSpeed += deltaWindSpeed;
                            }
                        }
                    }
                    else
                    {
                        if (seed < 3)
                        {
                            windSpeed -= deltaWindSpeed;
                        }
                    }

                    ReportWind(windDirection, windSpeed);
                    Debug.Log("airport is" + airportID + " windDirection is" + windDirection + " windSpeed is" + windSpeed);
                    ReportState(isSimulated);
                }
                else yield break;

                

                yield return wait;
            }
        }

        IEnumerator GetNetworkWind()
        {
            WaitForSeconds wait = new WaitForSeconds(10f);

            while (true)
            {
                // Add code here to obtain live data from an airport
                // JSON weather feed, and report it to the proper
                // observers. Query the airport data every ten seconds.
                // DO NOT query the airport data more often than five
                // times in a single second, or the server will block
                // you. Also, do not query the airport data more than
                // 10,000 times in a single day (which would be about
                // once every eight to nine seconds).

                // Query for network data with the UnityWebRequest class.
                // To query for JSON data from airport IAD, you would use
                // the Get method and this string as its argument:
                //
                // "https://api.weather.gov/stations/KIAD/observations/latest"
                //
                // The Get method returns a UnityWebRequest object. Use its
                // SendWebRequest method to send the actual query. It returns
                // a UnityWebRequestAsyncOperation object which you can use
                // in a yield return to pause a coroutine until the request
                // has been answered. Note that you will yield TWICE for each
                // request: once to pause until the request has been answered,
                // and a second time to pause for ten seconds before sending
                // another request.
                if (!isSimulated)
                {
                    using (UnityWebRequest www = UnityWebRequest.Get("https://api.weather.gov/stations/K" + airportID + "/observations/latest"))
                    {
                        yield return www.SendWebRequest();
                        if (www.result==UnityWebRequest.Result.ConnectionError || www.result==UnityWebRequest.Result.ProtocolError)
                        {
                            Debug.Log(www.error);
                        }
                        else
                        {
                            String json = www.downloadHandler.text;
                            Wx wx = JsonUtility.FromJson<Wx>(json);
                            Debug.Log(json);
                            Debug.Log("windDirection:"+wx.properties.windDirection.value+" windSpeed:"+wx.properties.windSpeed.value);
                            ReportWind(wx.properties.windDirection.value,wx.properties.windSpeed.value);
                            ReportState(isSimulated);
                        }
                    }

                }
                else yield break;

                yield return wait;
            }
        }

        //for testing only
        private void Start()
        {
            windDirection = r.Next(0, windDirectionMax+1);
            windSpeed = r.Next((int)windSpeedMin, (int)windSpeedMax);
            if (isSimulated)
            {
                StartCoroutine(SimulateWind());
            }
            else
            {
                StartCoroutine(GetNetworkWind());
            }
        }
    }
}