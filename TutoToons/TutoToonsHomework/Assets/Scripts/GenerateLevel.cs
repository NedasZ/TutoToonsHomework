using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateLevel : MonoBehaviour
{
    public TextAsset level_data;
    public int desired_level = 1;
    private List<int> level_coordinates_parsed = new List<int>();
    public GameObject node;
    public GameObject rope;
    public float spacing_scale = 0.01f;
    public float object_scale = 1;
    public int counterMax = 1;
    public int counter = 1;
    public int drawCounter = 1;
    private bool isAnimating = false;
    public float animationSpeed = 10f;


    private float x1 = 0;
    private float y1 = 0;
    private float x2 = 0;
    private float y2 = 0;


    // Start is called before the first frame update
    void Start()
    {
        int count = 1;
        All_levels al = JsonUtility.FromJson<All_levels>(level_data.text);
        
        foreach(Level_coordinates coordinates in al.levels)
        {
            if(count == desired_level)
            {
                int i = 0;
                foreach(string coordinate in coordinates.level_data)
                {
                   level_coordinates_parsed.Add(int.Parse(coordinate));
                }
                break;
            }
            else if(count >3)
            {
                break;
            }
            else
            {
                count++;
            }
        }

        if(level_coordinates_parsed.Count > 0)
        {
            float x = 0;
            float y = 0;
            
            for (int i = 0; i < level_coordinates_parsed.Count; i++)
            {
                if(i % 2 == 0)
                {
                    x = level_coordinates_parsed[i] * spacing_scale;
                }
                else
                {
                    y = -level_coordinates_parsed[i] * spacing_scale;
                    GameObject nodeclone = Instantiate(node, new Vector2(x,y), new Quaternion()) as GameObject;
                    nodeclone.GetComponent<NodeScript>().Count = counterMax;
                    nodeclone.GetComponent<Transform>().localScale = new Vector2(object_scale, object_scale);
                    counterMax++;
                }
            }

            counterMax--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAnimating && drawCounter < counter-1)
        {
            isAnimating = true;
             x1 = level_coordinates_parsed[(drawCounter - 1) * 2] * spacing_scale;
             y1 = -level_coordinates_parsed[((drawCounter - 1) * 2)+1] * spacing_scale;
             x2 = level_coordinates_parsed[(drawCounter) * 2] * spacing_scale;
             y2 = -level_coordinates_parsed[((drawCounter) * 2) + 1] * spacing_scale;
            StartCoroutine(DrawRopeLine());
            drawCounter++;
        }
        if (!isAnimating && drawCounter == counter - 1 && (counter - 1) == counterMax)
        {
            isAnimating = true;
            x1 = level_coordinates_parsed[(drawCounter - 1) * 2] * spacing_scale;
            y1 = -level_coordinates_parsed[((drawCounter - 1) * 2) + 1] * spacing_scale;
            x2 = level_coordinates_parsed[0] * spacing_scale;
            y2 = -level_coordinates_parsed[1] * spacing_scale;
            StartCoroutine(DrawRopeLine());
            drawCounter++;
        }

        if(drawCounter == counterMax+1 && !isAnimating)
        {
            StartCoroutine(ExitLevel());
        }
    }

    IEnumerator DrawRopeLine()
    {
        Vector2 point1 = new Vector2(x1, y1);
        Vector2 point2 = new Vector2(x2, y2);

        Vector2 instantiatePosition;
        float ropeSize = rope.GetComponent<SpriteRenderer>().bounds.size.x * object_scale*1.3f;
        int segmentsNeeded = Mathf.RoundToInt(Vector2.Distance(point1, point2) / ropeSize);
        float lerpValue = 0f;

        float distance = 1f / segmentsNeeded;

        for (int i = 0; i < segmentsNeeded; i++)
        {
            
            instantiatePosition = Vector2.Lerp(point1, point2, lerpValue);
            GameObject ropeclone = Instantiate(rope, instantiatePosition, transform.rotation);
            ropeclone.GetComponent<Transform>().localScale = new Vector2(object_scale*2, object_scale*2);

            var dir = new Vector3(x2, y2) - ropeclone.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            ropeclone.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
            
            Vector2 moveto = Vector2.Lerp(point1, point2, (lerpValue+distance));
            float step = animationSpeed * Time.deltaTime;
           
            while (Vector2.Distance(ropeclone.transform.position,moveto) >0)
            {
                ropeclone.transform.position = Vector2.MoveTowards(ropeclone.transform.position, moveto, step);
                yield return new WaitForSeconds(0.05f);
            }
            lerpValue += distance;
        }
        isAnimating = false;
        
    }


    IEnumerator ExitLevel()
    {

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("MainMenu");
    }

}




