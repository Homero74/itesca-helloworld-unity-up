using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewPlayer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMesh;

    [SerializeField]
    float moveSpeed = 2f;

    Animator anim;

    int score;

    Controles gameInputs;

    [SerializeField]
    GameObject Sonido;

    [SerializeField]
    float JumpForce = 5f;

    Rigidbody rb;

    [SerializeField]

    Color rayColor = Color.magenta;

    [SerializeField, Range(0.1f,10f)]
    float rayDistance = 5;
    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Transform rayTransform;


    void Awake(){
        gameInputs = new Controles();
        anim = GetComponent<Animator>();
        rb  = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInputs.Land.Jump.performed+=    _=>Jump();
    }
    void Jump()
    {
        if(isGrounding)
        {
            anim.SetTrigger("jump");
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse );
        }
    }

    void OnEnable(){
        gameInputs.Enable();
        
    }
    void OnDisable(){
        gameInputs.Disable();

    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        if(IsMoving)
        {
            transform.Translate( Vector3.forward * Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.LookRotation(new Vector3(Axis.x, 0f, Axis.y));
        }
    }

    void LateUpdate()
    {
        anim.SetFloat("movement", AxisMagnitudeAbs);
        anim.SetBool("ground",isGrounding);
    }
    /// <summary>
    /// Retunrs the axis with H input and V Input.
    /// </summary>
    /// <returns></returns>
    Vector3 Axis => gameInputs.Land.Move.ReadValue<Vector2>();

    /// <summary>
    /// Check if player is moving with inputs H and V.
    /// </summary>
    bool IsMoving => AxisMagnitudeAbs > 0;

    /// <summary>
    /// Returns the magnitude of the Axis with inputs H and V.
    /// </summary>
    /// <returns></returns>
    float AxisMagnitudeAbs => Mathf.Abs(Axis.magnitude);
    
    
    bool isGrounding => Physics.Raycast(rayTransform.position, -transform.up, rayDistance,groundLayer);

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("collectable"))
        {
            Instantiate(Sonido);
            score++;
            textMesh.text = $"Score: {score}";
            Destroy(other.gameObject);
        }   
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color=rayColor;
        Gizmos.DrawRay(transform.position, -transform.up * rayDistance);    
    }
}
