namespace MaximovInk
{
    public class BuildingManager : MonoBehaviourSingleton<BuildingManager>
    {
        private FracturedObject _fracturedObject;

        private void Awake()
        {
            _fracturedObject = FindObjectOfType<FracturedObject>();
        }
    }
}
