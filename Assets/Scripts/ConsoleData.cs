using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using XCharts.Runtime;
using TMPro;
using System;

public class ConsoleData : MonoBehaviour
{
    public List<string> Altitude = new List<string>();
    public List<string> RelativeVelocity = new List<string>();
    public List<string> Range = new List<string>();

    public List<string> GroundTraceLat = new List<string>();
    public List<string> GroundTraceLong = new List<string>();



    /// <summary>
    /// For ascent phase event child index at 1 - HS200, 2- CES, 3 - L110, 4 - SM Fairing Sep, 5 - OM-LV, 6 - Solar 
    /// </summary>
    public GameObject AscentPhaseEvents;
    /// <summary>
    /// For CES Abort phase event child index at 1 - CES-LV Sep, 2- CES-CM Sep, 3 - Apex Cover Sep, 4 - Drogue chute deploy , 5 - Main chute deploy , 6 - Main chute release
    /// </summary>
    public GameObject CESAbortPhaseEvents;
    /// <summary>
    /// For SM Abort phase event child index at 1 - L110 / C25 shut-off, 2- OM-LV sep, 3 - SM Burn Maneuver Start, 4 - SM Burn Maneuver End , 5 - CM-SM Sep, 6 - Apex Cover Sep, 7 - Drogue Chute Deploy, 8 - Main Chute Deploy, 9 - Main Chute Release
    /// </summary>
    public GameObject SMAbortPhaseEvents;
    /// <summary>
    /// For Descent phase event child index at 1 - Deboost Firing Start, 2 - Deboost Firing End, 3 - CM-SM Sep, 4 - Apex Cover Sep , 5 - Drogue Chute Deploy, 6 - Main Chute Deploy , 7 - Main Chute Release
    /// </summary>
    public GameObject DescentPhaseEvents;

    public GameObject AltitudeVelocityGraph;
    public GameObject GroundTraceGraph;

    private int initialZoomVal = 0;
    private void Awake()
    {
        List<string> mainFile = File.ReadLines("Assets/Resources/simulation.txt").ToList();

        foreach (string line in mainFile)
        {
             DatapackForValidValue15(line);
             DatapackForValidValue17(line);
        }
    }

    /// <summary>
    /// This function deals with all the data we recieve from the data pack with valid value 15
    /// </summary>
    /// <param name="line"></param>
    private void DatapackForValidValue15(string line)
    {
        if (line.Substring(0, 4) == "[15]")
        {
            List<string> allVariables = line.Split(" ").ToList();

            int i = 0;
            foreach (string var in allVariables)
            {
                if (i > 0 && i < 501)
                    Altitude.Add(var);
                else if (i >= 500 && i < 1001)
                    RelativeVelocity.Add(var);
                else if (i >= 1001 && i < 1501)
                    Range.Add(var);
                i++;
            }
            PlotAltitudeGraph(100);
            if (allVariables.Count > 0)
            {
                SetEstimatedVariables(allVariables);
            }
        }
    }

