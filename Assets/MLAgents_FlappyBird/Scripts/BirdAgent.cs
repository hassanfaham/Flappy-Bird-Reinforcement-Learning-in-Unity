using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;
using System.Linq;

public class BirdAgent : Agent {

    [SerializeField] private Level level;
    private Bird bird;
    private bool isJumpInputDown;

    private List<int> actionsTaken;
    

    // private List<float> stateVector;
    private List<List<float>> allStates;

    


    private void Awake() {
        bird = GetComponent<Bird>();
        bird.OnDied += Bird_OnDied;
        level.OnPipePassed += Level_OnPipePassed;
        actionsTaken = new List<int>();
        // stateVector = new List<float>();
        allStates = new List<List<float>>();
        
    }

    private void Level_OnPipePassed(object sender, System.EventArgs e) {
        AddReward(0.5f);
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        SetReward(-1f);
        EndEpisode();
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        // gameObject.SetActive(false);
        // Loader.Load(Loader.Scene.GameScene);
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isJumpInputDown = true;
        }
    }

    public override void OnEpisodeBegin() {

        actionsTaken.Clear();
        // stateVector.Clear();
        allStates.Clear();
        
        // bird.Reset();
        // level.Reset();
    }

    public override void CollectObservations(VectorSensor sensor) {


        List<float> state = new List<float>();


        float worldHeight = 100f;
        float birdNormalizedY = (bird.transform.position.y + (worldHeight / 2f)) / worldHeight;
        sensor.AddObservation(birdNormalizedY);
        state.Add(birdNormalizedY);

        float pipeSpawnXPosition = 100f;
        Level.PipeComplete pipeComplete = level.GetNextPipeComplete();
        if (pipeComplete != null && pipeComplete.pipeBottom != null && pipeComplete.pipeBottom.pipeBodyTransform != null) {
            sensor.AddObservation(pipeComplete.pipeBottom.GetXPosition() / pipeSpawnXPosition);
            state.Add(pipeComplete.pipeBottom.GetXPosition() / pipeSpawnXPosition);
        }else {
            sensor.AddObservation(1f);
            // state.Add(1f);
        }

        sensor.AddObservation(bird.GetVelocityY() / 200f);
        state.Add(bird.GetVelocityY() / 200f);

        allStates.Add(state);


        

    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (actions.DiscreteActions[0] == 1) {
            bird.Jump();
        }
    
        bool isWithinBoundaries =  bird.transform.position.y < 50f && bird.transform.position.y > -50f;
        if ( isWithinBoundaries) {
            SetReward(0.1f);
        }else{
            SetReward(-1f);
        }


        // AddReward(10f / MaxStep);

        //Debug.Log(GetCumulativeReward());

        int action = actions.DiscreteActions[0];
        actionsTaken.Add(action);


    }


    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = isJumpInputDown ? 1 : 0;

        isJumpInputDown = false;
    }


    public void EndEpisode() {
        // string output = "[" + string.Join(", ", actionsTaken) + "]";
        // Debug.Log(actionsTaken);
        string modelName = null;
        string path_act = "F:\\Unity_env\\ml-agents-release_20\\vectors\\action vec\\";
        string path_stat = "F:\\Unity_env\\ml-agents-release_20\\vectors\\state vec\\";
        if(bird.GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().Model == null){
            SaveListToFileAct(actionsTaken,"Player_1",path_act+"player\\");
            SaveListToFileStat(allStates,"Player_1",path_stat+"player\\");
        }else{
            modelName = bird.GetComponent<Unity.MLAgents.Policies.BehaviorParameters>().Model.name;
            Debug.Log(modelName);
            SaveListToFileAct(actionsTaken,modelName,path_act+"agent\\");
            SaveListToFileStat(allStates,modelName,path_stat+"agent\\");
            
        }
        
        
        
        // string output2 = "[" + string.Join(", ", allStates.Select(sublist => "[" + string.Join(", ", sublist) + "]")) + "]";
        // Debug.Log(output2); 

    }

    public void SaveListToFileStat(List<List<float>> list, String fileName,String path)
    {
        string path_ = path+fileName+".txt";
        
        // Convert the list to a string of comma-separated values
        string content = "[" + string.Join(", ", list.Select(sublist => "[" + string.Join(", ", sublist) + "]")) + "]";;
        // Write the content to the file
        
        File.AppendAllText(path_, content + Environment.NewLine);
        Debug.Log("Saving states vector has been done successfully !");
    }



    public void SaveListToFileAct(List<int> list, String fileName,String path)
    {
        string path_ = path+fileName+".txt";
        // Convert the list to a string of comma-separated values
        string content = "[" + string.Join(",", list) + "]";
        // Write the content to the file
        
        File.AppendAllText(path_, content + Environment.NewLine);
        Debug.Log("Saving actions vector has been done successfully !");
    }


}
