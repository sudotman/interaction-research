using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
using System.IO;

public class EventTester : MonoBehaviour
{
    public GameObject lineChart;
    List<int> listA = new List<int>();
    List<int> listB = new List<int>();


    int val = 0;

    Pose gripValue = new Pose();

    private void Awake()
    {

    }

/*    private void Start()
    {

        
        var reader = new StreamReader(@"C:\Users\Admin\Downloads\testing.csv");

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');

            listA.Add(int.Parse(values[0]));
            listB.Add(int.Parse(values[1]));

            lineChart.GetComponent<SimplifiedLineChart>().AddData("serie0", int.Parse(values[0]), int.Parse(values[1]));

        }
    }
*/
    
    
    
    
    
    
    /*public void OnMiddleClick(GameObject particleSystem)
    {
        if(!particleSystem.GetComponent<ParticleSystem>().isPlaying)
            particleSystem.GetComponent<ParticleSystem>().Play();
        else
            particleSystem.GetComponent<ParticleSystem>().Stop();
    }

    public void OnClickUnactive(GameObject go)
    {
        go.SetActive(false);
    }

    public void GetData()
    {
        lineChart.GetComponent<SimplifiedLineChart>().AddData("serie1", listA[val], listB[val]);
        val++;
    }*/
}
