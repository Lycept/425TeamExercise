using UnityEngine;
using TMPro;

namespace Wx
{
    public class TextWindObs : MonoBehaviour
    {
        public Wind wind;

        TextMeshPro tmp;

        void Start()
        {
            tmp = GetComponent<TextMeshPro>();
            wind.ReportWind += ReportWind;

        }

        void ReportWind(float d, float s)
        {
            string direction = "";
            string speed = s.ToString();
            
            if (d >= 157.5 && d < 202.5) {
                direction = "S";
            } else if (d >= 202.5 && d < 247.5){
                direction = "SW";
            } else if (d >= 247.5 && d < 292.5){
                direction = "W";
            } else if (d >= 292.5 && d < 337.5){
                direction = "NW";
            } else if (d >= 337.5 && d < 22.5){
                direction = "N";
            } else if (d >= 22.5 && d < 67.5){
                direction = "NE";
            } else if (d >= 67.5 && d < 112.5){
                direction = "E";
            } else if (d >= 112.5 && d < 157.5){
                direction = "SE";
            } 

            tmp.text = direction + "\n" + speed;

        }
    }
}