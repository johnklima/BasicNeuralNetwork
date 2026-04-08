using Alteruna;

using UnityEngine;

public class PlayerController : CommunicationBridge
{
    public float Speed = 10.0f;
    public float RotationSpeed = 180.0f;

    private Renderer _renderer;

    public override void Possessed(bool isPossessor, User user)
    {
        enabled = isPossessor;

        if (isPossessor)
        {
            Debug.Log("Possessed " + transform.name);
        }
    }

    // Only runs when possessed by me.
    void Update()
    {
        // Get the horizontal and vertical axis.
        float _translation = Input.GetAxis("Vertical") * Speed;
        float _rotation = -Input.GetAxis("Horizontal") * RotationSpeed;

        _translation *= Time.deltaTime;
        _rotation *= Time.deltaTime;

        transform.Translate(0, _translation, 0, Space.Self);
        transform.Rotate(0, 0, _rotation);
    }
}
