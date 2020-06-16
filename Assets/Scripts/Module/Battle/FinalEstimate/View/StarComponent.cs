using Common;
using uTools;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class StarComponent : MonoBehaviour
    {
        public void ShowStarAnimation(int num)
        {
            transform.gameObject.Show();

            Color grayColor = new Color(160/255.0f,160/255.0f,160/255.0f);

            float delay = 0.25f;
            
            for (int i = 0; i < 3; i++)
            {
                Transform item = transform.GetChild(i);
                
                if (i < num)
                {
                    item.gameObject.Show();
                }
                else
                {
                    item.gameObject.Hide();
                    item.gameObject.SetActive(true); 
                    item.Find("xingxing_00").gameObject.Hide();
                    item.Find("xingxing").gameObject.Hide();
                    item.GetComponent<Image>().color = grayColor;
                }
            }
            
            AudioManager.Instance.PlayEffect("story_" + num + "_star", 1f);
        }
        
        public void ShowStar(int num)
        {
            transform.gameObject.Show();

            Color grayColor = new Color(160/255.0f,160/255.0f,160/255.0f);

            float delay = 0.25f;
            
            for (int i = 0; i < 3; i++)
            {
                Transform item = transform.GetChild(i);
                
                if (i < num)
                {
                    item.gameObject.Show();
                }
                else
                {
                    item.gameObject.Hide();
                    item.gameObject.SetActive(true); 
                    item.Find("xingxing_00").gameObject.Hide();
                    item.GetComponent<Image>().color = grayColor;
                }
            }
        }
    }
}