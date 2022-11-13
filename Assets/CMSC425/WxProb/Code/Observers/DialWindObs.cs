using UnityEngine;

namespace Wx
{
    public class DialWindObs : MonoBehaviour
    {
        public Wind wind;

        public Vector3 axle;

        void Start()
        {
            wind.ReportWind += ReportWind;
            // Add code to obtain reports of state changes.
        }

        void ReportWind(float direction, float speed)
        {
            // Add code to manage reports of state changes.
            axle.z = direction;
            transform.rotation = Quaternion.Euler(axle);

        }
    }
}