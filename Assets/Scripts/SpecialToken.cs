using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialToken : MonoBehaviour
{
    [SerializeField] private Image _blankImage;
    [SerializeField] private Image _quickTimeBar;
    [SerializeField] private TMP_Text _keyToPress;

    [SerializeField] private GameObject _animalAlive;
    [SerializeField] private GameObject _animalDead;
    public GameObject point;

    private float _timer;
    private bool _freeze;
    private bool _activated = false;
    private float _decreaseSpeed = 0.2f;
    private int _flyingSpeed = 10;
    private bool _succeed = false;
    private bool oneTimeeffects = true;

    [SerializeField] private SOFloat scoreSO;

    KeyCode _key;
    KeyCode[] availableOptions = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    private void Start()
    {
        int rand = Random.Range(0, 3);
        _key = availableOptions[rand];
        if (rand == 0)
        {
            _keyToPress.text = "Press 1 Quickly";
        }
        if (rand == 1)
        {
            _keyToPress.text = "Press 2 Quickly";
        }
        if (rand == 2)
        {
            _keyToPress.text = "Press 3 Quickly";
        }
        _quickTimeBar.fillAmount = 0.3f;
    }

    private Color InterpolateInHSVSpace(Color a, Color b, float t)
    {
        Vector3 aHSV;
        Color.RGBToHSV(a, out aHSV.x, out aHSV.y, out aHSV.z);
        Vector3 bHSV;
        Color.RGBToHSV(b, out bHSV.x, out bHSV.y, out bHSV.z);
        Vector3 interpolatedHSVColor = Vector3.Lerp(aHSV, bHSV, t);
        return Color.HSVToRGB(interpolatedHSVColor.x, interpolatedHSVColor.y, interpolatedHSVColor.z);
    }

    private void Update()
    {        
        if (_activated)
        {
            if (_freeze == false)
            {
                _quickTimeBar.fillAmount = Mathf.MoveTowards(_quickTimeBar.fillAmount, 0, _decreaseSpeed * Time.deltaTime);
                _quickTimeBar.color = InterpolateInHSVSpace(Color.red, Color.green, _quickTimeBar.fillAmount);
            }

            if (Input.GetKeyDown(_key) && _quickTimeBar.fillAmount > 0f)
            {
                _quickTimeBar.fillAmount += 0.1f;
                _quickTimeBar.color = InterpolateInHSVSpace(Color.red, Color.green, _quickTimeBar.fillAmount);
                if (_quickTimeBar.fillAmount >= 1f)
                {
                    _succeed = true;
                }
            }

            if (_succeed)
            {
                if (oneTimeeffects)
                {
                    _quickTimeBar.transform.parent.gameObject.SetActive(false);
                    _freeze = true;
                    _blankImage.gameObject.SetActive(false);
                    scoreSO.Value += 100;
                    oneTimeeffects = false;
                }

                _timer += Time.deltaTime;
                _keyToPress.text = "Saved!";

                _animalAlive.transform.right = _animalAlive.transform.position - point.transform.position;
                _animalAlive.transform.position = Vector2.MoveTowards(_animalAlive.transform.position, point.transform.position, _flyingSpeed * Time.deltaTime);

                if (_timer > 1f)
                {
                    _animalAlive.gameObject.SetActive(false);
                    _keyToPress.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                    _timer = 0;
                }
            }

            if (_quickTimeBar.fillAmount <= 0f)
            {
                _timer += Time.deltaTime;
                _keyToPress.text = "Rip :(";
                _animalAlive.gameObject.SetActive(false);
                _animalDead.gameObject.SetActive(true);
                if (_timer > 1.0f)
                {
                    _keyToPress.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                    _timer = 0;
                    _animalDead.gameObject.SetActive(false);
                    _quickTimeBar.transform.parent.gameObject.SetActive(false);
                }
                _freeze = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _activated = true;
            _keyToPress.gameObject.SetActive(true);
            _quickTimeBar.transform.parent.gameObject.SetActive(true);
            _freeze = false;
            //Player.Instance.Root(3);
        }
    }
}
