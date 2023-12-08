using UnityEngine;
using UnityEngine.UI;

public class BulletCounterView : MonoBehaviour
{
    public Image[] BulletImages;

    [SerializeField] private int _maxBullets;
    [SerializeField] private int _curBullets;

    private void Start()
    {
        RefillAmmo();
    }

    public void RefillAmmo()
    {
        _curBullets = _maxBullets;
        UpdateAmmo();
    }

    public void ShootBullet()
    {
        _curBullets = Mathf.Clamp(_curBullets - 1, 0, _maxBullets);
        UpdateAmmo();
    }    

    private void UpdateAmmo()
    {
        int i = 0;
        for (i = 0; i < _maxBullets - _curBullets; i++)
        {
            BulletImages[i].enabled = false;
        }

        for (; i < _maxBullets; i++)
        {
            BulletImages[i].enabled = true;
        }
    }
}