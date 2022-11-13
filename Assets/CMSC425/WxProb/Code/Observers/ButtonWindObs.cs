using UnityEngine;
using TMPro;

namespace Wx
{
    public class ButtonWindObs : MonoBehaviour
    {
        public Wind wind;

        TextMeshPro tmp;
        string id;

        void Start()
        {
            // Add code to obtain reports of state changes.
            tmp = GetComponent<TextMeshPro>();
            wind.ReportState += ReportState;
            id = wind.airportID;
        }

        private void ReportState(bool isSimulated)
        {
            // Add code to manage reports of state changes.
            string state = "";

            if(isSimulated){
                state = "sim";
            }else {
                state = "net";
            }

            tmp.text = id + "\n" + state;
        }
    }
}