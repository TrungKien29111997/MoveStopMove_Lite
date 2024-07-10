using UnityEngine;

[ExecuteInEditMode]
public class TestAnimationClip : MonoBehaviour
{
    public Animator object1;
    public float frameNum1;
    public int numberOfFrames1;
    public string animationClip1Name;
    public int animationLayer1;

    public Animator object2;
    public int numberOfFrames2;
    public string animationClip2Name;
    public int animationLayer2;


    void OnValidate()
    {
        if (object1 != null)
        {
            object1.speed = 1;
            object1.Play(animationClip1Name, animationLayer1, frameNum1 / numberOfFrames1);
            object1.Update(Time.deltaTime);
        }
        if (object2 != null)
        {
            object2.speed = 0;
            object2.Play(animationClip1Name, animationLayer2, frameNum1 / numberOfFrames2);
            object2.Update(Time.deltaTime);
        }
    }
}
