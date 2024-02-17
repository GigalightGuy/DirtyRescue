using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialToken : MonoBehaviour
{
    [SerializeField] private Image _blankImage;
    [SerializeField] private Slider _quickTimeSlider;
    [SerializeField] private TMP_Text _keyToPress;

    [SerializeField] private GameObject _animalAlive;
    [SerializeField] private GameObject _animalDead;
    public GameObject point;

    private float _timer;
    private bool _freeze;
    private bool _activated = false;
    private int _decreaseSpeed = 2;
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
        _quickTimeSlider.value = 3;
    }

    private void Update()
    {        
        if (_activated)
        {
            if (_freeze == false)
            {
                _quickTimeSlider.value = Mathf.MoveTowards(_quickTimeSlider.value, 0, _decreaseSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(_key) && _quickTimeSlider.value > 0)
            {
                _quickTimeSlider.value += 1;
                if (_quickTimeSlider.value == 10)
                {
                    _succeed = true;
                }
            }

            if (_succeed)
            {
                if (oneTimeeffects)
                {
                    _quickTimeSlider.gameObject.SetActive(false);
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

            if (_quickTimeSlider.value == 0)
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
                    _quickTimeSlider.gameObject.SetActive(false);
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
            _quickTimeSlider.gameObject.SetActive(true);
            _freeze = false;
            //Player.Instance.Root(3);
        }
    }
}
