using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToiletBar : MonoBehaviour {

    public int minHeight;
    public int maxHeight;
    public int initialTop;

    public float fill;

    public Color color1;
    public Color color2;
    public Color color3;

    Color myColor;

    public Image almostfullPanel;
    public Text almostfullLabel;

    Image m_Image;
    RectTransform m_RectTransform;

	// Use this for initialization
	void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Image = GetComponent<Image>();

        UpdateBar(true);
        UpdateAlmostFullLabel(true);
    }
	
	// Update is called once per frame
	void Update()
    {
        UpdateBar(false);
        UpdateAlmostFullLabel(false);
    }

    void UpdateBar(bool instantChange)
    {
        float height = minHeight + (fill * (maxHeight - minHeight)) / 100;
        m_RectTransform.sizeDelta = new Vector2(m_RectTransform.sizeDelta.x, height);

        if (fill <= 10)
        {
            myColor = color1;
        }
        else if (fill <= 50)
        {
            myColor = color2;
        }
        else if (fill <= 100)
        {
            myColor = color3;
        }

        m_Image.color = Color.Lerp(m_Image.color, myColor, instantChange ? 1.0f : 0.05f);
    }

    void UpdateAlmostFullLabel(bool instantChange)
    {
        Color tempPanelColor = almostfullPanel.color;
        Color tempLabelColor = almostfullLabel.color;

        if (fill >= 80)
        {
            tempPanelColor.a = 1;
            tempLabelColor.a = 1;

            if (fill == 100)
            {
                almostfullLabel.text = "TOILET\nFULL";
            }
            else
            {
                almostfullLabel.text = "ALMOST\nFULL";
            }
        }
        else
        {
            tempPanelColor.a = 0;
            tempLabelColor.a = 0;
        }

        if (almostfullPanel.color.a != tempPanelColor.a)
        {
            almostfullPanel.color = Color.Lerp(almostfullPanel.color, tempPanelColor, instantChange ? 1.0f : 0.05f);
        }
        if (almostfullLabel.color.a != tempLabelColor.a)
        {
            almostfullLabel.color = Color.Lerp(almostfullLabel.color, tempLabelColor, instantChange ? 1.0f : 0.05f);
        }
    }
}
