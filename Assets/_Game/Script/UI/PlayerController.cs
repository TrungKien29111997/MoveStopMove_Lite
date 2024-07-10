using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform joyStickTransform;
    [SerializeField] Transform joyTransform;
    [SerializeField] int range;
    [SerializeField] Image[] imageControl;

    Vector2 startPress;
    [field: SerializeField] public Vector3 InputMobie { get; private set; }

    [field: SerializeField] public bool MobieActive { get; private set; }
    float timer;
    private void Start()
    {
        timer = 0;
    }

    void Update()
    {
        if (LevelManager.Instance.EGameStateL == EGameState.GamePlay)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    startPress = touch.position;
                    joyStickTransform.position = new Vector3(startPress.x, startPress.y, 0);
                }

                InputMobie = touch.position - startPress;

                if (InputMobie.sqrMagnitude > Mathf.Pow(range, 2))
                {
                    joyTransform.position = joyStickTransform.position + InputMobie.normalized * range;
                }
                else
                {
                    joyTransform.position = joyStickTransform.position + InputMobie;
                }

                timer += Time.deltaTime;
                if (timer > 0.1f)
                {
                    MobieActive = true;
                    SetJoyStickAlpha(1);
                }
            }
            else
            {
                timer = 0;
                SetJoyStickAlpha(0);
                InputMobie = Vector3.zero;
            }
        }
        else
        {
            SetJoyStickAlpha(0);
        }
    }

    public void SetJoyStickAlpha(float alpha)
    {
        for (int i = 0; i < imageControl.Length; i++)
        {
            imageControl[i].color = new Vector4(1, 1, 1, alpha);
        }
    }
}


