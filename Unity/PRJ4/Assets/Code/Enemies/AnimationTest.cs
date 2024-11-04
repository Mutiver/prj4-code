using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public float delay;
    public bool die;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (die)
        {
            delay += Time.deltaTime;
            animator.Play("DIE");
            if (delay >= 2) Destroy(this.gameObject);
        }

    }
}