    /// <summary>
    /// this function set all the variables for the event phases
    /// </summary>
    /// <param name="allVariables"></param>
    private void SetEstimatedVariables(List<string> allVariables)
    {
        AscentPhaseEvents.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1502]).ToString("D6"); //HS200 
        AscentPhaseEvents.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1504]).ToString("D6"); //CES-CM Separation 
        AscentPhaseEvents.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1503]).ToString("D6"); //L110 Separation 
        AscentPhaseEvents.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1513]).ToString("D6"); //SM Fairing Separation
        AscentPhaseEvents.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1505]).ToString("D6"); //OM-LV Separation
        AscentPhaseEvents.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1512]).ToString("D6"); //Solar Panel Deployment

        CESAbortPhaseEvents.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1518]).ToString("D6"); //CES-LV Separation
        CESAbortPhaseEvents.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1504]).ToString("D6"); //CES-CM Separation 
        CESAbortPhaseEvents.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1509]).ToString("D6"); //Apex Cover Separation
        CESAbortPhaseEvents.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1510]).ToString("D6"); //Drogue Chute Deploy
        CESAbortPhaseEvents.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1511]).ToString("D6"); //Main Chute Deploy
        CESAbortPhaseEvents.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1514]).ToString("D6"); //Main Chute Release 

        SMAbortPhaseEvents.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1519]).ToString("D6"); //L110 / C25 shut-off
        SMAbortPhaseEvents.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1505]).ToString("D6"); //OM-LV Separation 
        SMAbortPhaseEvents.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1516]).ToString("D6"); //SM Burn Maneuver start
        SMAbortPhaseEvents.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1517]).ToString("D6"); //SM Burn Maneuver end
        SMAbortPhaseEvents.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1508]).ToString("D6"); //CM-SM Separation
        SMAbortPhaseEvents.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1509]).ToString("D6"); //Apex Cover Separation
        SMAbortPhaseEvents.transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1510]).ToString("D6"); //Drogue Chute Deploy
        SMAbortPhaseEvents.transform.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1511]).ToString("D6"); //Main Chute Deploy
        SMAbortPhaseEvents.transform.GetChild(9).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1514]).ToString("D6"); //Main Chute Release

        DescentPhaseEvents.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1506]).ToString("D6"); //Deboost Firing Start
        DescentPhaseEvents.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1507]).ToString("D6"); //Deboost Firing End
        DescentPhaseEvents.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1508]).ToString("D6"); //CM-SM Separation
        DescentPhaseEvents.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1509]).ToString("D6"); //Apex Cover Separation
        DescentPhaseEvents.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1510]).ToString("D6"); //Drogue Chute Deploy
        DescentPhaseEvents.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1511]).ToString("D6"); //Main Chute Deploy
        DescentPhaseEvents.transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>().text = Int32.Parse(allVariables[1514]).ToString("D6"); //Main Chute Release
    }

    public void PlotAltitudeGraph(int count)
    {
        count += initialZoomVal;
        if (count > 500)
            return;

        for (int i = initialZoomVal; i < count; i++)
        {
            AltitudeVelocityGraph.GetComponent<LineChart>().AddData("Altitude", int.Parse(Range[i]), int.Parse(Altitude[i]));
            AltitudeVelocityGraph.GetComponent<LineChart>().AddData("RelativeVelocity", int.Parse(Range[i]), int.Parse(RelativeVelocity[i]));
        }
        initialZoomVal = count;
    }
    public void ZoomOutAltitudeGraph(int count)
    {
        count = initialZoomVal - count;
        if (count < 100)
            return;

        AltitudeVelocityGraph.GetComponent<LineChart>().ClearData();
        for (int i = 0; i < count; i++)
        {
            AltitudeVelocityGraph.GetComponent<LineChart>().AddData("Altitude", int.Parse(Range[i]), int.Parse(Altitude[i]));
            AltitudeVelocityGraph.GetComponent<LineChart>().AddData("RelativeVelocity", int.Parse(Range[i]), int.Parse(RelativeVelocity[i]));
        }
        initialZoomVal = count;
    }

    private void DatapackForValidValue17(string line)
    {
        if (line.Substring(0, 4) == "[17]")
        {
            List<string> allVariables = line.Split(" ").ToList();

            int i = 0;
            foreach (string var in allVariables)
            {
                if (i > 0 && i < 1125)
                    GroundTraceLat.Add(var);
                else if (i >= 1126 && i < 2251)
                    GroundTraceLong.Add(var);
                i++;
            }
            PlotGroundTraceGraph();
        }

    }

    private void PlotGroundTraceGraph()
    {
        for (int i = initialZoomVal; i < GroundTraceLat.Count; i++)
        {
            GroundTraceGraph.GetComponent<LineChart>().AddData("GroundTrack", int.Parse(GroundTraceLong[i]), int.Parse(GroundTraceLat[i]));
        }
    }
}
