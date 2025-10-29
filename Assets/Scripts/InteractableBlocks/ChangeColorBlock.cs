using UnityEngine;

public class ChangeColorBlock : MonoBehaviour, IChangeableBlock
{

    private Color _currentColor;
    private float _time = 0;

    public void ApplyChange()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();

        player.ChangeColor(_currentColor);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int colorRandom = Random.Range(0, 3);
        switch (colorRandom)
        {
            case 0:
                _currentColor = Color.red;
                break;
            case 1:
                _currentColor = Color.blue;
                break;
            case 2:
                _currentColor = Color.green;
                break;
            // This should never happen, but just in case
            default:
                _currentColor = Color.red;
                break;
        }

        GetComponent<SpriteRenderer>().color = _currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= 5)
        {
            SwapColors();
            GetComponent<SpriteRenderer>().color = _currentColor;
            _time = 0;
        }
    }

    private void SwapColors()
    {
        _currentColor = _currentColor == Color.red ? Color.blue : _currentColor == Color.blue ? Color.green : Color.red;
    }
}
