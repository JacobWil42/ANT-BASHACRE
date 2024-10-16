using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        //take current position and assign start position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //update the players position using default Unity inputs
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
    }
}